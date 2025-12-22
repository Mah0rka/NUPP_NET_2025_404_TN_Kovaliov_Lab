using Fish.Common;
using Fish.Common.Services;
using Fish.Common.Extensions;

namespace Fish.Console
{
    class Program
    {
        // Лічильник для демонстрації lock
        private static int _counter = 0;
        private static readonly object _lockObject = new object();

        static async Task Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створення асинхронного CRUD сервісу
            var fishService = new CrudServiceAsync<FishBase>(fish => fish.Id, "fish_collection.json");

            // Демонстрація паралельного створення об'єктів
            System.Console.WriteLine("=== Паралельне створення риб ===");
            System.Console.WriteLine("Створюємо 1200 об'єктів риб паралельно...\n");

            var startTime = DateTime.Now;

            // Створення 1200 риб (по 400 кожного типу)
            await Task.Run(() =>
            {
                Parallel.For(0, 400, async i =>
                {
                    var freshwater = FreshwaterFish.CreateNew();
                    await fishService.CreateAsync(freshwater);
                });
            });

            await Task.Run(() =>
            {
                Parallel.For(0, 400, async i =>
                {
                    var saltwater = SaltwaterFish.CreateNew();
                    await fishService.CreateAsync(saltwater);
                });
            });

            await Task.Run(() =>
            {
                Parallel.For(0, 400, async i =>
                {
                    var migratory = MigratoryFish.CreateNew();
                    await fishService.CreateAsync(migratory);
                });
            });

            var endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalMilliseconds;

            System.Console.WriteLine($"Створено {fishService.Count()} об'єктів риб");
            System.Console.WriteLine($"Час виконання: {duration:F2} мс\n");

            // LINQ - отримання статистики
            System.Console.WriteLine("=== LINQ: Статистика по рибах ===\n");

            var allFish = await fishService.ReadAllAsync();
            
            // Статистика для прісноводних риб
            var freshwaterFishes = allFish.OfType<FreshwaterFish>().ToList();
            if (freshwaterFishes.Any())
            {
                System.Console.WriteLine("Прісноводні риби:");
                System.Console.WriteLine($"  Кількість: {freshwaterFishes.Count}");
                System.Console.WriteLine($"  Температура: Min={freshwaterFishes.Min(f => f.PreferredTemperature):F1}°C, " +
                                       $"Max={freshwaterFishes.Max(f => f.PreferredTemperature):F1}°C, " +
                                       $"Avg={freshwaterFishes.Average(f => f.PreferredTemperature):F1}°C");
                System.Console.WriteLine($"  pH: Min={freshwaterFishes.Min(f => f.PhLevel):F1}, " +
                                       $"Max={freshwaterFishes.Max(f => f.PhLevel):F1}, " +
                                       $"Avg={freshwaterFishes.Average(f => f.PhLevel):F1}");
                System.Console.WriteLine($"  Розмір акваріума: Min={freshwaterFishes.Min(f => f.TankSize)}л, " +
                                       $"Max={freshwaterFishes.Max(f => f.TankSize)}л, " +
                                       $"Avg={freshwaterFishes.Average(f => f.TankSize):F1}л\n");
            }

            // Статистика для морських риб
            var saltwaterFishes = allFish.OfType<SaltwaterFish>().ToList();
            if (saltwaterFishes.Any())
            {
                System.Console.WriteLine("Морські риби:");
                System.Console.WriteLine($"  Кількість: {saltwaterFishes.Count}");
                System.Console.WriteLine($"  Максимальна глибина: Min={saltwaterFishes.Min(f => f.MaxDepth)}м, " +
                                       $"Max={saltwaterFishes.Max(f => f.MaxDepth)}м, " +
                                       $"Avg={saltwaterFishes.Average(f => f.MaxDepth):F1}м");
                System.Console.WriteLine($"  Толерантність до солі: Min={saltwaterFishes.Min(f => f.SaltTolerance):F1}%, " +
                                       $"Max={saltwaterFishes.Max(f => f.SaltTolerance):F1}%, " +
                                       $"Avg={saltwaterFishes.Average(f => f.SaltTolerance):F1}%");
                System.Console.WriteLine($"  Сумісні з рифами: {saltwaterFishes.Count(f => f.CoralReefCompatible)} " +
                                       $"({saltwaterFishes.Count(f => f.CoralReefCompatible) * 100.0 / saltwaterFishes.Count:F1}%)\n");
            }

            // Статистика для мігруючих риб
            var migratoryFishes = allFish.OfType<MigratoryFish>().ToList();
            if (migratoryFishes.Any())
            {
                System.Console.WriteLine("Мігруючі риби:");
                System.Console.WriteLine($"  Кількість: {migratoryFishes.Count}");
                System.Console.WriteLine($"  Відстань міграції: Min={migratoryFishes.Min(f => f.MigrationDistance):F0}км, " +
                                       $"Max={migratoryFishes.Max(f => f.MigrationDistance):F0}км, " +
                                       $"Avg={migratoryFishes.Average(f => f.MigrationDistance):F0}км");
                
                var seasonGroups = migratoryFishes.GroupBy(f => f.MigrationSeason);
                System.Console.WriteLine("  Розподіл по сезонах:");
                foreach (var group in seasonGroups.OrderByDescending(g => g.Count()))
                {
                    System.Console.WriteLine($"    {group.Key}: {group.Count()} ({group.Count() * 100.0 / migratoryFishes.Count:F1}%)");
                }
                System.Console.WriteLine();
            }

