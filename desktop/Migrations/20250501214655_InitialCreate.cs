using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desktop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adventures",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AdventureType = table.Column<int>(type: "INTEGER", nullable: false),
                    CompletionTime = table.Column<string>(type: "TEXT", nullable: true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: true),
                    Grade = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adventures", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<string>(type: "TEXT", nullable: true),
                    RarityRef = table.Column<string>(type: "TEXT", nullable: true),
                    ItemCode = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DescriptionRaw = table.Column<string>(type: "TEXT", nullable: true),
                    AssetAbsolutePath = table.Column<string>(type: "TEXT", nullable: true),
                    AssetFullAbsolutePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsWin = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeckCode = table.Column<string>(type: "TEXT", nullable: false),
                    Opponent = table.Column<string>(type: "TEXT", nullable: false),
                    GameType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Relics",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<string>(type: "TEXT", nullable: true),
                    RarityRef = table.Column<string>(type: "TEXT", nullable: true),
                    RelicCode = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DescriptionRaw = table.Column<string>(type: "TEXT", nullable: true),
                    AssetAbsolutePath = table.Column<string>(type: "TEXT", nullable: true),
                    AssetFullAbsolutePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relics", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "AdventureNodes",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CompletionTime = table.Column<string>(type: "TEXT", nullable: true),
                    AdventureDTOId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdventureNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdventureNodes_Adventures_AdventureDTOId",
                        column: x => x.AdventureDTOId,
                        principalTable: "Adventures",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "RegionDTO",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Region = table.Column<string>(type: "TEXT", nullable: true),
                    MatchDTOId = table.Column<int>(type: "INTEGER", nullable: true),
                    MatchDTOId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegionDTO_Matches_MatchDTOId",
                        column: x => x.MatchDTOId,
                        principalTable: "Matches",
                        principalColumn: "Id"
                    );
                    table.ForeignKey(
                        name: "FK_RegionDTO_Matches_MatchDTOId1",
                        column: x => x.MatchDTOId1,
                        principalTable: "Matches",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AdventureDeck",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardImage = table.Column<string>(type: "TEXT", nullable: true),
                    ManaCost = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DrawProbability = table.Column<string>(type: "TEXT", nullable: true),
                    Region = table.Column<string>(type: "TEXT", nullable: true),
                    CardType = table.Column<string>(type: "TEXT", nullable: true),
                    CardViewRect = table.Column<string>(type: "TEXT", nullable: true),
                    CardStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    CardCode = table.Column<string>(type: "TEXT", nullable: true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: true),
                    CopiesInDeck = table.Column<int>(type: "INTEGER", nullable: false),
                    CopiesRemaining = table.Column<int>(type: "INTEGER", nullable: false),
                    Attack = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    AdventureNodeDTOId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdventureDeck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdventureDeck_AdventureNodes_AdventureNodeDTOId",
                        column: x => x.AdventureNodeDTOId,
                        principalTable: "AdventureNodes",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AdventurePowers",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PowerCode = table.Column<string>(type: "TEXT", nullable: true),
                    Rarity = table.Column<string>(type: "TEXT", nullable: true),
                    RarityRef = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DescriptionRaw = table.Column<string>(type: "TEXT", nullable: true),
                    AssetAbsolutePath = table.Column<string>(type: "TEXT", nullable: true),
                    AssetFullAbsolutePath = table.Column<string>(type: "TEXT", nullable: true),
                    PowerState = table.Column<int>(type: "INTEGER", nullable: false),
                    AdventureDTOId = table.Column<int>(type: "INTEGER", nullable: true),
                    AdventureNodeDTOId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdventurePowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdventurePowers_AdventureNodes_AdventureNodeDTOId",
                        column: x => x.AdventureNodeDTOId,
                        principalTable: "AdventureNodes",
                        principalColumn: "Id"
                    );
                    table.ForeignKey(
                        name: "FK_AdventurePowers_Adventures_AdventureDTOId",
                        column: x => x.AdventureDTOId,
                        principalTable: "Adventures",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AttachmentDTO",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AttachmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    POCCardDTOId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentDTO_AdventureDeck_POCCardDTOId",
                        column: x => x.POCCardDTOId,
                        principalTable: "AdventureDeck",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AttachmentInfoDTO",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    AttachmentDTOId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentInfoDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentInfoDTO_AttachmentDTO_AttachmentDTOId",
                        column: x => x.AttachmentDTOId,
                        principalTable: "AttachmentDTO",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_AdventureDeck_AdventureNodeDTOId",
                table: "AdventureDeck",
                column: "AdventureNodeDTOId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_AdventureNodes_AdventureDTOId",
                table: "AdventureNodes",
                column: "AdventureDTOId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_AdventurePowers_AdventureDTOId",
                table: "AdventurePowers",
                column: "AdventureDTOId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_AdventurePowers_AdventureNodeDTOId",
                table: "AdventurePowers",
                column: "AdventureNodeDTOId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentDTO_POCCardDTOId",
                table: "AttachmentDTO",
                column: "POCCardDTOId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentInfoDTO_AttachmentDTOId",
                table: "AttachmentInfoDTO",
                column: "AttachmentDTOId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_RegionDTO_MatchDTOId",
                table: "RegionDTO",
                column: "MatchDTOId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_RegionDTO_MatchDTOId1",
                table: "RegionDTO",
                column: "MatchDTOId1"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AdventurePowers");

            migrationBuilder.DropTable(name: "AttachmentInfoDTO");

            migrationBuilder.DropTable(name: "Items");

            migrationBuilder.DropTable(name: "RegionDTO");

            migrationBuilder.DropTable(name: "Relics");

            migrationBuilder.DropTable(name: "AttachmentDTO");

            migrationBuilder.DropTable(name: "Matches");

            migrationBuilder.DropTable(name: "AdventureDeck");

            migrationBuilder.DropTable(name: "AdventureNodes");

            migrationBuilder.DropTable(name: "Adventures");
        }
    }
}
