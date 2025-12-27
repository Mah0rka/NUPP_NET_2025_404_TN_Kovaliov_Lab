namespace Fish.Common.Services
{
    // Інтерфейс для асинхронних CRUD операцій
    public interface ICrudServiceAsync<T> : IEnumerable<T> where T : class
    {
        // Створення елемента
        Task<bool> CreateAsync(T element);
        
        // Читання елемента за ID
        Task<T?> ReadAsync(Guid id);
        
        // Читання всіх елементів
        Task<IEnumerable<T>> ReadAllAsync();
        
        // Читання з пагінацією
        Task<IEnumerable<T>> ReadAllAsync(int page, int amount);
        
        // Оновлення елемента
        Task<bool> UpdateAsync(T element);
        
        // Видалення елемента
        Task<bool> RemoveAsync(T element);
        
        // Збереження у файл
        Task<bool> SaveAsync();
    }
}

