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
    }
}

