using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerVanilla.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exvs2_card_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Gp = table.Column<uint>(type: "INTEGER", nullable: false),
                    AccessCode = table.Column<string>(type: "TEXT", nullable: false),
                    ChipId = table.Column<string>(type: "TEXT", nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    IsNewCard = table.Column<bool>(type: "INTEGER", nullable: false),
                    UploadToken = table.Column<string>(type: "TEXT", nullable: false),
                    UploadTokenExpiry = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastPlayedAt = table.Column<ulong>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_card_profile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_battle_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    EchelonId = table.Column<uint>(type: "INTEGER", nullable: false),
                    EchelonExp = table.Column<int>(type: "INTEGER", nullable: false),
                    SEchelonFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    SEchelonMissionFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    SEchelonProgress = table.Column<uint>(type: "INTEGER", nullable: false),
                    SCaptainFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    SBrigadierFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    MatchingCorrectionSolo = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchingCorrectionTeam = table.Column<int>(type: "INTEGER", nullable: false),
                    VsmAfterRankUp = table.Column<uint>(type: "INTEGER", nullable: false),
                    ShuffleWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    ShuffleLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    SoloRankPoint = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamRankPoint = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_battle_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_battle_profile_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_boost_setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    GpBoost = table.Column<uint>(type: "INTEGER", nullable: false),
                    GuestNavBoost = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_boost_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_boost_setting_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_customize_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultGaugeDesignId = table.Column<uint>(type: "INTEGER", nullable: false),
                    DefaultBgmSettings = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultBgmPlayMethod = table.Column<uint>(type: "INTEGER", nullable: false),
                    StageRandoms = table.Column<string>(type: "TEXT", nullable: false),
                    BasePanelId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CommentPartsAId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CommentPartsBId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_customize_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_customize_profile_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_default_title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CardProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_default_title", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_default_title_exvs2_card_profile_CardProfileId",
                        column: x => x.CardProfileId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_favourite_ms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    MstMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    OpenSkillpoint = table.Column<bool>(type: "INTEGER", nullable: false),
                    GaugeDesignId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BgmSettings = table.Column<string>(type: "TEXT", nullable: false),
                    BgmPlayMethod = table.Column<uint>(type: "INTEGER", nullable: false),
                    BattleNavId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_favourite_ms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_favourite_ms_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2_ms_usage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    MstMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MsUsedNum = table.Column<uint>(type: "INTEGER", nullable: false),
                    CostumeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_ms_usage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_ms_usage_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2_navi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestNavId = table.Column<uint>(type: "INTEGER", nullable: false),
                    GuestNavSettingFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    GuestNavRemains = table.Column<uint>(type: "INTEGER", nullable: false),
                    BattleNavSettingFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    BattleNavRemains = table.Column<uint>(type: "INTEGER", nullable: false),
                    GuestNavCostume = table.Column<uint>(type: "INTEGER", nullable: false),
                    GuestNavFamiliarity = table.Column<uint>(type: "INTEGER", nullable: false),
                    NewCostumeFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_navi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_navi_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2_navi_setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    BattleNavAdviseFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    BattleNavNotifyFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_navi_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_navi_setting_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_player_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    OpenRecord = table.Column<uint>(type: "INTEGER", nullable: false),
                    OpenEchelon = table.Column<uint>(type: "INTEGER", nullable: false),
                    OpenSkillpoint = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_player_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_player_profile_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_tag_team_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamName = table.Column<string>(type: "TEXT", nullable: false),
                    TeammateCardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    OnlineRankPoint = table.Column<int>(type: "INTEGER", nullable: false),
                    BackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    EffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    EmblemId = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkillPoint = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkillPointBoost = table.Column<uint>(type: "INTEGER", nullable: false),
                    TagRate = table.Column<uint>(type: "INTEGER", nullable: false),
                    BgmId = table.Column<uint>(type: "INTEGER", nullable: false),
                    NameColorId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_tag_team_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_tag_team_data_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2_training_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    MstMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    CpuLevel = table.Column<uint>(type: "INTEGER", nullable: false),
                    ExBurstGauge = table.Column<uint>(type: "INTEGER", nullable: false),
                    DamageDisplay = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_training_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_training_profile_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_triad_course_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<uint>(type: "INTEGER", nullable: false),
                    ReleasedAt = table.Column<ulong>(type: "INTEGER", nullable: false),
                    TotalPlayNum = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalClearNum = table.Column<uint>(type: "INTEGER", nullable: false),
                    Highscore = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_triad_course_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_triad_course_data_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2_triad_misc_info",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    CpuRibbons = table.Column<string>(type: "TEXT", nullable: false),
                    TotalTriadScore = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalTriadWantedDefeatNum = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalTriadScenePlayNum = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_triad_misc_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_triad_misc_info_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2_triad_partner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    MstMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    ArmorLevel = table.Column<uint>(type: "INTEGER", nullable: false),
                    ShootAttackLevel = table.Column<uint>(type: "INTEGER", nullable: false),
                    InfightAttackLevel = table.Column<uint>(type: "INTEGER", nullable: false),
                    BoosterLevel = table.Column<uint>(type: "INTEGER", nullable: false),
                    ExGaugeLevel = table.Column<uint>(type: "INTEGER", nullable: false),
                    AiLevel = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    MsSkill1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    MsSkill2 = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2_triad_partner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2_triad_partner_exvs2_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_battle_profile_CardId",
                table: "exvs2_battle_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_battle_profile_Id",
                table: "exvs2_battle_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_boost_setting_CardId",
                table: "exvs2_boost_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_boost_setting_Id",
                table: "exvs2_boost_setting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_card_profile_Id",
                table: "exvs2_card_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_customize_profile_CardId",
                table: "exvs2_customize_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_customize_profile_Id",
                table: "exvs2_customize_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_default_title_CardId",
                table: "exvs2_default_title",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_default_title_CardProfileId",
                table: "exvs2_default_title",
                column: "CardProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_default_title_Id",
                table: "exvs2_default_title",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_favourite_ms_CardId",
                table: "exvs2_favourite_ms",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_favourite_ms_CardId_MstMobileSuitId",
                table: "exvs2_favourite_ms",
                columns: new[] { "CardId", "MstMobileSuitId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_favourite_ms_Id",
                table: "exvs2_favourite_ms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_ms_usage_CardId",
                table: "exvs2_ms_usage",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_ms_usage_CardId_MstMobileSuitId",
                table: "exvs2_ms_usage",
                columns: new[] { "CardId", "MstMobileSuitId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_ms_usage_Id",
                table: "exvs2_ms_usage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_navi_CardId",
                table: "exvs2_navi",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_navi_CardId_GuestNavId",
                table: "exvs2_navi",
                columns: new[] { "CardId", "GuestNavId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_navi_Id",
                table: "exvs2_navi",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_navi_setting_CardId",
                table: "exvs2_navi_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_navi_setting_Id",
                table: "exvs2_navi_setting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_player_profile_CardId",
                table: "exvs2_player_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_player_profile_Id",
                table: "exvs2_player_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_tag_team_data_CardId",
                table: "exvs2_tag_team_data",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_tag_team_data_Id",
                table: "exvs2_tag_team_data",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_training_profile_CardId",
                table: "exvs2_training_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_training_profile_Id",
                table: "exvs2_training_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_triad_course_data_CardId_CourseId",
                table: "exvs2_triad_course_data",
                columns: new[] { "CardId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_triad_course_data_Id",
                table: "exvs2_triad_course_data",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_triad_misc_info_CardId",
                table: "exvs2_triad_misc_info",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_triad_misc_info_Id",
                table: "exvs2_triad_misc_info",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_triad_partner_CardId",
                table: "exvs2_triad_partner",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2_triad_partner_Id",
                table: "exvs2_triad_partner",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exvs2_battle_profile");

            migrationBuilder.DropTable(
                name: "exvs2_boost_setting");

            migrationBuilder.DropTable(
                name: "exvs2_customize_profile");

            migrationBuilder.DropTable(
                name: "exvs2_default_title");

            migrationBuilder.DropTable(
                name: "exvs2_favourite_ms");

            migrationBuilder.DropTable(
                name: "exvs2_ms_usage");

            migrationBuilder.DropTable(
                name: "exvs2_navi");

            migrationBuilder.DropTable(
                name: "exvs2_navi_setting");

            migrationBuilder.DropTable(
                name: "exvs2_player_profile");

            migrationBuilder.DropTable(
                name: "exvs2_tag_team_data");

            migrationBuilder.DropTable(
                name: "exvs2_training_profile");

            migrationBuilder.DropTable(
                name: "exvs2_triad_course_data");

            migrationBuilder.DropTable(
                name: "exvs2_triad_misc_info");

            migrationBuilder.DropTable(
                name: "exvs2_triad_partner");

            migrationBuilder.DropTable(
                name: "exvs2_card_profile");
        }
    }
}
