using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class TagTeamData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tag_team_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamName = table.Column<string>(type: "TEXT", nullable: false),
                    TeammateCardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    EffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    EmblemId = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkillPoint = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkillPointBoost = table.Column<uint>(type: "INTEGER", nullable: false),
                    BgmId = table.Column<uint>(type: "INTEGER", nullable: false),
                    NameColorId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_team_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tag_team_data_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tag_team_data_CardId",
                table: "tag_team_data",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tag_team_data");
        }
    }
}
