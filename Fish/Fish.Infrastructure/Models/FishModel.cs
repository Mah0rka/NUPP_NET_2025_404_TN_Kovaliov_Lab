namespace Fish.Infrastructure.Models
{
    // Базова модель риби для бази даних
    public class FishModel
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } // Для сумісності з Guid з доменної моделі
        
        // Властивості FishType
        public string Variety { get; set; } = string.Empty;
        public string Habitat { get; set; } = string.Empty;
        public int TopSpeed { get; set; }
        public bool IsPredatory { get; set; }
        public double Length { get; set; }
        
        // Зв'язок один-до-багатьох з Aquarium
        public int? AquariumId { get; set; }
        public AquariumModel? Aquarium { get; set; }
        
        // Зв'язок один-до-одного з FishDetails
        public FishDetailsModel? Details { get; set; }
        
        // Зв'язок багато-до-багатьох з Feed
        public ICollection<FeedModel> Feeds { get; set; } = new List<FeedModel>();
    }
}
