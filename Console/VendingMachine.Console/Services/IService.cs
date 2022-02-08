
namespace VendingMachine.Console.Services
{
    public interface IService<T>
    {
        public Task<T> GetAsync();
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> PostAsync(T dto);
        public Task<T> PutAsync(T dto, int id);

    }
}
