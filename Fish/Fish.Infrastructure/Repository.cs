using Microsoft.EntityFrameworkCore;

namespace Fish.Infrastructure
{
    // Реалізація репозиторію для роботи з базою даних
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FishContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(FishContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Отримати елемент за ID
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Отримати всі елементи
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Додати новий елемент
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Оновити елемент
        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        // Видалити елемент
        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        // Зберегти зміни в базі даних
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