            // Загальна статистика по швидкості та довжині
            System.Console.WriteLine("Загальна статистика:");
            System.Console.WriteLine($"  Швидкість: Min={allFish.Min(f => f.FishType.TopSpeed)} км/год, " +
                                   $"Max={allFish.Max(f => f.FishType.TopSpeed)} км/год, " +
                                   $"Avg={allFish.Average(f => f.FishType.TopSpeed):F1} км/год");
            System.Console.WriteLine($"  Довжина: Min={allFish.Min(f => f.FishType.Length):F2}м, " +
                                   $"Max={allFish.Max(f => f.FishType.Length):F2}м, " +
                                   $"Avg={allFish.Average(f => f.FishType.Length):F2}м");
            System.Console.WriteLine($"  Хижаки: {allFish.Count(f => f.FishType.IsPredatory)} " +
                                   $"({allFish.Count(f => f.FishType.IsPredatory) * 100.0 / allFish.Count():F1}%)\n");

            // Демонстрація пагінації
            System.Console.WriteLine("=== Демонстрація пагінації ===");
            int pageSize = 10;
            var firstPage = await fishService.ReadAllAsync(1, pageSize);
            System.Console.WriteLine($"Перша сторінка (10 елементів):");
            foreach (var fish in firstPage.Take(5))
            {
                System.Console.WriteLine($"  - {fish.FishType.Variety} (ID: {fish.Id.ToString()[..8]}...)");
            }
            System.Console.WriteLine($"  ... та ще {pageSize - 5} елементів\n");

            // Демонстрація примітивів синхронізації
            System.Console.WriteLine("=== Примітиви синхронізації ===\n");

            // 1. Lock - захист критичної секції
            System.Console.WriteLine("1. Lock - захист лічильника:");
            _counter = 0;
            var lockTasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                lockTasks.Add(Task.Run(() => IncrementWithLock()));
            }
            await Task.WhenAll(lockTasks);
            System.Console.WriteLine($"   Результат: {_counter} (очікувалось: 1000)\n");

            // 2. SemaphoreSlim - обмеження кількості одночасних операцій
            System.Console.WriteLine("2. SemaphoreSlim - обмеження до 3 одночасних операцій:");
            var semaphore = new SemaphoreSlim(3, 3);
            var semaphoreTasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                int taskId = i + 1;
                semaphoreTasks.Add(Task.Run(async () => await WorkWithSemaphore(semaphore, taskId)));
            }
            await Task.WhenAll(semaphoreTasks);
            System.Console.WriteLine();

            // 3. AutoResetEvent - сигналізація між потоками
            System.Console.WriteLine("3. AutoResetEvent - сигналізація між потоками:");
            var autoResetEvent = new AutoResetEvent(false);
            var waiterTask = Task.Run(() =>
            {
                System.Console.WriteLine("   Потік чекає на сигнал...");
                autoResetEvent.WaitOne();
                System.Console.WriteLine("   Потік отримав сигнал і продовжує роботу!");
            });
            
            await Task.Delay(1000);
            System.Console.WriteLine("   Головний потік надсилає сигнал...");
            autoResetEvent.Set();
            await waiterTask;
            System.Console.WriteLine();

            // 4. Monitor - Wait/Pulse pattern
            System.Console.WriteLine("4. Monitor - Wait/Pulse pattern:");
            var monitorLock = new object();
            var producerTask = Task.Run(() => ProducerWithMonitor(monitorLock));
            var consumerTask = Task.Run(() => ConsumerWithMonitor(monitorLock));
            await Task.WhenAll(producerTask, consumerTask);
            System.Console.WriteLine();

            // Збереження колекції у файл
            System.Console.WriteLine("=== Збереження у файл ===");
            var saved = await fishService.SaveAsync();
            if (saved)
            {
                var fileInfo = new FileInfo(fishService.FilePath);
                System.Console.WriteLine($"Колекція збережена у файл: {fishService.FilePath}");
                System.Console.WriteLine($"Розмір файлу: {fileInfo.Length / 1024.0:F2} KB\n");
            }

            // Демонстрація IEnumerable
            System.Console.WriteLine("=== Демонстрація IEnumerable ===");
            System.Console.WriteLine("Перші 5 риб через foreach:");
            int count = 0;
            foreach (var fish in fishService)
            {
                if (count++ >= 5) break;
                System.Console.WriteLine($"  - {fish.FishType.Variety}");
            }

            System.Console.WriteLine("\n=== Завершено ===");
            System.Console.WriteLine($"Всього створено риб: {FishBase.TotalFishCount}");
        }

        // Метод для демонстрації lock
        static void IncrementWithLock()
        {
            for (int i = 0; i < 100; i++)
            {
                lock (_lockObject)
                {
                    _counter++;
                }
            }
        }

        // Метод для демонстрації SemaphoreSlim
        static async Task WorkWithSemaphore(SemaphoreSlim semaphore, int taskId)
        {
            await semaphore.WaitAsync();
            try
            {
                System.Console.WriteLine($"   Завдання {taskId} виконується...");
                await Task.Delay(500);
                System.Console.WriteLine($"   Завдання {taskId} завершено");
            }
            finally
            {
                semaphore.Release();
            }
        }

        // Producer для Monitor
        static void ProducerWithMonitor(object lockObj)
        {
            lock (lockObj)
            {
                System.Console.WriteLine("   Producer: готує дані...");
                Thread.Sleep(1000);
                System.Console.WriteLine("   Producer: дані готові, надсилає сигнал");
                Monitor.Pulse(lockObj);
            }
        }

        // Consumer для Monitor
        static void ConsumerWithMonitor(object lockObj)
        {
            lock (lockObj)
            {
                System.Console.WriteLine("   Consumer: чекає на дані...");
                Monitor.Wait(lockObj);
                System.Console.WriteLine("   Consumer: отримав дані та обробляє їх");
            }
        }
    }
}
