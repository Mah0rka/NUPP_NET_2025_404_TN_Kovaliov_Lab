namespace Fish.Infrastructure.Models
{
    // Модель прісноводної риби (Table-per-Type)
    public class FreshwaterFishModel : FishModel
    {
        public double PreferredTemperature { get; set; }
        public double PhLevel { get; set; }
        public int TankSize { get; set; }
    }
}

