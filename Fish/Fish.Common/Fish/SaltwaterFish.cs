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
    }
}

