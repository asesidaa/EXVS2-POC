using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerVanilla.Migrations
{
    /// <inheritdoc />
    public partial class AddOnlinePair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exvs2_online_pair",
                columns: table => new
                {
                    PairId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_online_pair", x => x.PairId);
                    table.ForeignKey(
                        name: "FK_exvs2_online_pair_exvs2_card_profile_CardProfileId",
                        column: x => x.CardProfileId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_online_pair_CardProfileId",
                table: "exvs2_online_pair",
                column: "CardProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exvs2_online_pair");
        }
    }
}
