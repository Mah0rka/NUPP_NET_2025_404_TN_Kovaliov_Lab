using Fish.Common;
using Fish.Common.Services;
using Fish.Common.Extensions;

namespace Fish.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.WriteLine("=== Демонстрація роботи з рибами ===\n");

            // Демонстрація статичного поля
            System.Console.WriteLine($"Початкова кількість риб: {FishBase.TotalFishCount}\n");

            // Створення риб
            System.Console.WriteLine("--- Створення риб ---");

            var shark = new SaltwaterFish(
                new FishType("Акула", "Океан", 50, true, 4.5),
                3.5, 1000, true
            );

            var goldfish = new FreshwaterFish(
                new FishType("Золота рибка", "Прісна вода", 5, false, 0.15),
                22.5, 7.0, 50
            );

            var salmon = new MigratoryFish(
                new FishType("Лосось", "Ріки та океани", 30, false, 0.8),
                3000, "Ріка Фрейзер", "Осінь"
            );

            // Метод розширення
            System.Console.WriteLine(shark.FishType.Variety.ToFishDisplayName());
            System.Console.WriteLine(goldfish.FishType.Variety.ToFishDisplayName());
            System.Console.WriteLine(salmon.FishType.Variety.ToFishDisplayName());
            System.Console.WriteLine();

            // Виклик методів
            System.Console.WriteLine("--- Методи риб ---");
            shark.Swim();
            shark.DiveDeep();
            System.Console.WriteLine();

            goldfish.Swim();
            goldfish.CheckWaterQuality();
            System.Console.WriteLine();

            salmon.Swim();
            salmon.Migrate();
            System.Console.WriteLine();

            // Делегати та події
            System.Console.WriteLine("--- Події ---");
            shark.OnSwim += (fish) => 
            {
                System.Console.WriteLine($"  [Подія] {fish.FishType.Variety} плаває!");
            };
            shark.Swim();
            System.Console.WriteLine();

            // Статичний метод
            FishBase.PrintTotalFish();
            System.Console.WriteLine();

            // Акваріум
            System.Console.WriteLine("--- Акваріум ---");
            var aquarium = new Aquarium("Тропічний рай", 200, "Вітальня");
            System.Console.WriteLine(aquarium.Name.ToAquariumLabel());
            aquarium.DisplayInfo();
            System.Console.WriteLine();

            // CRUD для риб
            System.Console.WriteLine("\n=== CRUD сервіс для риб ===\n");
            
            var fishService = new CrudService<FishBase>(fish => fish.Id);

            // CREATE
            System.Console.WriteLine("--- CREATE ---");
            fishService.Create(shark);
            fishService.Create(goldfish);
            fishService.Create(salmon);
            System.Console.WriteLine();

            // READ ALL
            System.Console.WriteLine("--- READ ALL ---");
            foreach (var fish in fishService.ReadAll())
            {
                System.Console.WriteLine($"  {fish.FishType.Variety}");
            }
            System.Console.WriteLine($"Всього риб: {fishService.Count()}\n");

            // READ
            System.Console.WriteLine("--- READ ---");
            var found = fishService.Read(shark.Id);
            if (found != null)
            {
                System.Console.WriteLine($"Знайдено: {found.FishType.Variety}\n");
            }

            // REMOVE
            System.Console.WriteLine("--- REMOVE ---");
            fishService.Remove(goldfish);
            System.Console.WriteLine($"Залишилось: {fishService.Count()}\n");

            // CRUD для акваріумів
            System.Console.WriteLine("\n=== CRUD сервіс для акваріумів ===\n");
            
            var aquariumService = new CrudService<Aquarium>(aq => aq.Id);

            System.Console.WriteLine("--- CREATE ---");
            aquariumService.Create(aquarium);
            aquariumService.Create(new Aquarium("Океанаріум", 5000, "Холл"));
            aquariumService.Create(new Aquarium("Річковий світ", 300, "Кабінет"));
            System.Console.WriteLine();

            System.Console.WriteLine("--- READ ALL ---");
            foreach (var aq in aquariumService.ReadAll())
            {
                System.Console.WriteLine($"  {aq.Name}: {aq.Volume}л, {aq.Location}");
            }

            System.Console.WriteLine("\n=== Завершено ===");
            FishBase.PrintTotalFish();
        }
    }
}
