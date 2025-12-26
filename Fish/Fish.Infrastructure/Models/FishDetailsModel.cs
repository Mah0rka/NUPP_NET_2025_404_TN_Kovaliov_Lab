namespace Fish.Infrastructure.Models
{
    // Модель деталей риби (зв'язок один-до-одного з рибою)
    public class FishDetailsModel
    {
        public int Id { get; set; }
        public int FishId { get; set; }
        public FishModel Fish { get; set; } = null!;
        
        // Додаткові деталі
        public DateTime BirthDate { get; set; }
        public string HealthStatus { get; set; } = string.Empty;
        public double Weight { get; set; }
    }
}

