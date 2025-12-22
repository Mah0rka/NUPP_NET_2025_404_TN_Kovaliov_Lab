namespace Fish.REST.Models
{
    // DTO для створення/оновлення акваріума
    public class AquariumDto
    {
        public string Name { get; set; } = string.Empty;
        public double Volume { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}

