using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerVanilla.Migrations
{
    /// <inheritdoc />
    public partial class AddReplay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exvs2_shared_upload_replay",
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
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_shared_upload_replay", x => x.ReplayId);
                    table.ForeignKey(
                        name: "FK_exvs2_shared_upload_replay_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2_upload_replay",
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
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_upload_replay", x => x.ReplayId);
                    table.ForeignKey(
                        name: "FK_exvs2_upload_replay_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_shared_upload_replay_CardId",
                table: "exvs2_shared_upload_replay",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_shared_upload_replay_ReplayId",
                table: "exvs2_shared_upload_replay",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_upload_replay_CardId",
                table: "exvs2_upload_replay",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_upload_replay_ReplayId",
                table: "exvs2_upload_replay",
                column: "ReplayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exvs2_shared_upload_replay");

            migrationBuilder.DropTable(
                name: "exvs2_upload_replay");
        }
    }
}
