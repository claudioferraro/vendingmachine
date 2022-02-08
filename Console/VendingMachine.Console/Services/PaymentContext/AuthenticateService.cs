using Microsoft.Extensions.Configuration;

namespace VendingMachine.Console.Services
{
    public class AuthenticateService
    {
        protected readonly IConfiguration _configuration;
        public AuthenticateService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
