namespace VendingMachine.Console.WebClient
{
    public enum RESTEnum
    {
        GET,
        POST,
        PUT,
        DELETE,
        HEAD
    }

    public class RESTUtils
    {
        public static string getName(RESTEnum methodEnum)
        {
            return Enum.GetName(
                typeof(RESTEnum), 
                methodEnum
            );
        }
    }
}
