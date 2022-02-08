namespace VendingMachine.Console.WebClient
{
    public class UrlStr
    {
        public UrlStr() {}
        public UrlStr(string protocol, string host, int port, string path, string endpoint, string urlId) 
        {
            Protocol = protocol;
            Host     = host;
            Port     = port;
            Path     = path;
            Endpoint = endpoint;
            UrlId    = urlId;
        }

        public int    Port     { get; set; }
        public string Endpoint { get; set; }
        public string Protocol { get; set; }
        public string Host     { get; set; }
        public string Path     { get; set; }
        public string UrlId    { get; set; }   

        public override string ToString()
        {
            return $"{Protocol}://{Host}:{Port}/{Path}{Endpoint}/{UrlId}";
        }
    }

}
