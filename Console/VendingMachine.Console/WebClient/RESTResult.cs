using Newtonsoft.Json;

namespace VendingMachine.Console.WebClient
{
    internal class RESTResult<T>
    {
        public string Data { get; set; } = null;

        public RESTResult(string data = null)
        {
            Data = data;
        }

        public async Task<T> Deserialize(string data)
        {
            try
            {
                if (Data is null)
                    Data = data;
                var obj = await Task.Run(() => JsonConvert.DeserializeObject<T>(Data));
                return obj;
            }
            catch
            {
                return default(T);
            }
        }

        public string Serialize(T data) => JsonConvert.SerializeObject(data);
    }
}
