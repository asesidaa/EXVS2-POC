using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerVanilla.Migrations
{
    /// <inheritdoc />
    public partial class AddTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exvs2_default_title_exvs2_card_profile_CardProfileId",
                table: "exvs2_default_title");

            migrationBuilder.DropIndex(
                name: "IX_exvs2_default_title_CardId",
                table: "exvs2_default_title");

            migrationBuilder.DropIndex(
                name: "IX_exvs2_default_title_CardProfileId",
                table: "exvs2_default_title");

            migrationBuilder.DropColumn(
                name: "CardProfileId",
                table: "exvs2_default_title");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_default_title_CardId",
                table: "exvs2_default_title",
                column: "CardId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_exvs2_default_title_exvs2_card_profile_CardId",
                table: "exvs2_default_title",
                column: "CardId",
                principalTable: "exvs2_card_profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exvs2_default_title_exvs2_card_profile_CardId",
                table: "exvs2_default_title");

            migrationBuilder.DropIndex(
                name: "IX_exvs2_default_title_CardId",
                table: "exvs2_default_title");

            migrationBuilder.AddColumn<int>(
                name: "CardProfileId",
                table: "exvs2_default_title",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_default_title_CardId",
                table: "exvs2_default_title",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_default_title_CardProfileId",
                table: "exvs2_default_title",
                column: "CardProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_exvs2_default_title_exvs2_card_profile_CardProfileId",
                table: "exvs2_default_title",
                column: "CardProfileId",
                principalTable: "exvs2_card_profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
