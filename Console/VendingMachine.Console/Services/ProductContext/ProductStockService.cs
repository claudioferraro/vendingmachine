using Microsoft.Extensions.Configuration;
using VendingMachine.Console.Model.DTOs;
using VendingMachine.Console.WebClient;

namespace VendingMachine.Console.Services.ProductContext
{
    public class ProductStockService : IProductStockService
    {
        protected readonly IConfiguration _configuration;
        private   readonly HttpRESTClient<ProductStockDTO> _httpClient;

        private   int    port;
        private   string endpoint;

        public ProductStockService(IConfiguration configuration, IHttpRESTClient<ProductStockDTO> httpRESTClient)
        {
            _configuration = configuration;
            _httpClient    = (HttpRESTClient<ProductStockDTO>) httpRESTClient;
            port           = Int32.Parse(configuration["Product:Port"]);
            endpoint       = $"{configuration["Product:Endpoint"]}Stock";

            httpRESTClient.Url.Port     = port;
            httpRESTClient.Url.Endpoint = endpoint;
        }

        // alternate pattern. Observable
        public async Task<ProductStockDTO> GetAsync()
        {
            return await _httpClient.Get();
        }

        public async Task<IEnumerable<ProductStockDTO>> GetAllAsync()
        {
            return await _httpClient.GetAll();
        }

        public async Task<ProductStockDTO> PostAsync(ProductStockDTO product)
        {
            return await _httpClient.Post(product);
        }

        public async Task<ProductStockDTO> PutAsync(ProductStockDTO product, int productId)
        {
            return await _httpClient.Put(product, productId);
        }
    }
}