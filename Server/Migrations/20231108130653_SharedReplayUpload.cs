using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class SharedReplayUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shared_upload_replay",
                columns: table => new
                {
                    ReplayId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    ReplaySize = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayedAt = table.Column<ulong>(type: "INTEGER", nullable: false),
                    StageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PilotsJson = table.Column<string>(type: "TEXT", nullable: false),
                    SpecialFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReplayServiceFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    MobileUserId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MatchingMode = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamType = table.Column<uint>(type: "INTEGER", nullable: false),
                    ReturnMatchFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    TournamentId = table.Column<uint>(type: "INTEGER", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shared_upload_replay", x => x.ReplayId);
                    table.ForeignKey(
                        name: "FK_shared_upload_replay_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_shared_upload_replay_CardId",
                table: "shared_upload_replay",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shared_upload_replay");
        }
    }
}
