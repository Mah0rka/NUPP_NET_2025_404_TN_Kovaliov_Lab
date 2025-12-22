namespace Fish.Infrastructure.Models
{
    // Модель морської риби (Table-per-Type)
    public class SaltwaterFishModel : FishModel
    {
        public double SaltTolerance { get; set; }
        public int MaxDepth { get; set; }
        public bool CoralReefCompatible { get; set; }
    }
}

