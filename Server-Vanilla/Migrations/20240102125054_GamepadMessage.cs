using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerVanilla.Migrations
{
    /// <inheritdoc />
    public partial class GamepadMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exvs2_gamepad_setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    XKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    YKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    AKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    BKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    LbKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    RbKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    LtKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    RtKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    LsbKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    RsbKey = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_gamepad_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_gamepad_setting_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_online_opening_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    TopUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    DownMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    DownUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LeftMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    LeftUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    RightMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    RightUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_online_opening_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_online_opening_message_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_online_playing_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    TopUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    DownMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    DownUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LeftMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    LeftUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    RightMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    RightUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_online_playing_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_online_playing_message_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_online_result_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    TopUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    DownMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    DownUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LeftMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    LeftUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    RightMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    RightUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_online_result_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_online_result_message_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_opening_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    TopUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    DownMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    DownUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LeftMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    LeftUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    RightMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    RightUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_opening_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_opening_message_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_playing_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    TopUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    DownMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    DownUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LeftMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    LeftUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    RightMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    RightUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_playing_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_playing_message_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_result_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    TopUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    DownMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    DownUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LeftMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    LeftUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    RightMessageText = table.Column<string>(type: "TEXT", nullable: false),
                    RightUniqueMessageId = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_result_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_result_message_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_gamepad_setting_CardId",
                table: "exvs2_gamepad_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_gamepad_setting_Id",
                table: "exvs2_gamepad_setting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_online_opening_message_CardId",
                table: "exvs2_online_opening_message",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_online_opening_message_Id",
                table: "exvs2_online_opening_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_online_playing_message_CardId",
                table: "exvs2_online_playing_message",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_online_playing_message_Id",
                table: "exvs2_online_playing_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_online_result_message_CardId",
                table: "exvs2_online_result_message",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_online_result_message_Id",
                table: "exvs2_online_result_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_opening_message_CardId",
                table: "exvs2_opening_message",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_opening_message_Id",
                table: "exvs2_opening_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_playing_message_CardId",
                table: "exvs2_playing_message",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_playing_message_Id",
                table: "exvs2_playing_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_result_message_CardId",
                table: "exvs2_result_message",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_result_message_Id",
                table: "exvs2_result_message",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exvs2_gamepad_setting");

            migrationBuilder.DropTable(
                name: "exvs2_online_opening_message");

            migrationBuilder.DropTable(
                name: "exvs2_online_playing_message");

            migrationBuilder.DropTable(
                name: "exvs2_online_result_message");

            migrationBuilder.DropTable(
                name: "exvs2_opening_message");

            migrationBuilder.DropTable(
                name: "exvs2_playing_message");

            migrationBuilder.DropTable(
                name: "exvs2_result_message");
        }
    }
}
