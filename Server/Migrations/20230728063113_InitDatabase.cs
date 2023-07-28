using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "card_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccessCode = table.Column<string>(type: "TEXT", nullable: false),
                    ChipId = table.Column<string>(type: "TEXT", nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_profile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pilot_domain",
                columns: table => new
                {
                    PilotId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoadPlayerJson = table.Column<string>(type: "TEXT", nullable: false),
                    PilotDataGroupJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pilot_domain", x => x.PilotId);
                    table.ForeignKey(
                        name: "FK_pilot_domain_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_domain",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserJson = table.Column<string>(type: "TEXT", nullable: false),
                    MobileUserGroupJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_domain", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_user_domain_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pilot_domain_CardId",
                table: "pilot_domain",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_domain_CardId",
                table: "user_domain",
                column: "CardId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pilot_domain");

            migrationBuilder.DropTable(
                name: "user_domain");

            migrationBuilder.DropTable(
                name: "card_profile");
        }
    }
}
