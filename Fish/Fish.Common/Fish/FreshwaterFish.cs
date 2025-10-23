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
    }
}

