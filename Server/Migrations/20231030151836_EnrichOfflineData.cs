using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class EnrichOfflineData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foe1Indicator",
                table: "offline_pvp_battle_result",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Foe2Indicator",
                table: "offline_pvp_battle_result",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PartnerIndicator",
                table: "offline_pvp_battle_result",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "snapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SnapshotType = table.Column<string>(type: "TEXT", nullable: false),
                    SnapshotData = table.Column<string>(type: "TEXT", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_snapshot", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "snapshot");

            migrationBuilder.DropColumn(
                name: "Foe1Indicator",
                table: "offline_pvp_battle_result");

            migrationBuilder.DropColumn(
                name: "Foe2Indicator",
                table: "offline_pvp_battle_result");

            migrationBuilder.DropColumn(
                name: "PartnerIndicator",
                table: "offline_pvp_battle_result");
        }
    }
}
