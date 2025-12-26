namespace Fish.Common
{
    // Клас для морських риб
    public class SaltwaterFish : FishBase
    {
        public double SaltTolerance { get; set; } // Толерантність до солі (%)
        public int MaxDepth { get; set; } // Максимальна глибина занурення (метри)
        public bool CoralReefCompatible { get; set; } // Сумісність з кораловими рифами

        // Конструктор
        public SaltwaterFish(FishType fishType, double saltTolerance, int maxDepth, bool coralReefCompatible)
            : base(fishType)
        {
            SaltTolerance = saltTolerance;
            MaxDepth = maxDepth;
            CoralReefCompatible = coralReefCompatible;
        }

        // Метод
        public override void Swim()
        {
            Console.WriteLine($"{FishType.Variety} плаває в морській воді на глибині до {MaxDepth}м");
            RaiseOnSwim();
        }

        // Метод
        public void DiveDeep()
        {
            Console.WriteLine($"{FishType.Variety} пірнає на глибину {MaxDepth} метрів");
        }

        // Статичний метод для створення нової риби з випадковими даними
        public static SaltwaterFish CreateNew()
        {
            var random = new Random();
            
            // Випадкові дані для FishType
            string[] varieties = { "Акула", "Тунець", "Риба-клоун", "Мурена", "Скат", "Морський окунь" };
            string variety = varieties[random.Next(varieties.Length)];
            uint topSpeed = (uint)random.Next(20, 80);
            bool isPredatory = random.Next(2) == 0;
            double length = Math.Round(random.NextDouble() * 5 + 0.5, 2);
            
            var fishType = new FishType(variety, "Морська вода", topSpeed, isPredatory, length);
            
            // Випадкові параметри морської риби
            double saltTolerance = Math.Round(random.NextDouble() * 1.5 + 2.5, 1); // 2.5-4.0%
            int maxDepth = random.Next(50, 1500); // 50-1500 метрів
            bool coralReefCompatible = random.Next(2) == 0;
            
            return new SaltwaterFish(fishType, saltTolerance, maxDepth, coralReefCompatible);
        }
    }
}

