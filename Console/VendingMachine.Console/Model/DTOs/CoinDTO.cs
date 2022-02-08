namespace VendingMachine.Console.Model.DTOs
{
    public class CoinDTO
    {
        public CoinDTO(decimal value) { Value = value; }
        public decimal Value { get; set; }
        public string UserUUID { get; set; }
    }
}
