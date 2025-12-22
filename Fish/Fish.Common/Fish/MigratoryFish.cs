namespace Fish.Common
{
    // Клас для мігруючих риб
    public class MigratoryFish : FishBase
    {
        public double MigrationDistance { get; set; } // Відстань міграції (км)
        public string SpawningGrounds { get; set; } // Місце нересту
        public string MigrationSeason { get; set; } // Сезон міграції

        // Конструктор
        public MigratoryFish(FishType fishType, double migrationDistance, string spawningGrounds, string migrationSeason)
            : base(fishType)
        {
            MigrationDistance = migrationDistance;
            SpawningGrounds = spawningGrounds;
            MigrationSeason = migrationSeason;
        }

        // Метод
        public override void Swim()
        {
            Console.WriteLine($"{FishType.Variety} мігрує на відстань {MigrationDistance} км");
            RaiseOnSwim();
        }

        // Метод
        public void Migrate()
        {
            Console.WriteLine($"{FishType.Variety} починає міграцію до {SpawningGrounds} у сезон '{MigrationSeason}'");
        }

        // Статичний метод для створення нової риби з випадковими даними
        public static MigratoryFish CreateNew()
        {
            var random = new Random();
            
            // Випадкові дані для FishType
            string[] varieties = { "Лосось", "Оселедець", "Вугор", "Осетер", "Форель", "Сьомга" };
            string variety = varieties[random.Next(varieties.Length)];
            uint topSpeed = (uint)random.Next(15, 50);
            bool isPredatory = random.Next(2) == 0;
            double length = Math.Round(random.NextDouble() * 1.5 + 0.3, 2);
            
            var fishType = new FishType(variety, "Ріки та океани", topSpeed, isPredatory, length);
            
            // Випадкові параметри мігруючої риби
            double migrationDistance = Math.Round(random.NextDouble() * 4500 + 500, 0); // 500-5000 км
            string[] spawningGrounds = { "Ріка Фрейзер", "Балтійське море", "Саргасове море", "Ріка Дунай", "Тихий океан" };
            string spawningGround = spawningGrounds[random.Next(spawningGrounds.Length)];
            string[] seasons = { "Весна", "Літо", "Осінь", "Зима" };
            string season = seasons[random.Next(seasons.Length)];
            
            return new MigratoryFish(fishType, migrationDistance, spawningGround, season);
        }
    }
}

