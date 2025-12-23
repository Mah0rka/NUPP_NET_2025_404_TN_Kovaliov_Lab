using Fish.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fish.Infrastructure
{
    // DbContext для роботи з базою даних риб та Identity
    public class FishContext : IdentityDbContext<ApplicationUser>
    {
        public FishContext(DbContextOptions<FishContext> options) : base(options)
        {
        }

        // DbSet для всіх сутностей
        public DbSet<FishModel> Fishes { get; set; }
        public DbSet<FreshwaterFishModel> FreshwaterFishes { get; set; }
        public DbSet<SaltwaterFishModel> SaltwaterFishes { get; set; }
        public DbSet<MigratoryFishModel> MigratoryFishes { get; set; }
        public DbSet<AquariumModel> Aquariums { get; set; }
        public DbSet<FishDetailsModel> FishDetails { get; set; }
        public DbSet<FeedModel> Feeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфігурація базової моделі FishModel
            modelBuilder.Entity<FishModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.Property(e => e.Variety).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Habitat).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TopSpeed).IsRequired();
                entity.Property(e => e.IsPredatory).IsRequired();
                entity.Property(e => e.Length).IsRequired();

                // Зв'язок один-до-багатьох з Aquarium
                entity.HasOne(e => e.Aquarium)
                    .WithMany(a => a.Fishes)
                    .HasForeignKey(e => e.AquariumId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Зв'язок один-до-одного з FishDetails
                entity.HasOne(e => e.Details)
                    .WithOne(d => d.Fish)
                    .HasForeignKey<FishDetailsModel>(d => d.FishId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Зв'язок багато-до-багатьох з Feed
                entity.HasMany(e => e.Feeds)
                    .WithMany(f => f.Fishes)
                    .UsingEntity(j => j.ToTable("FishFeeds"));
            });

            // Table-per-Type (TPT) для FreshwaterFishModel
            modelBuilder.Entity<FreshwaterFishModel>(entity =>
            {
                entity.ToTable("FreshwaterFishes");
                entity.Property(e => e.PreferredTemperature).IsRequired();
                entity.Property(e => e.PhLevel).IsRequired();
                entity.Property(e => e.TankSize).IsRequired();
            });

            // Table-per-Type (TPT) для SaltwaterFishModel
            modelBuilder.Entity<SaltwaterFishModel>(entity =>
            {
                entity.ToTable("SaltwaterFishes");
                entity.Property(e => e.SaltTolerance).IsRequired();
                entity.Property(e => e.MaxDepth).IsRequired();
                entity.Property(e => e.CoralReefCompatible).IsRequired();
            });

            // Table-per-Type (TPT) для MigratoryFishModel
            modelBuilder.Entity<MigratoryFishModel>(entity =>
            {
                entity.ToTable("MigratoryFishes");
                entity.Property(e => e.MigrationDistance).IsRequired();
                entity.Property(e => e.SpawningGrounds).IsRequired().HasMaxLength(200);
                entity.Property(e => e.MigrationSeason).IsRequired().HasMaxLength(50);
            });

            // Конфігурація AquariumModel
            modelBuilder.Entity<AquariumModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Volume).IsRequired();
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
            });

            // Конфігурація FishDetailsModel
            modelBuilder.Entity<FishDetailsModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BirthDate).IsRequired();
                entity.Property(e => e.HealthStatus).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Weight).IsRequired();
            });

            // Конфігурація FeedModel
            modelBuilder.Entity<FeedModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).IsRequired();
            });
        }
    }
}

