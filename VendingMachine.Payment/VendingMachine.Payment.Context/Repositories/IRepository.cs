namespace ProductContextApi.Repositories
{
    public interface IRepository<T>
    {
        void AddItem(T item);
        IEnumerable<T> FetchAll();
        void DeleteItem(int id);
        void SubmitChanges();
    }
}
