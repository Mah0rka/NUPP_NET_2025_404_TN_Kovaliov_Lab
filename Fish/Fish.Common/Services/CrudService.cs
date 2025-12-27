namespace Fish.Common.Services
{
    // Generic CRUD сервіс
    public class CrudService<T> : ICrudService<T> where T : class
    {
        // Колекція для зберігання даних
        private List<T> _items;

        // Делегат для отримання ID з елемента
        private readonly Func<T, Guid> _getIdFunc;

        // Конструктор
        public CrudService(Func<T, Guid> getIdFunc)
        {
            _items = new List<T>();
            _getIdFunc = getIdFunc;
        }

        // Метод створення
        public void Create(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            _items.Add(element);
            Console.WriteLine($"Створено новий елемент з ID: {_getIdFunc(element)}");
        }

        // Метод читання
        public T? Read(Guid id)
        {
            return _items.FirstOrDefault(item => _getIdFunc(item) == id);
        }

        // Метод читання всіх
        public IEnumerable<T> ReadAll()
        {
            return _items;
        }

        // Метод оновлення
        public void Update(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var id = _getIdFunc(element);
            var existingItem = Read(id);
            
            if (existingItem == null)
            {
                Console.WriteLine($"Елемент з ID {id} не знайдено");
                return;
            }

            var index = _items.IndexOf(existingItem);
            _items[index] = element;
            Console.WriteLine($"Оновлено елемент з ID: {id}");
        }

        // Метод видалення
        public void Remove(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var removed = _items.Remove(element);
            if (removed)
            {
                Console.WriteLine($"Видалено елемент з ID: {_getIdFunc(element)}");
            }
            else
            {
                Console.WriteLine($"Елемент не знайдено");
            }
        }

        // Метод для підрахунку кількості елементів
        public int Count()
        {
            return _items.Count;
        }
    }
}

