using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;

namespace VendingMachine.Console.WebClient
{
    public class HttpRESTClient<T> : IDisposable, IHttpRESTClient<T> where T : class
    {
        static readonly HttpClient client = new HttpClient();

        static readonly TimeSpan timeout  = new TimeSpan(0, 0, 0, 0, -1);

        RESTResult<T> tResult;
        public RESTEnum Method  { get; set; }
        public UrlStr   Url     { get; set; }

        public HttpRESTClient(IConfiguration configuration)
        {
            Url            = new UrlStr();
            tResult        = new RESTResult<T>();

            Url.Protocol   = configuration["Protocol"];
            Url.Host       = configuration["Host"];
            Url.Path       = configuration["Url"];

            if (client is null) client.Timeout = timeout;
        }

        public Task<IEnumerable<T>> GetAll(string content = "")
        {
            Method = RESTEnum.GET;
            return SendGetAllNoAuthRequest(content);
        }

        public Task<T> Get(string content = "")
        {
            Method = RESTEnum.GET;
            return SendNoAuthRequest(content);
        }

        public Task<T> Post(T content)
        {
            Method = RESTEnum.POST;
            return SendNoAuthRequest(parseOutput(content));
        }

        public Task<T> Put(T content, int id)
        {
            Url.UrlId = id.ToString();
            Method    = RESTEnum.PUT;
            return SendNoAuthRequest(parseOutput(content));
        }

        private async Task<T> SendNoAuthRequest(string content = "")
        {
            string retString = await Send(content);
            if (retString is null)
                return null;
            return await parseInput(retString);
        }

        private async Task<IEnumerable<T>> SendGetAllNoAuthRequest(string content = "")
        {
            string retString = await Send(content);
            if (retString is null)
                return null;
            return await parseAllInput(retString);
        }

        // Parser: TODO move to a parser class
        private string                     parseOutput(T obj)            => tResult.Serialize(obj);
        private async Task<T>              parseInput(string content)    => await new RESTResult<T>().Deserialize(content);
        private async Task<IEnumerable<T>> parseAllInput(string content) => await new RESTResult<IEnumerable<T>>().Deserialize(content);
        // End parser

        public async Task<string> Send(string content)
        {
            string result = "";
            string url    = Url.ToString();
            string method = RESTUtils.getName(Method);

            HttpMethod httpMethod        = new HttpMethod(method);
            HttpRequestMessage  request  = new HttpRequestMessage(httpMethod, url);
            HttpResponseMessage response = new HttpResponseMessage();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (!String.IsNullOrWhiteSpace(content))
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            try
            {
                using (response = await client.SendAsync(request).ConfigureAwait(false))
                    result = await response.Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);
            }
            catch { throw; }
            return result;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
