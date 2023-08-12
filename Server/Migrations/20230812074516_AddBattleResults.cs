using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddBattleResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "offline_pvp_battle_result",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OfflineBattleMode = table.Column<string>(type: "TEXT", nullable: false),
                    SEchelonFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    SEchelonProgress = table.Column<uint>(type: "INTEGER", nullable: false),
                    PartnerPilotId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PartnerMsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PartnerEchelonId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PartnerBurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    Foe1PilotId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Foe1MsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Foe1EchelonId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Foe1BurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    Foe2PilotId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Foe2MsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Foe2EchelonId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Foe2BurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Mode = table.Column<string>(type: "TEXT", nullable: false),
                    WinFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    Score = table.Column<uint>(type: "INTEGER", nullable: false),
                    UsedMsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    UsedBurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    ElapsedSecond = table.Column<uint>(type: "INTEGER", nullable: false),
                    PastEchelonId = table.Column<uint>(type: "INTEGER", nullable: false),
                    EchelonExpChange = table.Column<int>(type: "INTEGER", nullable: false),
                    EchelonIdAfterBattle = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalEchelonExp = table.Column<int>(type: "INTEGER", nullable: false),
                    FullBattleResultJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offline_pvp_battle_result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_offline_pvp_battle_result_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "triad_battle_result",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<uint>(type: "INTEGER", nullable: false),
                    SceneId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Mode = table.Column<string>(type: "TEXT", nullable: false),
                    WinFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    Score = table.Column<uint>(type: "INTEGER", nullable: false),
                    UsedMsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    UsedBurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    ElapsedSecond = table.Column<uint>(type: "INTEGER", nullable: false),
                    PastEchelonId = table.Column<uint>(type: "INTEGER", nullable: false),
                    EchelonExpChange = table.Column<int>(type: "INTEGER", nullable: false),
                    EchelonIdAfterBattle = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalEchelonExp = table.Column<int>(type: "INTEGER", nullable: false),
                    FullBattleResultJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_triad_battle_result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_triad_battle_result_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_offline_pvp_battle_result_CardId",
                table: "offline_pvp_battle_result",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_triad_battle_result_CardId",
                table: "triad_battle_result",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "offline_pvp_battle_result");

            migrationBuilder.DropTable(
                name: "triad_battle_result");
        }
    }
}
