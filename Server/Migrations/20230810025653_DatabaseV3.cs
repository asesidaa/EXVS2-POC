using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploadToken",
                table: "card_profile",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadTokenExpiry",
                table: "card_profile",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "upload_image",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_upload_image", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_upload_image_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_upload_image_CardId",
                table: "upload_image",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "upload_image");

            migrationBuilder.DropColumn(
                name: "UploadToken",
                table: "card_profile");

            migrationBuilder.DropColumn(
                name: "UploadTokenExpiry",
                table: "card_profile");
        }
    }
}
