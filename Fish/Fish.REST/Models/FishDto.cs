namespace Fish.REST.Models
{
    // DTO для створення/оновлення риби
    public class FishDto
    {
        public string Variety { get; set; } = string.Empty;
        public string Habitat { get; set; } = string.Empty;
        public int TopSpeed { get; set; }
        public bool IsPredatory { get; set; }
        public double Length { get; set; }
        public int? AquariumId { get; set; }
    }
}


