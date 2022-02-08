namespace VendingMachine.Console.WebClient
{
    public interface IHttpRESTClient<T> where T : class 
    {
        public UrlStr  Url { get; set; }
        public Task<T> Get(string content = "");
        public Task<T> Post(T content);
    }
}
