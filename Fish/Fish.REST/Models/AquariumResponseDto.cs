namespace Fish.REST.Models
{
    // DTO для відповіді з даними акваріума
    public class AquariumResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Volume { get; set; }
        public string Location { get; set; } = string.Empty;
        public int FishCount { get; set; }
    }
}

