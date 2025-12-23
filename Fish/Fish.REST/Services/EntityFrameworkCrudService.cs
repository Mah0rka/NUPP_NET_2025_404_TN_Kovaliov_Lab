using System.Collections;
using Fish.Common.Services;
using Fish.Infrastructure;

namespace Fish.REST.Services
{
    // CRUD сервіс що працює з Entity Framework через репозиторій
    public class EntityFrameworkCrudService<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly Func<T, Guid> _getExternalIdFunc;
        private readonly Func<T, int> _getIdFunc;
        private readonly Action<T, Guid> _setExternalIdAction;

        public EntityFrameworkCrudService(
            IRepository<T> repository,
            Func<T, Guid> getExternalIdFunc,
            Func<T, int> getIdFunc,
            Action<T, Guid> setExternalIdAction)
        {
            _repository = repository;
            _getExternalIdFunc = getExternalIdFunc;
            _getIdFunc = getIdFunc;
            _setExternalIdAction = setExternalIdAction;
        }

        // Створення елемента
        public async Task<bool> CreateAsync(T element)
        {
            if (element == null)
                return false;

            try
            {
                // Генеруємо новий Guid якщо потрібно
                if (_getExternalIdFunc(element) == Guid.Empty)
                {
                    _setExternalIdAction(element, Guid.NewGuid());
                }

                await _repository.AddAsync(element);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Читання елемента за Guid
        public async Task<T?> ReadAsync(Guid id)
        {
            var allItems = await _repository.GetAllAsync();
            return allItems.FirstOrDefault(x => _getExternalIdFunc(x) == id);
        }

        // Читання всіх елементів
        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Читання з пагінацією
        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            if (page < 1 || amount < 1)
                return new List<T>();

            var allItems = await _repository.GetAllAsync();
            return allItems.Skip((page - 1) * amount).Take(amount);
        }

        // Оновлення елемента
        public async Task<bool> UpdateAsync(T element)
        {
            if (element == null)
                return false;

            try
            {
                await _repository.UpdateAsync(element);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Видалення елемента
        public async Task<bool> RemoveAsync(T element)
        {
            if (element == null)
                return false;

            try
            {
                await _repository.DeleteAsync(element);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Збереження (вже викликається в інших методах)
        public async Task<bool> SaveAsync()
        {
            try
            {
                await _repository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Реалізація IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            return _repository.GetAllAsync().Result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}


