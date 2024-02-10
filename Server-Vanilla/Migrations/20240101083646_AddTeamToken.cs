using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerVanilla.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistinctTeamFormationToken",
                table: "exvs2_card_profile",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistinctTeamFormationToken",
                table: "exvs2_card_profile");
        }
    }
}
