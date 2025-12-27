namespace Fish.Common.Services
{
    // Інтерфейс для CRUD операцій
    public interface ICrudService<T> where T : class
    {
        void Create(T element);
        T? Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);
    }
}

