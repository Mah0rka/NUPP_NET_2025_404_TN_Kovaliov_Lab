using Fish.Common;
using Fish.Infrastructure;
using Fish.Infrastructure.Models;
using Fish.NoSQL;
using Fish.NoSQL.Models;
using Microsoft.EntityFrameworkCore;

namespace Fish.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;


            // Налаштування PostgreSQL
            var connectionString = "Host=localhost;Database=FishDb;Username=postgres;Password=123";
            var optionsBuilder = new DbContextOptionsBuilder<FishContext>();
            optionsBuilder.UseNpgsql(connectionString);

            // Створення контексту та застосування міграцій
            using var context = new FishContext(optionsBuilder.Options);
            
            System.Console.WriteLine("Застосування міграцій до PostgreSQL...");
            try
            {
                await context.Database.MigrateAsync();
                System.Console.WriteLine("Міграції успішно застосовані!\n");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Помилка підключення до PostgreSQL: {ex.Message}");
                System.Console.WriteLine("Переконайтеся, що PostgreSQL запущено та доступний.\n");
            }

            // Налаштування MongoDB
            var mongoConnectionString = "mongodb+srv://admin:12345678+@fishdb.26j2crt.mongodb.net/";
            var mongoDatabase = "FishDb";
            var mongoCollection = "fishes";
            var mongoRepo = new MongoRepository<FishDocument>(mongoConnectionString, mongoDatabase, mongoCollection);

            System.Console.WriteLine("Підключення до MongoDB...");
            try
            {
                await mongoRepo.ClearAsync(); // Очищення колекції для чистого тесту
                System.Console.WriteLine("MongoDB підключено!\n");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Помилка підключення до MongoDB: {ex.Message}");
                System.Console.WriteLine("Переконайтеся, що MongoDB запущено та доступний.\n");
            }

            // Створення репозиторіїв для PostgreSQL
            var aquariumRepo = new Repository<AquariumModel>(context);
            var fishRepo = new Repository<FishModel>(context);
            var feedRepo = new Repository<FeedModel>(context);

            // Створення акваріумів
            System.Console.WriteLine("=== Створення акваріумів ===");
            var aquariums = new List<AquariumModel>
            {
                new AquariumModel { ExternalId = Guid.NewGuid(), Name = "Тропічний акваріум", Volume = 500, Location = "Зал 1" },
                new AquariumModel { ExternalId = Guid.NewGuid(), Name = "Морський акваріум", Volume = 1000, Location = "Зал 2" },
                new AquariumModel { ExternalId = Guid.NewGuid(), Name = "Річковий акваріум", Volume = 300, Location = "Зал 3" }
            };

            foreach (var aquarium in aquariums)
            {
                await aquariumRepo.AddAsync(aquarium);
            }
            await aquariumRepo.SaveChangesAsync();
            System.Console.WriteLine($"Створено {aquariums.Count} акваріумів\n");

            // Створення кормів
            System.Console.WriteLine("=== Створення кормів ===");
            var feeds = new List<FeedModel>
            {
                new FeedModel { Name = "Сухий корм", Type = "Універсальний", Price = 150.50 },
                new FeedModel { Name = "Живий корм", Type = "Спеціалізований", Price = 250.00 },
                new FeedModel { Name = "Заморожений корм", Type = "Преміум", Price = 320.75 }
            };

            foreach (var feed in feeds)
            {
                await feedRepo.AddAsync(feed);
            }
            await feedRepo.SaveChangesAsync();
            System.Console.WriteLine($"Створено {feeds.Count} типів корму\n");

            // Паралельне створення риб
            System.Console.WriteLine("=== Паралельне створення риб ===");
            System.Console.WriteLine("Створюємо 1200 об'єктів риб паралельно...\n");

            var startTime = DateTime.Now;
            var allFishList = new List<FishBase>();
            var lockObj = new object();

            // Створення 1200 риб (по 400 кожного типу) паралельно
            await Task.Run(() =>
            {
                Parallel.For(0, 400, i =>
                {
                    var freshwater = FreshwaterFish.CreateNew();
                    lock (lockObj)
                    {
                        allFishList.Add(freshwater);
                    }
                });
            });

            await Task.Run(() =>
            {
                Parallel.For(0, 400, i =>
                {
                    var saltwater = SaltwaterFish.CreateNew();
                    lock (lockObj)
                    {
                        allFishList.Add(saltwater);
                    }
                });
            });

            await Task.Run(() =>
            {
                Parallel.For(0, 400, i =>
                {
                    var migratory = MigratoryFish.CreateNew();
                    lock (lockObj)
                    {
                        allFishList.Add(migratory);
                    }
                });
            });

            var endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalMilliseconds;

            System.Console.WriteLine($"Створено {allFishList.Count} об'єктів риб");
            System.Console.WriteLine($"Час виконання: {duration:F2} мс\n");

            // Збереження в PostgreSQL
            System.Console.WriteLine("=== Збереження в PostgreSQL ===");
            var random = new Random();
            int savedToPostgres = 0;

            foreach (var fish in allFishList)
            {
                FishModel fishModel;
                
                if (fish is FreshwaterFish fw)
                {
                    fishModel = new FreshwaterFishModel
                    {
                        ExternalId = fish.Id,
                        Variety = fish.FishType.Variety,
                        Habitat = fish.FishType.Habitat,
                        TopSpeed = (int)fish.FishType.TopSpeed,
                        IsPredatory = fish.FishType.IsPredatory,
                        Length = fish.FishType.Length,
                        AquariumId = aquariums[random.Next(aquariums.Count)].Id,
                        PreferredTemperature = fw.PreferredTemperature,
                        PhLevel = fw.PhLevel,
                        TankSize = fw.TankSize
                    };
                }
                else if (fish is SaltwaterFish sw)
                {
                    fishModel = new SaltwaterFishModel
                    {
                        ExternalId = fish.Id,
                        Variety = fish.FishType.Variety,
                        Habitat = fish.FishType.Habitat,
                        TopSpeed = (int)fish.FishType.TopSpeed,
                        IsPredatory = fish.FishType.IsPredatory,
                        Length = fish.FishType.Length,
                        AquariumId = aquariums[random.Next(aquariums.Count)].Id,
                        SaltTolerance = sw.SaltTolerance,
                        MaxDepth = sw.MaxDepth,
                        CoralReefCompatible = sw.CoralReefCompatible
                    };
                }
                else if (fish is MigratoryFish mf)
                {
                    fishModel = new MigratoryFishModel
                    {
                        ExternalId = fish.Id,
                        Variety = fish.FishType.Variety,
                        Habitat = fish.FishType.Habitat,
                        TopSpeed = (int)fish.FishType.TopSpeed,
                        IsPredatory = fish.FishType.IsPredatory,
                        Length = fish.FishType.Length,
                        AquariumId = aquariums[random.Next(aquariums.Count)].Id,
                        MigrationDistance = mf.MigrationDistance,
                        SpawningGrounds = mf.SpawningGrounds,
                        MigrationSeason = mf.MigrationSeason
                    };
                }
                else
                {
                    continue;
                }

                // Додавання кормів (багато-до-багатьох)
                var feedCount = random.Next(1, 4);
                for (int i = 0; i < feedCount; i++)
                {
                    fishModel.Feeds.Add(feeds[random.Next(feeds.Count)]);
                }

                // Додавання деталей (один-до-одного)
                fishModel.Details = new FishDetailsModel
                {
                    BirthDate = DateTime.UtcNow.AddDays(-random.Next(30, 365)),
                    HealthStatus = random.Next(2) == 0 ? "Здорова" : "Відмінна",
                    Weight = Math.Round(random.NextDouble() * 2 + 0.1, 2)
                };

                await fishRepo.AddAsync(fishModel);
                savedToPostgres++;
            }

            await fishRepo.SaveChangesAsync();
            System.Console.WriteLine($"Збережено {savedToPostgres} риб в PostgreSQL\n");

            // Збереження в MongoDB
            System.Console.WriteLine("=== Збереження в MongoDB ===");
            int savedToMongo = 0;

            foreach (var fish in allFishList)
            {
                var fishDoc = new FishDocument
                {
                    ExternalId = fish.Id,
                    Variety = fish.FishType.Variety,
                    Habitat = fish.FishType.Habitat,
                    TopSpeed = (int)fish.FishType.TopSpeed,
                    IsPredatory = fish.FishType.IsPredatory,
                    Length = fish.FishType.Length
                };

                if (fish is FreshwaterFish fw)
                {
                    fishDoc.FishType = "Freshwater";
                    fishDoc.PreferredTemperature = fw.PreferredTemperature;
                    fishDoc.PhLevel = fw.PhLevel;
                    fishDoc.TankSize = fw.TankSize;
                }
                else if (fish is SaltwaterFish sw)
                {
                    fishDoc.FishType = "Saltwater";
                    fishDoc.SaltTolerance = sw.SaltTolerance;
                    fishDoc.MaxDepth = sw.MaxDepth;
                    fishDoc.CoralReefCompatible = sw.CoralReefCompatible;
                }
                else if (fish is MigratoryFish mf)
                {
                    fishDoc.FishType = "Migratory";
                    fishDoc.MigrationDistance = mf.MigrationDistance;
                    fishDoc.SpawningGrounds = mf.SpawningGrounds;
                    fishDoc.MigrationSeason = mf.MigrationSeason;
                }

                await mongoRepo.AddAsync(fishDoc);
                savedToMongo++;
            }

            System.Console.WriteLine($"Збережено {savedToMongo} риб в MongoDB\n");

            // LINQ статистика з PostgreSQL
            System.Console.WriteLine("=== LINQ: Статистика з PostgreSQL ===\n");

            var allFishFromDb = await fishRepo.GetAllAsync();
            var freshwaterFishes = allFishFromDb.OfType<FreshwaterFishModel>().ToList();
            var saltwaterFishes = allFishFromDb.OfType<SaltwaterFishModel>().ToList();
            var migratoryFishes = allFishFromDb.OfType<MigratoryFishModel>().ToList();

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

            if (migratoryFishes.Any())
            {
                System.Console.WriteLine("Мігруючі риби:");
                System.Console.WriteLine($"  Кількість: {migratoryFishes.Count}");
                System.Console.WriteLine($"  Відстань міграції: Min={migratoryFishes.Min(f => f.MigrationDistance):F0}км, " +
                                       $"Max={migratoryFishes.Max(f => f.MigrationDistance):F0}км, " +
                                       $"Avg={migratoryFishes.Average(f => f.MigrationDistance):F0}км\n");
            }

            // Загальна статистика
            System.Console.WriteLine("Загальна статистика (PostgreSQL):");
            System.Console.WriteLine($"  Всього риб: {allFishFromDb.Count()}");
            System.Console.WriteLine($"  Швидкість: Min={allFishFromDb.Min(f => f.TopSpeed)} км/год, " +
                                   $"Max={allFishFromDb.Max(f => f.TopSpeed)} км/год, " +
                                   $"Avg={allFishFromDb.Average(f => f.TopSpeed):F1} км/год");
            System.Console.WriteLine($"  Довжина: Min={allFishFromDb.Min(f => f.Length):F2}м, " +
                                   $"Max={allFishFromDb.Max(f => f.Length):F2}м, " +
                                   $"Avg={allFishFromDb.Average(f => f.Length):F2}м");
            System.Console.WriteLine($"  Хижаки: {allFishFromDb.Count(f => f.IsPredatory)} " +
                                   $"({allFishFromDb.Count(f => f.IsPredatory) * 100.0 / allFishFromDb.Count():F1}%)\n");

            // Статистика з MongoDB
            System.Console.WriteLine("=== Статистика з MongoDB ===");
            var mongoCount = await mongoRepo.CountAsync();
            System.Console.WriteLine($"Всього риб в MongoDB: {mongoCount}\n");

            // Демонстрація зв'язків
            System.Console.WriteLine("=== Демонстрація зв'язків ===");
            
            // Один-до-багатьох: Aquarium → Fish
            var firstAquarium = aquariums.First();
            var fishInAquarium = allFishFromDb.Where(f => f.AquariumId == firstAquarium.Id).Take(3);
            System.Console.WriteLine($"Акваріум '{firstAquarium.Name}' містить риб:");
            foreach (var f in fishInAquarium)
            {
                System.Console.WriteLine($"  - {f.Variety}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("=== Завершено ===");
            System.Console.WriteLine($"Всього створено риб: {FishBase.TotalFishCount}");
            System.Console.WriteLine($"Збережено в PostgreSQL: {savedToPostgres}");
            System.Console.WriteLine($"Збережено в MongoDB: {savedToMongo}");
        }
    }
}
