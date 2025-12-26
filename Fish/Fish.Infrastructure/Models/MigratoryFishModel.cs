namespace Fish.Infrastructure.Models
{
    // Модель мігруючої риби (Table-per-Type)
    public class MigratoryFishModel : FishModel
    {
        public double MigrationDistance { get; set; }
        public string SpawningGrounds { get; set; } = string.Empty;
        public string MigrationSeason { get; set; } = string.Empty;
    }
}

