using System;
using Microsoft.EntityFrameworkCore.Migrations;
using ServerVanilla.Models.Cards.Common;

#nullable disable

namespace ServerVanilla.Migrations
{
    /// <inheritdoc />
    public partial class InitEchelonTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exvs2_common_echelon_setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EchelonId = table.Column<uint>(type: "INTEGER", nullable: false),
                    OnlineMatchRankS = table.Column<uint>(type: "INTEGER", nullable: false),
                    OnlineMatchRankT = table.Column<uint>(type: "INTEGER", nullable: false),
                    OnlineMatchRankE = table.Column<uint>(type: "INTEGER", nullable: false),
                    MsAdjustFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    DownDefaultExp = table.Column<int>(type: "INTEGER", nullable: false),
                    UpDefaultExp = table.Column<int>(type: "INTEGER", nullable: false),
                    WinCorrectionRate = table.Column<uint>(type: "INTEGER", nullable: false),
                    LoseCorrectionRate = table.Column<uint>(type: "INTEGER", nullable: false),
                    ExpWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    DowngradeThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_common_echelon_setting", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_common_echelon_setting_Id",
                table: "exvs2_common_echelon_setting",
                column: "Id");

            migrationBuilder.InsertData(
                table: "exvs2_common_echelon_setting",
                columns: new[]
                {
                    nameof(EchelonSetting.EchelonId), 
                    nameof(EchelonSetting.OnlineMatchRankS), nameof(EchelonSetting.OnlineMatchRankT), nameof(EchelonSetting.OnlineMatchRankE),
                    nameof(EchelonSetting.MsAdjustFlag), 
                    nameof(EchelonSetting.DownDefaultExp), nameof(EchelonSetting.UpDefaultExp),
                    nameof(EchelonSetting.WinCorrectionRate), nameof(EchelonSetting.LoseCorrectionRate),
                    nameof(EchelonSetting.ExpWidth), nameof(EchelonSetting.DowngradeThreshold),
                    nameof(EchelonSetting.CreateTime), nameof(EchelonSetting.UpdateTime),
                },
                values: new object[,] {
					{
						1, 
						0, 0, 0, 
						true, 
						0, 0, 
						0, 0, 
						99, 0, 
						DateTime.Now, DateTime.Now
					},
					{
						2, 
						0, 0, 0, 
						true, 
						0, 0, 
						0, 0, 
						100, 0,
						DateTime.Now, DateTime.Now
					},
					{
						3, 
						1, 1, 1, 
						true, 
						0, 0, 
						0, 0, 
						150, 0, 
						DateTime.Now, DateTime.Now
					},
					{
						4, 
						1, 1, 1, 
						true,
						0, 0, 
						0, 0, 
						300, 0, 
						DateTime.Now, DateTime.Now
					},
					{
						5, 
						2, 1, 2, 
						true, 
						0, 0, 
						0, 0, 
						300, 0, 
						DateTime.Now, DateTime.Now
					},
					{
						6, 
						2, 2, 2, 
						true, 
						0, 0, 
						0, 0, 
						500, 0, 
						DateTime.Now, DateTime.Now
					},
					{
						7, 
						2, 2, 2, 
						true, 
						0, 0, 
						0, 0, 
						750, 0, 
						DateTime.Now, DateTime.Now
					},
					{
						8, 
						3, 2, 2, 
						true, 
						250, 0, 
						80, 90, 
						500, 0, 
						DateTime.Now,DateTime.Now
					},
					{
						9, 
						3, 3, 3, 
						true, 
						250, 0, 
						80, 90, 
						500, -600, 
						DateTime.Now, DateTime.Now
					},
					{
						10, 
						4, 3, 4, 
						true, 
						250, 0, 
						80, 90, 
						500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						11, 
						4, 3, 4, 
						true, 
						250, 0, 
						80, 90, 
						500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						12, 
						4, 3, 5, 
						true, 
						250, 0, 
						80, 90, 
						500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						13, 
						5, 3, 5, 
						true, 
						500, 0, 
						80, 90, 
						800, -550, 
						DateTime.Now, DateTime.Now
					},
					{
						14, 
						6, 4, 6, 
						true, 
						700, 0, 
						80, 90, 
						1000, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						15, 
						7, 4, 6, 
						true, 
						700, 0, 
						80, 90, 
						1000, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						16, 
						7, 4, 6, 
						true, 
						700, 0, 
						80, 90, 
						1000, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						17,
						7, 4, 6, 
						true, 
						700, 0, 
						80, 90, 
						1000, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						18, 
						7, 4, 6, 
						true, 
						1200, 0, 
						80, 90, 
						1500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						19, 
						8, 5, 7, 
						false, 
						1200, 0, 
						80, 90, 
						1500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						20, 
						9, 5, 7, 
						false, 
						1200, 0, 
						80, 90, 
						1500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						21, 
						9, 5, 7, 
						false, 
						1200, 0, 
						80, 90, 
						1500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						22, 
						9, 5, 7, 
						false, 
						1200, 0, 
						80, 90, 
						1500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						23, 
						10, 5, 7, 
						false, 
						1500, 0, 
						80, 90, 
						2000, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						24, 
						11, 6, 7, 
						false, 
						1000, 0, 
						90, 95, 
						2000, -600, 
						DateTime.Now, DateTime.Now
					},
					{
						25, 
						12, 6, 7, 
						false, 
						1000, 0, 
						90, 95, 
						2500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						26, 
						12, 6, 7, 
						false, 
						1000, 0, 
						90, 95, 
						2500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						27, 
						12, 6, 7, 
						false, 
						1000, 0, 
						90, 95, 
						2500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						28, 
						12, 6, 7, 
						false, 
						4000, 0, 
						90, 95, 
						5000, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						29, 
						13, 7, 7, 
						false, 
						4000, 0, 
						90, 95, 
						5500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						30, 
						13, 7, 7, 
						false, 
						4000, 0, 
						90, 95, 
						5500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						31, 
						13, 7, 7, 
						false, 
						4000, 0, 
						90, 95, 
						5500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						32, 
						13, 7, 7, 
						false, 
						4000, 0, 
						90, 95, 
						5500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						33, 
						13, 7, 7, 
						false, 
						6000, 0,
						90, 95, 
						7500, -550, 
						DateTime.Now, DateTime.Now
					},
					{
						34, 
						13, 7, 7, 
						false,
						7000, 0, 
						90, 95, 
						8500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						35, 
						13, 7, 7, 
						false, 
						7000, 0, 
						90, 95, 
						8500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						36, 
						13, 7, 7, 
						false, 
						7000, 0, 
						90, 95, 
						8500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						37, 
						13, 7, 7, 
						false, 
						7000, 0, 
						90, 95, 
						8500, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						38, 
						13, 7, 7, 
						false, 
						10000, 0, 
						90, 95, 
						11500, -550, 
						DateTime.Now, DateTime.Now
					},
					{
						39, 
						13, 7, 7, 
						false, 
						8000, 0, 
						100, 100, 
						11000, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						40, 
						13, 7, 7, 
						false, 
						8000, 0, 
						100, 100, 
						11000, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						41, 
						13, 7, 7, 
						false, 
						8000, 0, 
						100, 100, 
						11000, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						42, 
						13, 7, 7, 
						false, 
						8000, 0, 
						100, 100, 
						11000, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						43, 
						13, 7, 7, 
						false, 
						16000, 0, 
						100, 100, 
						19000, -550, 
						DateTime.Now, DateTime.Now
					},
					{
						44, 
						13, 7, 7, 
						false, 
						13000, 0,
						100, 100, 
						16000, -450, 
						DateTime.Now, DateTime.Now
					},
					{
						45, 
						13, 7, 7, 
						false, 
						13000, 0, 
						100, 100, 
						16000, -450, 
						DateTime.Now, DateTime.Now
					},
					{
						46, 
						13, 7, 7, 
						false, 
						13000, 0, 
						100, 100, 
						16000, -450, 
						DateTime.Now, DateTime.Now
					},
					{
						47, 
						13, 7, 7, 
						false, 
						13000, 0, 
						100, 100, 
						16000, -450, 
						DateTime.Now, DateTime.Now
					},
					{
						48, 
						13, 7, 7, 
						false, 
						18000, 0, 
						100, 100, 
						21000, -550, 
						DateTime.Now, DateTime.Now
					},
					{
						49, 
						13, 7, 7, 
						false, 
						18000, 0, 
						100, 100, 
						21000, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						50, 
						13, 7, 7, 
						false, 
						18000, 0, 
						100, 100, 
						21000, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						51, 
						13, 7, 7, 
						false,
						18000, 0,
						100, 100, 
						21000, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						52, 
						13, 7, 7, 
						false, 
						18000, 0, 
						100, 100, 
						21000, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						53, 
						13, 7, 7, 
						false, 
						26000, 0, 
						100, 100, 
						29000, -500, 
						DateTime.Now, DateTime.Now
					},
					{
						54, 
						13, 7, 7, 
						false, 
						0, 0, 
						100, 100, 
						500, -400, 
						DateTime.Now, DateTime.Now
					},
					{
						55, 
						13, 7, 7, 
						false, 
						0, 0, 
						100, 100, 
						0, 0, 
						DateTime.Now, DateTime.Now
					}
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exvs2_common_echelon_setting");
        }
    }
}
