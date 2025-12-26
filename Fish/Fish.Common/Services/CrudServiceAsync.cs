using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Fish.Common.Services
{
    // Асинхронний CRUD сервіс з підтримкою багатопотоковості
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        // Thread-safe колекція для зберігання даних
        private readonly ConcurrentDictionary<Guid, T> _items;
        
        // Семафор для синхронізації операцій з файлами
        private readonly SemaphoreSlim _fileSemaphore;
        
        // Делегат для отримання ID з елемента
        private readonly Func<T, Guid> _getIdFunc;
        
        // Шлях до файлу для збереження
        public string FilePath { get; set; }
        
        // Опції для JSON серіалізації
        private readonly JsonSerializerOptions _jsonOptions;

        // Конструктор
        public CrudServiceAsync(Func<T, Guid> getIdFunc, string filePath = "data.json")
        {
            _items = new ConcurrentDictionary<Guid, T>();
            _fileSemaphore = new SemaphoreSlim(1, 1);
            _getIdFunc = getIdFunc;
            FilePath = filePath;
            
            // Налаштування JSON серіалізації
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        // Створення елемента
        public Task<bool> CreateAsync(T element)
        {
            if (element == null)
                return Task.FromResult(false);

            var id = _getIdFunc(element);
            return Task.FromResult(_items.TryAdd(id, element));
        }

        // Читання елемента за ID
        public Task<T?> ReadAsync(Guid id)
        {
            _items.TryGetValue(id, out var element);
            return Task.FromResult(element);
        }

        // Читання всіх елементів
        public Task<IEnumerable<T>> ReadAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(_items.Values.ToList());
        }

        // Читання з пагінацією
        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            if (page < 1 || amount < 1)
                return Task.FromResult<IEnumerable<T>>(new List<T>());

            var result = _items.Values
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToList();

            return Task.FromResult<IEnumerable<T>>(result);
        }

        // Оновлення елемента
        public Task<bool> UpdateAsync(T element)
        {
            if (element == null)
                return Task.FromResult(false);

            var id = _getIdFunc(element);
            
            if (!_items.ContainsKey(id))
                return Task.FromResult(false);

            _items[id] = element;
            return Task.FromResult(true);
        }

        // Видалення елемента
        public Task<bool> RemoveAsync(T element)
        {
            if (element == null)
                return Task.FromResult(false);

            var id = _getIdFunc(element);
            return Task.FromResult(_items.TryRemove(id, out _));
        }

        // Збереження у файл
        public async Task<bool> SaveAsync()
        {
            await _fileSemaphore.WaitAsync();
            try
            {
                var data = _items.Values.ToList();
                await using var stream = File.Create(FilePath);
                await JsonSerializer.SerializeAsync(stream, data, _jsonOptions);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження: {ex.Message}");
                return false;
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }

        // Завантаження з файлу
        public async Task<bool> LoadAsync()
        {
            await _fileSemaphore.WaitAsync();
            try
            {
                if (!File.Exists(FilePath))
                    return false;

                await using var stream = File.OpenRead(FilePath);
                var data = await JsonSerializer.DeserializeAsync<List<T>>(stream, _jsonOptions);
                
                if (data == null)
                    return false;

                _items.Clear();
                foreach (var item in data)
                {
                    var id = _getIdFunc(item);
                    _items.TryAdd(id, item);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження: {ex.Message}");
                return false;
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }

        // Підрахунок кількості елементів
        public int Count()
        {
            return _items.Count;
        }

        // Реалізація IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

