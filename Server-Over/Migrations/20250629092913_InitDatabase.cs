using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exvs2ob_card_profile",
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
                    DistinctTeamFormationToken = table.Column<string>(type: "TEXT", nullable: false),
                    LastPlayedAt = table.Column<ulong>(type: "INTEGER", nullable: false),
                    LastLoginCabinet = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_card_profile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_invalid_visit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Token = table.Column<string>(type: "TEXT", maxLength: 384, nullable: false),
                    Ip = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_invalid_visit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_history",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    BattleMode = table.Column<string>(type: "TEXT", nullable: false),
                    IsWin = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlayedAt = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ElapsedSeconds = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<uint>(type: "INTEGER", nullable: false),
                    StageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Score = table.Column<uint>(type: "INTEGER", nullable: false),
                    ScoreRank = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalExBurstDamage = table.Column<uint>(type: "INTEGER", nullable: false),
                    GivenDamage = table.Column<uint>(type: "INTEGER", nullable: false),
                    TakenDamage = table.Column<uint>(type: "INTEGER", nullable: false),
                    OverheatCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    ComboGivenDamage = table.Column<uint>(type: "INTEGER", nullable: false),
                    ConsecutiveWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_history_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_player_level",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerLevelId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PrestigeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerExp = table.Column<uint>(type: "INTEGER", nullable: false),
                    LevelMaxDispFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_player_level", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_player_level_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_solo_class_record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Rate = table.Column<float>(type: "REAL", nullable: false),
                    MaxPosition = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClassChangeStatus = table.Column<uint>(type: "INTEGER", nullable: false),
                    WeeklyTotalBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    WeeklyTotalWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TopPointRankEntryCount = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_solo_class_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_solo_class_record_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_team_class_record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Rate = table.Column<float>(type: "REAL", nullable: false),
                    MaxPosition = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClassChangeStatus = table.Column<uint>(type: "INTEGER", nullable: false),
                    WeeklyTotalBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    WeeklyTotalWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TopPointRankEntryCount = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_team_class_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_team_class_record_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_win_loss_record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    ShuffleWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    ShuffleLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClassSoloWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClassSoloLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClassTeamWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClassTeamLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    FesWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    FesLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    FreeWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    FreeLose = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_win_loss_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_win_loss_record_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_boost_setting",
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
                    table.PrimaryKey("PK_exvs2ob_boost_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_boost_setting_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_challenge_mission_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    EffectiveYear = table.Column<uint>(type: "INTEGER", nullable: false),
                    EffectiveMonth = table.Column<uint>(type: "INTEGER", nullable: false),
                    EffectiveDay = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalBattleWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    MaxConsecutiveWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalDefeatCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalDamageCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_challenge_mission_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_challenge_mission_profile_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_customize_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultGaugeDesignId = table.Column<uint>(type: "INTEGER", nullable: false),
                    DefaultBgmSettings = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultBgmPlayMethod = table.Column<uint>(type: "INTEGER", nullable: false),
                    StageRandoms = table.Column<string>(type: "TEXT", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_customize_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_customize_profile_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_default_sticker_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    BasePanelId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CommentPartsAId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CommentPartsBId = table.Column<uint>(type: "INTEGER", nullable: false),
                    StickerBackgroundId = table.Column<uint>(type: "INTEGER", nullable: false),
                    StickerEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Tracker1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Tracker2 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Tracker3 = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_default_sticker_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_default_sticker_profile_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_favourite_ms",
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
                    table.PrimaryKey("PK_exvs2ob_favourite_ms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_favourite_ms_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_gamepad_setting",
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
                    table.PrimaryKey("PK_exvs2ob_gamepad_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_gamepad_setting_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_general_setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    FixPositionRadar = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_general_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_general_setting_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_license_score_record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseScore = table.Column<uint>(type: "INTEGER", nullable: false),
                    LastObtainedScore = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_license_score_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_license_score_record_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_message_setting",
                columns: table => new
                {
                    MessageSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    MessagePosition = table.Column<uint>(type: "INTEGER", nullable: false),
                    AllowReceiveMessage = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_message_setting", x => x.MessageSettingId);
                    table.ForeignKey(
                        name: "FK_exvs2ob_message_setting_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_ms_pvp_stat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    MstMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalGivenDamage = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalEnemyDefeatedCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalClassMatchTenConsecutiveWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalNoDamageBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalExBurstDamage = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_ms_pvp_stat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_ms_pvp_stat_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_ms_sticker",
                columns: table => new
                {
                    StickerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    MstMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PoseId = table.Column<uint>(type: "INTEGER", nullable: false),
                    StickerBackgroundId = table.Column<uint>(type: "INTEGER", nullable: false),
                    StickerEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Tracker1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Tracker2 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Tracker3 = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_ms_sticker", x => x.StickerId);
                    table.ForeignKey(
                        name: "FK_exvs2ob_ms_sticker_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_ms_usage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    MstMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MsUsedNum = table.Column<uint>(type: "INTEGER", nullable: false),
                    CostumeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TriadBuddyPoint = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkinId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_ms_usage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_ms_usage_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_navi",
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
                    table.PrimaryKey("PK_exvs2ob_navi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_navi_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_navi_setting",
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
                    table.PrimaryKey("PK_exvs2ob_navi_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_navi_setting_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_online_pair",
                columns: table => new
                {
                    PairId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_online_pair", x => x.PairId);
                    table.ForeignKey(
                        name: "FK_exvs2ob_online_pair_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_player_battle_statistic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalGivenDamage = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalEnemyDefeatedCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalClassMatchTenConsecutiveWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalNoDamageBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalExBurstDamage = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_player_battle_statistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_player_battle_statistic_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_player_burst_statistic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalBattle = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalWin = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_player_burst_statistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_player_burst_statistic_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_player_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    OpenRecord = table.Column<uint>(type: "INTEGER", nullable: false),
                    OpenEchelon = table.Column<uint>(type: "INTEGER", nullable: false),
                    OpenSkillpoint = table.Column<bool>(type: "INTEGER", nullable: false),
                    FullLoadCardCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    ExTutorialDispFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_player_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_player_profile_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_pre_battle_history",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentConsecutiveWins = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_pre_battle_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_pre_battle_history_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_shared_upload_replay",
                columns: table => new
                {
                    ReplayId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    ReplaySize = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayedAt = table.Column<ulong>(type: "INTEGER", nullable: false),
                    StageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PilotsJson = table.Column<string>(type: "TEXT", nullable: false),
                    SpecialFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReplayServiceFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    MobileUserId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MatchingMode = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamType = table.Column<uint>(type: "INTEGER", nullable: false),
                    ReturnMatchFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    TournamentId = table.Column<uint>(type: "INTEGER", nullable: true),
                    BattleClass = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_shared_upload_replay", x => x.ReplayId);
                    table.ForeignKey(
                        name: "FK_exvs2ob_shared_upload_replay_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_tag_team_data",
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
                    BgmId = table.Column<uint>(type: "INTEGER", nullable: false),
                    NameColorId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BoostRemains = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_tag_team_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_tag_team_data_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_team_setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuickOnlineTagCardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_team_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_team_setting_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_training_profile",
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
                    CpuAutoGuard = table.Column<bool>(type: "INTEGER", nullable: false),
                    CommandGuideDisplay = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_training_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_training_profile_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_triad_clear_time",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<uint>(type: "INTEGER", nullable: false),
                    Month = table.Column<uint>(type: "INTEGER", nullable: false),
                    CourseClearTime = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_triad_clear_time", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_triad_clear_time_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_triad_course_data",
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
                    table.PrimaryKey("PK_exvs2ob_triad_course_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_triad_course_data_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_triad_high_score",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<uint>(type: "INTEGER", nullable: false),
                    Month = table.Column<uint>(type: "INTEGER", nullable: false),
                    CourseHighScore = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_triad_high_score", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_triad_high_score_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_triad_misc_info",
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
                    table.PrimaryKey("PK_exvs2ob_triad_misc_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_triad_misc_info_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_triad_partner",
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
                    TriadTeamName = table.Column<string>(type: "TEXT", nullable: false),
                    TriadBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_triad_partner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_triad_partner_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_triad_target_defeated_count",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<uint>(type: "INTEGER", nullable: false),
                    Month = table.Column<uint>(type: "INTEGER", nullable: false),
                    DestroyCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_triad_target_defeated_count", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_triad_target_defeated_count_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_triad_wanted_defeated_count",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<uint>(type: "INTEGER", nullable: false),
                    Month = table.Column<uint>(type: "INTEGER", nullable: false),
                    DestroyCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_triad_wanted_defeated_count", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_triad_wanted_defeated_count_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_upload_image",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_upload_image", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_exvs2ob_upload_image_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_upload_replay",
                columns: table => new
                {
                    ReplayId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    ReplaySize = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayedAt = table.Column<ulong>(type: "INTEGER", nullable: false),
                    StageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PilotsJson = table.Column<string>(type: "TEXT", nullable: false),
                    SpecialFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReplayServiceFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    MobileUserId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MatchingMode = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamType = table.Column<uint>(type: "INTEGER", nullable: false),
                    ReturnMatchFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    TournamentId = table.Column<uint>(type: "INTEGER", nullable: true),
                    BattleClass = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_upload_replay", x => x.ReplayId);
                    table.ForeignKey(
                        name: "FK_exvs2ob_upload_replay_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_user_private_match_room_setting",
                columns: table => new
                {
                    PrivateMatchRoomSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnablePrivateMatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPrivateMatchHost = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParticipatedPrivateRoomId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_user_private_match_room_setting", x => x.PrivateMatchRoomSettingId);
                    table.ForeignKey(
                        name: "FK_exvs2ob_user_private_match_room_setting_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_user_title_setting",
                columns: table => new
                {
                    UserTitleSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    RandomTitleFlag = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_user_title_setting", x => x.UserTitleSettingId);
                    table.ForeignKey(
                        name: "FK_exvs2ob_user_title_setting_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_action_log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BattleHistoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    FrameTime = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    ActionType = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_action_log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_action_log_exvs2ob_battle_history_BattleHistoryId",
                        column: x => x.BattleHistoryId,
                        principalTable: "exvs2ob_battle_history",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_ally",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BattleHistoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    ClassId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PrestigeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LevelId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkinId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Mastery = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_ally", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_ally_exvs2ob_battle_history_BattleHistoryId",
                        column: x => x.BattleHistoryId,
                        principalTable: "exvs2ob_battle_history",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_self",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BattleHistoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    ClassId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PrestigeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LevelId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkinId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Mastery = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_self", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_self_exvs2ob_battle_history_BattleHistoryId",
                        column: x => x.BattleHistoryId,
                        principalTable: "exvs2ob_battle_history",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_battle_target",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BattleHistoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    ClassId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PrestigeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    LevelId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkinId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Mastery = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_battle_target", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_battle_target_exvs2ob_battle_history_BattleHistoryId",
                        column: x => x.BattleHistoryId,
                        principalTable: "exvs2ob_battle_history",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_ms_class_match_title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CustomText = table.Column<string>(type: "TEXT", nullable: false),
                    FavouriteMobileSuitId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_ms_class_match_title", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_ms_class_match_title_exvs2ob_favourite_ms_FavouriteMobileSuitId",
                        column: x => x.FavouriteMobileSuitId,
                        principalTable: "exvs2ob_favourite_ms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_ms_default_title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CustomText = table.Column<string>(type: "TEXT", nullable: false),
                    FavouriteMobileSuitId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_ms_default_title", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_ms_default_title_exvs2ob_favourite_ms_FavouriteMobileSuitId",
                        column: x => x.FavouriteMobileSuitId,
                        principalTable: "exvs2ob_favourite_ms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_ms_triad_title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CustomText = table.Column<string>(type: "TEXT", nullable: false),
                    FavouriteMobileSuitId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_ms_triad_title", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_ms_triad_title_exvs2ob_favourite_ms_FavouriteMobileSuitId",
                        column: x => x.FavouriteMobileSuitId,
                        principalTable: "exvs2ob_favourite_ms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_online_shuffle_opening_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MessageSettingId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_exvs2ob_online_shuffle_opening_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_online_shuffle_opening_message_exvs2ob_message_setting_MessageSettingId",
                        column: x => x.MessageSettingId,
                        principalTable: "exvs2ob_message_setting",
                        principalColumn: "MessageSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_online_shuffle_playing_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MessageSettingId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_exvs2ob_online_shuffle_playing_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_online_shuffle_playing_message_exvs2ob_message_setting_MessageSettingId",
                        column: x => x.MessageSettingId,
                        principalTable: "exvs2ob_message_setting",
                        principalColumn: "MessageSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_online_shuffle_result_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MessageSettingId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_exvs2ob_online_shuffle_result_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_online_shuffle_result_message_exvs2ob_message_setting_MessageSettingId",
                        column: x => x.MessageSettingId,
                        principalTable: "exvs2ob_message_setting",
                        principalColumn: "MessageSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_opening_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MessageSettingId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_exvs2ob_opening_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_opening_message_exvs2ob_message_setting_MessageSettingId",
                        column: x => x.MessageSettingId,
                        principalTable: "exvs2ob_message_setting",
                        principalColumn: "MessageSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_playing_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MessageSettingId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_exvs2ob_playing_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_playing_message_exvs2ob_message_setting_MessageSettingId",
                        column: x => x.MessageSettingId,
                        principalTable: "exvs2ob_message_setting",
                        principalColumn: "MessageSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_result_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MessageSettingId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_exvs2ob_result_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_result_message_exvs2ob_message_setting_MessageSettingId",
                        column: x => x.MessageSettingId,
                        principalTable: "exvs2ob_message_setting",
                        principalColumn: "MessageSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_private_match_room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PrivateMatchRoomSettingId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagName = table.Column<string>(type: "TEXT", nullable: false),
                    TagType = table.Column<uint>(type: "INTEGER", nullable: false),
                    MatchingType = table.Column<uint>(type: "INTEGER", nullable: false),
                    MatchingAttribute = table.Column<uint>(type: "INTEGER", nullable: false),
                    RuleType = table.Column<uint>(type: "INTEGER", nullable: false),
                    SelectableMsIds = table.Column<string>(type: "TEXT", nullable: false),
                    RevengeFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_private_match_room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_private_match_room_exvs2ob_user_private_match_room_setting_PrivateMatchRoomSettingId",
                        column: x => x.PrivateMatchRoomSettingId,
                        principalTable: "exvs2ob_user_private_match_room_setting",
                        principalColumn: "PrivateMatchRoomSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_user_class_match_title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CustomText = table.Column<string>(type: "TEXT", nullable: false),
                    UserTitleSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_user_class_match_title", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_user_class_match_title_exvs2ob_user_title_setting_UserTitleSettingId",
                        column: x => x.UserTitleSettingId,
                        principalTable: "exvs2ob_user_title_setting",
                        principalColumn: "UserTitleSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_user_default_title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CustomText = table.Column<string>(type: "TEXT", nullable: false),
                    UserTitleSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_user_default_title", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_user_default_title_exvs2ob_user_title_setting_UserTitleSettingId",
                        column: x => x.UserTitleSettingId,
                        principalTable: "exvs2ob_user_title_setting",
                        principalColumn: "UserTitleSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exvs2ob_user_triad_title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CustomText = table.Column<string>(type: "TEXT", nullable: false),
                    UserTitleSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_user_triad_title", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_user_triad_title_exvs2ob_user_title_setting_UserTitleSettingId",
                        column: x => x.UserTitleSettingId,
                        principalTable: "exvs2ob_user_title_setting",
                        principalColumn: "UserTitleSettingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_action_log_BattleHistoryId_CardId",
                table: "exvs2ob_battle_action_log",
                columns: new[] { "BattleHistoryId", "CardId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_action_log_Id",
                table: "exvs2ob_battle_action_log",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_action_log_Id_BattleHistoryId",
                table: "exvs2ob_battle_action_log",
                columns: new[] { "Id", "BattleHistoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_ally_BattleHistoryId",
                table: "exvs2ob_battle_ally",
                column: "BattleHistoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_ally_BattleHistoryId_CardId",
                table: "exvs2ob_battle_ally",
                columns: new[] { "BattleHistoryId", "CardId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_ally_Id",
                table: "exvs2ob_battle_ally",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_ally_Id_BattleHistoryId",
                table: "exvs2ob_battle_ally",
                columns: new[] { "Id", "BattleHistoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_history_CardId",
                table: "exvs2ob_battle_history",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_history_Id",
                table: "exvs2ob_battle_history",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_history_Id_CardId",
                table: "exvs2ob_battle_history",
                columns: new[] { "Id", "CardId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_history_PlayedAt",
                table: "exvs2ob_battle_history",
                column: "PlayedAt");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_player_level_CardId",
                table: "exvs2ob_battle_player_level",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_player_level_Id",
                table: "exvs2ob_battle_player_level",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_self_BattleHistoryId",
                table: "exvs2ob_battle_self",
                column: "BattleHistoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_self_BattleHistoryId_CardId",
                table: "exvs2ob_battle_self",
                columns: new[] { "BattleHistoryId", "CardId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_self_Id",
                table: "exvs2ob_battle_self",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_self_Id_BattleHistoryId",
                table: "exvs2ob_battle_self",
                columns: new[] { "Id", "BattleHistoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_solo_class_record_CardId",
                table: "exvs2ob_battle_solo_class_record",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_solo_class_record_Id",
                table: "exvs2ob_battle_solo_class_record",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_target_BattleHistoryId_CardId",
                table: "exvs2ob_battle_target",
                columns: new[] { "BattleHistoryId", "CardId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_target_Id",
                table: "exvs2ob_battle_target",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_target_Id_BattleHistoryId",
                table: "exvs2ob_battle_target",
                columns: new[] { "Id", "BattleHistoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_team_class_record_CardId",
                table: "exvs2ob_battle_team_class_record",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_team_class_record_Id",
                table: "exvs2ob_battle_team_class_record",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_win_loss_record_CardId",
                table: "exvs2ob_battle_win_loss_record",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_battle_win_loss_record_Id",
                table: "exvs2ob_battle_win_loss_record",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_boost_setting_CardId",
                table: "exvs2ob_boost_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_boost_setting_Id",
                table: "exvs2ob_boost_setting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_card_profile_Id",
                table: "exvs2ob_card_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_challenge_mission_profile_CardId",
                table: "exvs2ob_challenge_mission_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_challenge_mission_profile_CardId_EffectiveYear_EffectiveMonth_EffectiveDay",
                table: "exvs2ob_challenge_mission_profile",
                columns: new[] { "CardId", "EffectiveYear", "EffectiveMonth", "EffectiveDay" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_challenge_mission_profile_Id",
                table: "exvs2ob_challenge_mission_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_customize_profile_CardId",
                table: "exvs2ob_customize_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_customize_profile_Id",
                table: "exvs2ob_customize_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_default_sticker_profile_CardId",
                table: "exvs2ob_default_sticker_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_default_sticker_profile_Id",
                table: "exvs2ob_default_sticker_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_favourite_ms_CardId",
                table: "exvs2ob_favourite_ms",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_favourite_ms_CardId_MstMobileSuitId",
                table: "exvs2ob_favourite_ms",
                columns: new[] { "CardId", "MstMobileSuitId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_favourite_ms_Id",
                table: "exvs2ob_favourite_ms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_gamepad_setting_CardId",
                table: "exvs2ob_gamepad_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_gamepad_setting_Id",
                table: "exvs2ob_gamepad_setting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_general_setting_CardId",
                table: "exvs2ob_general_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_general_setting_Id",
                table: "exvs2ob_general_setting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_invalid_visit_Id",
                table: "exvs2ob_invalid_visit",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_license_score_record_CardId",
                table: "exvs2ob_license_score_record",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_license_score_record_Id",
                table: "exvs2ob_license_score_record",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_message_setting_CardId",
                table: "exvs2ob_message_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_message_setting_MessageSettingId",
                table: "exvs2ob_message_setting",
                column: "MessageSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_class_match_title_FavouriteMobileSuitId",
                table: "exvs2ob_ms_class_match_title",
                column: "FavouriteMobileSuitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_class_match_title_Id",
                table: "exvs2ob_ms_class_match_title",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_default_title_FavouriteMobileSuitId",
                table: "exvs2ob_ms_default_title",
                column: "FavouriteMobileSuitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_default_title_Id",
                table: "exvs2ob_ms_default_title",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_pvp_stat_CardId",
                table: "exvs2ob_ms_pvp_stat",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_pvp_stat_CardId_MstMobileSuitId",
                table: "exvs2ob_ms_pvp_stat",
                columns: new[] { "CardId", "MstMobileSuitId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_pvp_stat_Id",
                table: "exvs2ob_ms_pvp_stat",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_sticker_CardId",
                table: "exvs2ob_ms_sticker",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_sticker_CardId_MstMobileSuitId",
                table: "exvs2ob_ms_sticker",
                columns: new[] { "CardId", "MstMobileSuitId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_sticker_StickerId",
                table: "exvs2ob_ms_sticker",
                column: "StickerId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_triad_title_FavouriteMobileSuitId",
                table: "exvs2ob_ms_triad_title",
                column: "FavouriteMobileSuitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_triad_title_Id",
                table: "exvs2ob_ms_triad_title",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_usage_CardId",
                table: "exvs2ob_ms_usage",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_usage_CardId_MstMobileSuitId",
                table: "exvs2ob_ms_usage",
                columns: new[] { "CardId", "MstMobileSuitId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_ms_usage_Id",
                table: "exvs2ob_ms_usage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_navi_CardId",
                table: "exvs2ob_navi",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_navi_CardId_GuestNavId",
                table: "exvs2ob_navi",
                columns: new[] { "CardId", "GuestNavId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_navi_Id",
                table: "exvs2ob_navi",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_navi_setting_CardId",
                table: "exvs2ob_navi_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_navi_setting_Id",
                table: "exvs2ob_navi_setting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_online_pair_CardId",
                table: "exvs2ob_online_pair",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_online_shuffle_opening_message_Id",
                table: "exvs2ob_online_shuffle_opening_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_online_shuffle_opening_message_MessageSettingId",
                table: "exvs2ob_online_shuffle_opening_message",
                column: "MessageSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_online_shuffle_playing_message_Id",
                table: "exvs2ob_online_shuffle_playing_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_online_shuffle_playing_message_MessageSettingId",
                table: "exvs2ob_online_shuffle_playing_message",
                column: "MessageSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_online_shuffle_result_message_Id",
                table: "exvs2ob_online_shuffle_result_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_online_shuffle_result_message_MessageSettingId",
                table: "exvs2ob_online_shuffle_result_message",
                column: "MessageSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_opening_message_Id",
                table: "exvs2ob_opening_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_opening_message_MessageSettingId",
                table: "exvs2ob_opening_message",
                column: "MessageSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_player_battle_statistic_CardId",
                table: "exvs2ob_player_battle_statistic",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_player_battle_statistic_Id",
                table: "exvs2ob_player_battle_statistic",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_player_burst_statistic_CardId",
                table: "exvs2ob_player_burst_statistic",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_player_burst_statistic_CardId_BurstType",
                table: "exvs2ob_player_burst_statistic",
                columns: new[] { "CardId", "BurstType" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_player_burst_statistic_Id",
                table: "exvs2ob_player_burst_statistic",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_player_profile_CardId",
                table: "exvs2ob_player_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_player_profile_Id",
                table: "exvs2ob_player_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_playing_message_Id",
                table: "exvs2ob_playing_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_playing_message_MessageSettingId",
                table: "exvs2ob_playing_message",
                column: "MessageSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_pre_battle_history_CardId",
                table: "exvs2ob_pre_battle_history",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_pre_battle_history_Id",
                table: "exvs2ob_pre_battle_history",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_private_match_room_Id",
                table: "exvs2ob_private_match_room",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_private_match_room_PrivateMatchRoomSettingId",
                table: "exvs2ob_private_match_room",
                column: "PrivateMatchRoomSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_result_message_Id",
                table: "exvs2ob_result_message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_result_message_MessageSettingId",
                table: "exvs2ob_result_message",
                column: "MessageSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_shared_upload_replay_CardId_ReplayId",
                table: "exvs2ob_shared_upload_replay",
                columns: new[] { "CardId", "ReplayId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_shared_upload_replay_ReplayId",
                table: "exvs2ob_shared_upload_replay",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_tag_team_data_CardId",
                table: "exvs2ob_tag_team_data",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_team_setting_CardId",
                table: "exvs2ob_team_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_team_setting_Id",
                table: "exvs2ob_team_setting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_training_profile_CardId",
                table: "exvs2ob_training_profile",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_training_profile_Id",
                table: "exvs2ob_training_profile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_clear_time_CardId_Year_Month",
                table: "exvs2ob_triad_clear_time",
                columns: new[] { "CardId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_clear_time_Id",
                table: "exvs2ob_triad_clear_time",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_course_data_CardId_CourseId",
                table: "exvs2ob_triad_course_data",
                columns: new[] { "CardId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_course_data_Id",
                table: "exvs2ob_triad_course_data",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_high_score_CardId_Year_Month",
                table: "exvs2ob_triad_high_score",
                columns: new[] { "CardId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_high_score_Id",
                table: "exvs2ob_triad_high_score",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_misc_info_CardId",
                table: "exvs2ob_triad_misc_info",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_misc_info_Id",
                table: "exvs2ob_triad_misc_info",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_partner_CardId",
                table: "exvs2ob_triad_partner",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_partner_Id",
                table: "exvs2ob_triad_partner",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_target_defeated_count_CardId_Year_Month",
                table: "exvs2ob_triad_target_defeated_count",
                columns: new[] { "CardId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_target_defeated_count_Id",
                table: "exvs2ob_triad_target_defeated_count",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_wanted_defeated_count_CardId_Year_Month",
                table: "exvs2ob_triad_wanted_defeated_count",
                columns: new[] { "CardId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_triad_wanted_defeated_count_Id",
                table: "exvs2ob_triad_wanted_defeated_count",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_upload_image_CardId_ImageId",
                table: "exvs2ob_upload_image",
                columns: new[] { "CardId", "ImageId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_upload_image_ImageId",
                table: "exvs2ob_upload_image",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_upload_replay_CardId_ReplayId",
                table: "exvs2ob_upload_replay",
                columns: new[] { "CardId", "ReplayId" });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_upload_replay_ReplayId",
                table: "exvs2ob_upload_replay",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_class_match_title_Id",
                table: "exvs2ob_user_class_match_title",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_class_match_title_UserTitleSettingId",
                table: "exvs2ob_user_class_match_title",
                column: "UserTitleSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_default_title_Id",
                table: "exvs2ob_user_default_title",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_default_title_UserTitleSettingId",
                table: "exvs2ob_user_default_title",
                column: "UserTitleSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_private_match_room_setting_CardId",
                table: "exvs2ob_user_private_match_room_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_private_match_room_setting_PrivateMatchRoomSettingId",
                table: "exvs2ob_user_private_match_room_setting",
                column: "PrivateMatchRoomSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_title_setting_CardId",
                table: "exvs2ob_user_title_setting",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_title_setting_UserTitleSettingId",
                table: "exvs2ob_user_title_setting",
                column: "UserTitleSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_triad_title_Id",
                table: "exvs2ob_user_triad_title",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_user_triad_title_UserTitleSettingId",
                table: "exvs2ob_user_triad_title",
                column: "UserTitleSettingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exvs2ob_battle_action_log");

            migrationBuilder.DropTable(
                name: "exvs2ob_battle_ally");

            migrationBuilder.DropTable(
                name: "exvs2ob_battle_player_level");

            migrationBuilder.DropTable(
                name: "exvs2ob_battle_self");

            migrationBuilder.DropTable(
                name: "exvs2ob_battle_solo_class_record");

            migrationBuilder.DropTable(
                name: "exvs2ob_battle_target");

            migrationBuilder.DropTable(
                name: "exvs2ob_battle_team_class_record");

            migrationBuilder.DropTable(
                name: "exvs2ob_battle_win_loss_record");

            migrationBuilder.DropTable(
                name: "exvs2ob_boost_setting");

            migrationBuilder.DropTable(
                name: "exvs2ob_challenge_mission_profile");

            migrationBuilder.DropTable(
                name: "exvs2ob_customize_profile");

            migrationBuilder.DropTable(
                name: "exvs2ob_default_sticker_profile");

            migrationBuilder.DropTable(
                name: "exvs2ob_gamepad_setting");

            migrationBuilder.DropTable(
                name: "exvs2ob_general_setting");

            migrationBuilder.DropTable(
                name: "exvs2ob_invalid_visit");

            migrationBuilder.DropTable(
                name: "exvs2ob_license_score_record");

            migrationBuilder.DropTable(
                name: "exvs2ob_ms_class_match_title");

            migrationBuilder.DropTable(
                name: "exvs2ob_ms_default_title");

            migrationBuilder.DropTable(
                name: "exvs2ob_ms_pvp_stat");

            migrationBuilder.DropTable(
                name: "exvs2ob_ms_sticker");

            migrationBuilder.DropTable(
                name: "exvs2ob_ms_triad_title");

            migrationBuilder.DropTable(
                name: "exvs2ob_ms_usage");

            migrationBuilder.DropTable(
                name: "exvs2ob_navi");

            migrationBuilder.DropTable(
                name: "exvs2ob_navi_setting");

            migrationBuilder.DropTable(
                name: "exvs2ob_online_pair");

            migrationBuilder.DropTable(
                name: "exvs2ob_online_shuffle_opening_message");

            migrationBuilder.DropTable(
                name: "exvs2ob_online_shuffle_playing_message");

            migrationBuilder.DropTable(
                name: "exvs2ob_online_shuffle_result_message");

            migrationBuilder.DropTable(
                name: "exvs2ob_opening_message");

            migrationBuilder.DropTable(
                name: "exvs2ob_player_battle_statistic");

            migrationBuilder.DropTable(
                name: "exvs2ob_player_burst_statistic");

            migrationBuilder.DropTable(
                name: "exvs2ob_player_profile");

            migrationBuilder.DropTable(
                name: "exvs2ob_playing_message");

            migrationBuilder.DropTable(
                name: "exvs2ob_pre_battle_history");

            migrationBuilder.DropTable(
                name: "exvs2ob_private_match_room");

            migrationBuilder.DropTable(
                name: "exvs2ob_result_message");

            migrationBuilder.DropTable(
                name: "exvs2ob_shared_upload_replay");

            migrationBuilder.DropTable(
                name: "exvs2ob_tag_team_data");

            migrationBuilder.DropTable(
                name: "exvs2ob_team_setting");

            migrationBuilder.DropTable(
                name: "exvs2ob_training_profile");

            migrationBuilder.DropTable(
                name: "exvs2ob_triad_clear_time");

            migrationBuilder.DropTable(
                name: "exvs2ob_triad_course_data");

            migrationBuilder.DropTable(
                name: "exvs2ob_triad_high_score");

            migrationBuilder.DropTable(
                name: "exvs2ob_triad_misc_info");

            migrationBuilder.DropTable(
                name: "exvs2ob_triad_partner");

            migrationBuilder.DropTable(
                name: "exvs2ob_triad_target_defeated_count");

            migrationBuilder.DropTable(
                name: "exvs2ob_triad_wanted_defeated_count");

            migrationBuilder.DropTable(
                name: "exvs2ob_upload_image");

            migrationBuilder.DropTable(
                name: "exvs2ob_upload_replay");

            migrationBuilder.DropTable(
                name: "exvs2ob_user_class_match_title");

            migrationBuilder.DropTable(
                name: "exvs2ob_user_default_title");

            migrationBuilder.DropTable(
                name: "exvs2ob_user_triad_title");

            migrationBuilder.DropTable(
                name: "exvs2ob_battle_history");

            migrationBuilder.DropTable(
                name: "exvs2ob_favourite_ms");

            migrationBuilder.DropTable(
                name: "exvs2ob_user_private_match_room_setting");

            migrationBuilder.DropTable(
                name: "exvs2ob_message_setting");

            migrationBuilder.DropTable(
                name: "exvs2ob_user_title_setting");

            migrationBuilder.DropTable(
                name: "exvs2ob_card_profile");
        }
    }
}
