namespace Fish.Infrastructure.Models
{
    // Модель корму (зв'язок багато-до-багатьох з рибами)
    public class FeedModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Price { get; set; }
        
        // Навігаційна властивість для риб
        public ICollection<FishModel> Fishes { get; set; } = new List<FishModel>();
    }
}

