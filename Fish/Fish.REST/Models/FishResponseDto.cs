namespace Fish.REST.Models
{
    // DTO для відповіді з даними риби
    public class FishResponseDto
    {
        public Guid Id { get; set; }
        public string Variety { get; set; } = string.Empty;
        public string Habitat { get; set; } = string.Empty;
        public int TopSpeed { get; set; }
        public bool IsPredatory { get; set; }
        public double Length { get; set; }
        public int? AquariumId { get; set; }
        public string? AquariumName { get; set; }
    }
}


