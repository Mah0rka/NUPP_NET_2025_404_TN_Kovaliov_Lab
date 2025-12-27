namespace Fish.Infrastructure.Models
{
    // Модель акваріума (зв'язок один-до-багатьох з рибами)
    public class AquariumModel
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Volume { get; set; }
        public string Location { get; set; } = string.Empty;
        
        // Навігаційна властивість для риб
        public ICollection<FishModel> Fishes { get; set; } = new List<FishModel>();
    }
}

