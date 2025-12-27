using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fish.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aquariums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Volume = table.Column<double>(type: "double precision", nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aquariums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Variety = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Habitat = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TopSpeed = table.Column<int>(type: "integer", nullable: false),
                    IsPredatory = table.Column<bool>(type: "boolean", nullable: false),
                    Length = table.Column<double>(type: "double precision", nullable: false),
                    AquariumId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fishes_Aquariums_AquariumId",
                        column: x => x.AquariumId,
                        principalTable: "Aquariums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FishDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FishId = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HealthStatus = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FishDetails_Fishes_FishId",
                        column: x => x.FishId,
                        principalTable: "Fishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FishFeeds",
                columns: table => new
                {
                    FeedsId = table.Column<int>(type: "integer", nullable: false),
                    FishesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishFeeds", x => new { x.FeedsId, x.FishesId });
                    table.ForeignKey(
                        name: "FK_FishFeeds_Feeds_FeedsId",
                        column: x => x.FeedsId,
                        principalTable: "Feeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FishFeeds_Fishes_FishesId",
                        column: x => x.FishesId,
                        principalTable: "Fishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FreshwaterFishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    PreferredTemperature = table.Column<double>(type: "double precision", nullable: false),
                    PhLevel = table.Column<double>(type: "double precision", nullable: false),
                    TankSize = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreshwaterFishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreshwaterFishes_Fishes_Id",
                        column: x => x.Id,
                        principalTable: "Fishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MigratoryFishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    MigrationDistance = table.Column<double>(type: "double precision", nullable: false),
                    SpawningGrounds = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MigrationSeason = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MigratoryFishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MigratoryFishes_Fishes_Id",
                        column: x => x.Id,
                        principalTable: "Fishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaltwaterFishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    SaltTolerance = table.Column<double>(type: "double precision", nullable: false),
                    MaxDepth = table.Column<int>(type: "integer", nullable: false),
                    CoralReefCompatible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaltwaterFishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaltwaterFishes_Fishes_Id",
                        column: x => x.Id,
                        principalTable: "Fishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FishDetails_FishId",
                table: "FishDetails",
                column: "FishId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fishes_AquariumId",
                table: "Fishes",
                column: "AquariumId");

            migrationBuilder.CreateIndex(
                name: "IX_FishFeeds_FishesId",
                table: "FishFeeds",
                column: "FishesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FishDetails");

            migrationBuilder.DropTable(
                name: "FishFeeds");

            migrationBuilder.DropTable(
                name: "FreshwaterFishes");

            migrationBuilder.DropTable(
                name: "MigratoryFishes");

            migrationBuilder.DropTable(
                name: "SaltwaterFishes");

            migrationBuilder.DropTable(
                name: "Feeds");

            migrationBuilder.DropTable(
                name: "Fishes");

            migrationBuilder.DropTable(
                name: "Aquariums");
        }
    }
}
