using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Server.Models.Common;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InitEchelonSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "common_echelon_setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EchelonId = table.Column<uint>(type: "INTEGER", nullable: false),
                    EchelonName = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ExpWidth = table.Column<uint>(type: "INTEGER", nullable: false),
                    UpDefaultExp = table.Column<int>(type: "INTEGER", nullable: false),
                    DownDefaultExp = table.Column<int>(type: "INTEGER", nullable: false),
                    DowngradeThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_common_echelon_setting", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_common_echelon_setting_Id_EchelonId",
                table: "common_echelon_setting",
                columns: new[] { "Id", "EchelonId" });
            
            migrationBuilder.InsertData(
                table: "common_echelon_setting",
                columns: new[]
                {
                    nameof(EchelonSetting.EchelonId), 
                    nameof(EchelonSetting.EchelonName), 
                    nameof(EchelonSetting.ExpWidth), 
                    nameof(EchelonSetting.UpDefaultExp),
                    nameof(EchelonSetting.DownDefaultExp),
                    nameof(EchelonSetting.DowngradeThreshold),
                    nameof(EchelonSetting.CreateTime), 
                    nameof(EchelonSetting.UpdateTime)
                },
                values: new object[,] {
                    { 0, "民間人", 1, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 1, "新兵", 100, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 2, "二等兵", 100, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 3, "一等兵", 150, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 4, "上等兵", 300, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 5, "伍長", 300, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 6, "軍曹", 500, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 7, "曹長", 750, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 8, "准尉", 500, 0, 250, 0, DateTime.Now, DateTime.Now },
                    { 9, "少尉1", 500, 0, 250, 0, DateTime.Now, DateTime.Now },
                    { 10, "少尉2", 500, 0, 250, -300, DateTime.Now, DateTime.Now },
                    { 11, "少尉3", 500, 0, 250, -300, DateTime.Now, DateTime.Now },
                    { 12, "少尉4", 500, 0, 250, -300, DateTime.Now, DateTime.Now },
                    { 13, "少尉5", 1000, 0, 0, -450, DateTime.Now, DateTime.Now },
                    { 14, "中尉1", 1000, 0, 700, 0, DateTime.Now, DateTime.Now },
                    { 15, "中尉2", 1000, 0, 700, -300, DateTime.Now, DateTime.Now },
                    { 16, "中尉3", 1000, 0, 700, -300, DateTime.Now, DateTime.Now },
                    { 17, "中尉4", 1000, 0, 700, -300, DateTime.Now, DateTime.Now },
                    { 18, "中尉5", 3000, 0, 0, -400, DateTime.Now, DateTime.Now },
                    { 19, "大尉1", 1500, 0, 1200, 0, DateTime.Now, DateTime.Now },
                    { 20, "大尉2", 1500, 0, 1200, -300, DateTime.Now, DateTime.Now },
                    { 21, "大尉3", 1500, 0, 1200, -300, DateTime.Now, DateTime.Now },
                    { 22, "大尉4", 1500, 0, 1200, -300, DateTime.Now, DateTime.Now },
                    { 23, "大尉5", 4000, 0, 0, -400, DateTime.Now, DateTime.Now },
                    { 24, "少佐1", 2000, 0, 1000, 0, DateTime.Now, DateTime.Now },
                    { 25, "少佐2", 2500, 0, 1000, -300, DateTime.Now, DateTime.Now },
                    { 26, "少佐3", 2500, 0, 1000, -300, DateTime.Now, DateTime.Now },
                    { 27, "少佐4", 2500, 0, 1000, -300, DateTime.Now, DateTime.Now },
                    { 28, "少佐5", 6000, 0, 0, -400, DateTime.Now, DateTime.Now },
                    { 29, "中佐1", 10000, 0, 4000, 0, DateTime.Now, DateTime.Now },
                    { 30, "中佐2", 10000, 0, 4000, -300, DateTime.Now, DateTime.Now },
                    { 31, "中佐3", 10000, 0, 4000, -300, DateTime.Now, DateTime.Now },
                    { 32, "中佐4", 10000, 0, 4000, -300, DateTime.Now, DateTime.Now },
                    { 33, "中佐5", 20000, 0, 0, -350, DateTime.Now, DateTime.Now },
                    { 34, "大佐1", 15000, 0, 7000, 0, DateTime.Now, DateTime.Now },
                    { 35, "大佐2", 15000, 0, 7000, -300, DateTime.Now, DateTime.Now },
                    { 36, "大佐3", 15000, 0, 7000, -300, DateTime.Now, DateTime.Now },
                    { 37, "大佐4", 15000, 0, 7000, -300, DateTime.Now, DateTime.Now },
                    { 38, "大佐5", 30000, 0, 0, -350, DateTime.Now, DateTime.Now },
                    { 39, "少将1", 18000, 0, 8000, 0, DateTime.Now, DateTime.Now },
                    { 40, "少将2", 18000, 0, 8000, -300, DateTime.Now, DateTime.Now },
                    { 41, "少将3", 18000, 0, 8000, -300, DateTime.Now, DateTime.Now },
                    { 42, "少将4", 18000, 0, 8000, -300, DateTime.Now, DateTime.Now },
                    { 43, "少将5", 30000, 0, 0, -350, DateTime.Now, DateTime.Now },
                    { 44, "中将1", 20000, 0, 13000, 0, DateTime.Now, DateTime.Now },
                    { 45, "中将2", 20000, 0, 13000, -250, DateTime.Now, DateTime.Now },
                    { 46, "中将3", 20000, 0, 13000, -250, DateTime.Now, DateTime.Now },
                    { 47, "中将4", 20000, 0, 13000, -250, DateTime.Now, DateTime.Now },
                    { 48, "中将5", 35000, 0, 18000, -350, DateTime.Now, DateTime.Now },
                    { 49, "大将1", 25000, 0, 18000, 0, DateTime.Now, DateTime.Now },
                    { 50, "大将2", 25000, 0, 18000, -200, DateTime.Now, DateTime.Now },
                    { 51, "大将3", 25000, 0, 18000, -200, DateTime.Now, DateTime.Now },
                    { 52, "大将4", 25000, 0, 18000, -200, DateTime.Now, DateTime.Now },
                    { 53, "大将5", 50000, 0, 26000, -300, DateTime.Now, DateTime.Now },
                    { 54, "元帥", 500, 0, 0, 0, DateTime.Now, DateTime.Now },
                    { 55, "大元帥", 0, 0, 0, 0, DateTime.Now, DateTime.Now }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "common_echelon_setting");
        }
    }
}
