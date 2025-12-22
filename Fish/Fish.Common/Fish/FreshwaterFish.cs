namespace Fish.Common
{
    // Клас для прісноводних риб
    public class FreshwaterFish : FishBase
    {
        public double PreferredTemperature { get; set; } // Бажана температура (°C)
        public double PhLevel { get; set; } // Рівень pH води
        public int TankSize { get; set; } // Мінімальний розмір акваріума (літри)

        // Конструктор
        public FreshwaterFish(FishType fishType, double preferredTemperature, double phLevel, int tankSize)
            : base(fishType)
        {
            PreferredTemperature = preferredTemperature;
            PhLevel = phLevel;
            TankSize = tankSize;
        }

        // Метод
        public override void Swim()
        {
            Console.WriteLine($"{FishType.Variety} плаває у прісній воді при температурі {PreferredTemperature}°C");
            RaiseOnSwim();
        }

        // Метод
        public void CheckWaterQuality()
        {
            Console.WriteLine($"Перевірка якості води: pH={PhLevel}, Температура={PreferredTemperature}°C");
        }

        // Статичний метод для створення нової риби з випадковими даними
        public static FreshwaterFish CreateNew()
        {
            var random = new Random();
            
            // Випадкові дані для FishType
            string[] varieties = { "Золота рибка", "Гуппі", "Неон", "Скалярія", "Барбус", "Данио" };
            string variety = varieties[random.Next(varieties.Length)];
            uint topSpeed = (uint)random.Next(3, 15);
            bool isPredatory = random.Next(2) == 0;
            double length = Math.Round(random.NextDouble() * 0.3 + 0.05, 2);
            
            var fishType = new FishType(variety, "Прісна вода", topSpeed, isPredatory, length);
            
            // Випадкові параметри прісноводної риби
            double temperature = Math.Round(random.NextDouble() * 13 + 15, 1); // 15-28°C
            double phLevel = Math.Round(random.NextDouble() * 2.5 + 6.0, 1); // 6.0-8.5
            int tankSize = random.Next(20, 200); // 20-200 літрів
            
            return new FreshwaterFish(fishType, temperature, phLevel, tankSize);
        }
    }
}

