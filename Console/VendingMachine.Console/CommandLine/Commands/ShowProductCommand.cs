using Microsoft.Extensions.Localization;
using System.Linq;
using System.Text;
using VendingMachine.Console.CommandLine.Commands.Interfaces;
using VendingMachine.Console.Model;
using VendingMachine.Console.Model.DTOs;
using VendingMachine.Console.Resources.ShowProduct;
using VendingMachine.Console.Services.ProductContext;

namespace VendingMachine.Console.CommandLine.Commands
{
    public class ShowProductCommand : CommandBase, IShowProductCommand
    {
        private readonly IStringLocalizer<ShowProductResource> _localizer = null!;

        private readonly IProductService         _productService;

        private readonly IProductStockService    _productStockService;

        private readonly IWallet                 _wallet;

        private readonly Change                  _change;

        private IEnumerable<ProductDTO>          _products;

        private Dictionary<int, ProductStockDTO> _productStocks;


        private ProductDTO           _selectedProduct;

        private ProductStockDTO _selectedProductStock;

        string[] allowedCommands = new string[] { "INT", "RETURN COINS" };

        int maxAttempts = 10;

        public ShowProductCommand(IStringLocalizer<ShowProductResource> localizer, 
                                  IProductService productService,
                                  IProductStockService productStockService, 
                                  IWallet wallet,
                                  IChange change) 
        {
            _localizer           = localizer;
            _productService      = productService;
            _productStockService = productStockService;
            _wallet              = wallet;
            _change              = (Change)change;
        }

        public override bool executeCommand()
        {
            IO.ClearScreen();

            IO.WriteLine(_localizer["Title"]);
            IO.WriteLine(String.Format("Your current credit is {0}{1}", _wallet.Credit.ToString(), "EUR"));
            IO.WriteLine(getProductString());
            IO.WriteLine(_localizer["Instruction2"], 1);
            IO.WriteLine(_localizer["Instruction"], 2);
            

            Tuple<string, string> inputKeyValue;
            do
            {
                maxAttempts--;
                inputKeyValue = IO.CatchMany(allowedCommands);
                switch (inputKeyValue.Item1)
                {
                    case "INT":
                    case "DECIMAL":
                        if (selectProductAndStore(inputKeyValue.Item2))
                            return finilizeCommand();
                        break;
                    case "RETURN COINS": return GoToBackup();
                    default: IO.Write(_localizer["InventoryError"]); break;
                }

                if (maxAttempts == 0) { IO.Write(_localizer["MaxAttempts"]); return GoToBackup(); }
            } while (inputKeyValue is not null);

            return finilizeCommand();
        }

        private bool finilizeCommand()
        {
            IO.Write(String.Format(_localizer["Success"], _selectedProduct.Name));
            Thread.Sleep(2000);
            IO.Write(_localizer["Continue"].Value);
            IO.CatchEnter();
            return GoToNext();
        }

        private bool selectProductAndStore(string number)
        {
            var product = _products.SingleOrDefault(p => p.Id == IOParser.ParseNumber(number));
            if (product is not null)
            { 
                 if (!_wallet.IsEnoughMoney(product.Price))
                    { IO.Write(_localizer["NoFunds"]); return false; }

                 _selectedProduct = product;

                 if (_productStocks.ContainsKey(product.Id))
                    _selectedProductStock = _productStocks[product.Id];

                 if (_productStocks.ContainsKey(product.Id) && _productStocks[product.Id].Stock == 0)
                     { IO.Write(_localizer["OutOfStock"]); return false; }

                 updateStockInformation();
                 createChange();
                 return true;
            }
            else
                IO.Write(_localizer["InventoryError"]);
            return false;
        }

        private void createChange()
        {
            _change.TotalAmount     = _selectedProduct.Price;
            _change.SelectedProduct = _selectedProduct;
            _change.Credit          = _wallet.Credit;
        }

        /* in a real UI scenario we can bubble up the async a bit more */
        private string getProductString()
        {
            try
            {
                Task<String> prodTask = getProductStringAsyc();
                prodTask.Wait();
                return prodTask.Result;
            }
            catch (Exception ex) { return $"{_localizer["NetworkError"].Value} Error Message: {ex.Message}"; }
        }

        private async Task<String> getProductStringAsyc()
        {
            StringBuilder retString = new StringBuilder();

            _products      = await _productService.GetAllAsync();
            _productStocks = await getStockInformationAsync();
            _products      = _products.OrderBy(p => p.Id);

            foreach (ProductDTO product in _products)
                if (_productStocks.ContainsKey(product.Id))
                    retString.Append($"{product.Id}. {product.Name}, " +
                        $"{string.Format("{0}", product.Price)} {product.Currency} (Left: {_productStocks[product.Id].Stock})\t{Environment.NewLine}");

            return retString.ToString();
        }

        private async Task<Dictionary<int, ProductStockDTO>> getStockInformationAsync()
        {
            Dictionary<int, ProductStockDTO> retProductStockDTO = new Dictionary<int, ProductStockDTO>();

            IEnumerable<ProductStockDTO> productStocks = await _productStockService.GetAllAsync();
            foreach(ProductStockDTO stock in productStocks)
                retProductStockDTO[stock.ProductId] = stock;

            return retProductStockDTO;
        }

        private ProductStockDTO updateStockInformation()
        {
            Task<ProductStockDTO> stockTask = _productStockService.PutAsync(_selectedProductStock, _selectedProductStock.ProductId);
            stockTask.Wait();
            return stockTask.Result;
        }
    }
}
