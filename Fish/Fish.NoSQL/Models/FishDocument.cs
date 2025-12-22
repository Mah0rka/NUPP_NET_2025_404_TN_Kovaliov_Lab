using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fish.NoSQL.Models
{
    // Документ для зберігання риби в MongoDB
    [BsonIgnoreExtraElements]
    public class FishDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("externalId")]
        public Guid ExternalId { get; set; }

        [BsonElement("variety")]
        public string Variety { get; set; } = string.Empty;

        [BsonElement("habitat")]
        public string Habitat { get; set; } = string.Empty;

        [BsonElement("topSpeed")]
        public int TopSpeed { get; set; }

        [BsonElement("isPredatory")]
        public bool IsPredatory { get; set; }

        [BsonElement("length")]
        public double Length { get; set; }

        [BsonElement("fishType")]
        public string FishType { get; set; } = string.Empty; // "Freshwater", "Saltwater", "Migratory"

        // Специфічні властивості (зберігаються як додаткові поля)
        [BsonElement("preferredTemperature")]
        [BsonIgnoreIfDefault]
        public double? PreferredTemperature { get; set; }

        [BsonElement("phLevel")]
        [BsonIgnoreIfDefault]
        public double? PhLevel { get; set; }

        [BsonElement("tankSize")]
        [BsonIgnoreIfDefault]
        public int? TankSize { get; set; }

        [BsonElement("saltTolerance")]
        [BsonIgnoreIfDefault]
        public double? SaltTolerance { get; set; }

        [BsonElement("maxDepth")]
        [BsonIgnoreIfDefault]
        public int? MaxDepth { get; set; }

        [BsonElement("coralReefCompatible")]
        [BsonIgnoreIfDefault]
        public bool? CoralReefCompatible { get; set; }

        [BsonElement("migrationDistance")]
        [BsonIgnoreIfDefault]
        public double? MigrationDistance { get; set; }

        [BsonElement("spawningGrounds")]
        [BsonIgnoreIfDefault]
        public string? SpawningGrounds { get; set; }

        [BsonElement("migrationSeason")]
        [BsonIgnoreIfDefault]
        public string? MigrationSeason { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

