using Microsoft.Extensions.Configuration;
using VendingMachine.Console.Model.DTOs;
using VendingMachine.Console.WebClient;

namespace VendingMachine.Console.Services.ProductContext
{
    public class ProductService : IProductService
    {
        protected readonly IConfiguration _configuration;
        private   readonly HttpRESTClient<ProductDTO> _httpClient;

        private   int    port;
        private   string endpoint;

        public ProductService(IConfiguration configuration, IHttpRESTClient<ProductDTO> httpRESTClient)
        {
            _configuration = configuration;
            _httpClient    = (HttpRESTClient<ProductDTO>) httpRESTClient;
            port           = Int32.Parse(configuration["Product:Port"]);
            endpoint       = configuration["Product:Endpoint"];

            httpRESTClient.Url.Port     = port;
            httpRESTClient.Url.Endpoint = endpoint;
        }

        // alternate pattern. Observable
        public async Task<ProductDTO> GetAsync()
        {
            return await _httpClient.Get();
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            return await _httpClient.GetAll();
        }

        public async Task<ProductDTO> PostAsync(ProductDTO product)
        {
            return await _httpClient.Post(product);
        }

        public async Task<ProductDTO> PutAsync(ProductDTO product, int id)
        {
            return await _httpClient.Put(product, id);
        }
    }
}