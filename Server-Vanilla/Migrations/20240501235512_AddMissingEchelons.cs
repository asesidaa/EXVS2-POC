using Microsoft.EntityFrameworkCore.Migrations;
using ServerVanilla.Models.Cards.Common;

#nullable disable

namespace ServerVanilla.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingEchelons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "exvs2_common_echelon_setting",
                columns: new[]
                {
                    nameof(EchelonSetting.EchelonId),
                    nameof(EchelonSetting.OnlineMatchRankS), nameof(EchelonSetting.OnlineMatchRankT),
                    nameof(EchelonSetting.OnlineMatchRankE),
                    nameof(EchelonSetting.MsAdjustFlag),
                    nameof(EchelonSetting.DownDefaultExp), nameof(EchelonSetting.UpDefaultExp),
                    nameof(EchelonSetting.WinCorrectionRate), nameof(EchelonSetting.LoseCorrectionRate),
                    nameof(EchelonSetting.ExpWidth), nameof(EchelonSetting.DowngradeThreshold),
                    nameof(EchelonSetting.CreateTime), nameof(EchelonSetting.UpdateTime),
                },
                values: new object[,]
                {
                    {
                        0,
                        0, 0, 0,
                        true,
                        0, 0,
                        0, 0,
                        1, 0,
                        DateTime.Now, DateTime.Now
                    }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
