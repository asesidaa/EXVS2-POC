using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class CreateViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create view exvs2ob_solo_pilot_percentile_view as
                WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                           FROM exvs2ob_battle_solo_class_record
                           WHERE ClassId = 1)
                SELECT RatePercentile, MAX(rate) as RatePoint
                FROM p
                GROUP BY RatePercentile;");
            
            migrationBuilder.Sql(@"
                create view exvs2ob_solo_valiant_percentile_view as
                WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                           FROM exvs2ob_battle_solo_class_record
                           WHERE ClassId = 2)
                SELECT RatePercentile, MAX(rate) as RatePoint
                FROM p
                GROUP BY RatePercentile;");
            
            migrationBuilder.Sql(@"
                create view exvs2ob_solo_ace_percentile_view as
                WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                           FROM exvs2ob_battle_solo_class_record
                           WHERE ClassId = 3)
                SELECT RatePercentile, MAX(rate) as RatePoint
                FROM p
                GROUP BY RatePercentile;");
            
            migrationBuilder.Sql(@"
                create view exvs2ob_team_pilot_percentile_view as
                WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                           FROM exvs2ob_battle_team_class_record
                           WHERE ClassId = 1)
                SELECT RatePercentile, MAX(rate) as RatePoint
                FROM p
                GROUP BY RatePercentile;");
            
            migrationBuilder.Sql(@"
                create view exvs2ob_team_valiant_percentile_view as
                WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                           FROM exvs2ob_battle_team_class_record
                           WHERE ClassId = 2)
                SELECT RatePercentile, MAX(rate) as RatePoint
                FROM p
                GROUP BY RatePercentile;");
            
            migrationBuilder.Sql(@"
                create view exvs2ob_team_ace_percentile_view as
                WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                           FROM exvs2ob_battle_team_class_record
                           WHERE ClassId = 3)
                SELECT RatePercentile, MAX(rate) as RatePoint
                FROM p
                GROUP BY RatePercentile;");
            
			migrationBuilder.Sql(@"
                create view exvs2ob_solo_over_percentile_view as
                WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                           FROM exvs2ob_battle_solo_class_record
                           WHERE ClassId = 4)
                SELECT RatePercentile, MAX(rate) as RatePoint
                FROM p
                GROUP BY RatePercentile;");
            
			migrationBuilder.Sql(@"
				create view exvs2ob_team_over_percentile_view as
				WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
						   FROM exvs2ob_battle_team_class_record
						   WHERE ClassId = 4)
				SELECT RatePercentile, MAX(rate) as RatePoint
				FROM p
				GROUP BY RatePercentile;");
		
			migrationBuilder.Sql(@"
				create view exvs2ob_solo_over_rank_view as
				select DENSE_RANK() OVER (ORDER BY ClassId DESC, Rate DESC)
				as Rank, CardId, Rate
				from exvs2ob_battle_solo_class_record where ClassId = 4;");
		
			migrationBuilder.Sql(@"
				create view exvs2ob_team_over_rank_view as
				select DENSE_RANK() OVER (ORDER BY ClassId DESC, Rate DESC)
				as Rank, CardId, Rate
				from exvs2ob_battle_team_class_record where ClassId = 4;");
			
			migrationBuilder.Sql(@"
                create view exvs2ob_triad_target_defeated_count_view as
                select DENSE_RANK() OVER (ORDER BY DestroyCount desc)
                           as Rank, CardId, DestroyCount, Year, Month
                from exvs2ob_triad_target_defeated_count
                where Year = strftime('%Y', CURRENT_TIMESTAMP)
                and Month = ltrim(strftime('%m', CURRENT_TIMESTAMP), '0');");
            
            migrationBuilder.Sql(@"
                create view exvs2ob_triad_wanted_defeated_count_view as
                select DENSE_RANK() OVER (ORDER BY DestroyCount desc)
                as Rank, CardId, DestroyCount, Year, Month
                from exvs2ob_triad_wanted_defeated_count
                where Year = strftime('%Y', CURRENT_TIMESTAMP)
                and Month = ltrim(strftime('%m', CURRENT_TIMESTAMP), '0');");
            
            migrationBuilder.Sql(@"
                create view exvs2ob_triad_high_score_view as
                select DENSE_RANK() OVER (ORDER BY CourseHighScore desc)
                as Rank, CardId, CourseHighScore, Year, Month
                from exvs2ob_triad_high_score
                where Year = strftime('%Y', CURRENT_TIMESTAMP)
                and Month = ltrim(strftime('%m', CURRENT_TIMESTAMP), '0');");
            
            migrationBuilder.Sql(@"
                create view exvs2ob_triad_clear_time_view as
                select DENSE_RANK() OVER (ORDER BY CourseClearTime)
                as Rank, CardId, CourseClearTime, Year, Month
                from exvs2ob_triad_clear_time 
                where Year = strftime('%Y', CURRENT_TIMESTAMP)
                and Month = ltrim(strftime('%m', CURRENT_TIMESTAMP), '0');");
				
			migrationBuilder.Sql(@"
	            create view exvs2ob_player_level_rank_view as
	            select DENSE_RANK() OVER (ORDER BY PrestigeId desc, PlayerLevelId desc, PlayerExp desc)
	            as Rank, CardProfile.UserName, 
	                     PlayerLevel.PrestigeId, PlayerLevel.PlayerLevelId, PlayerLevel.PlayerExp, 
	                     TotalWin, TotalLose
	            from exvs2ob_battle_player_level PlayerLevel
	            join exvs2ob_card_profile CardProfile
	            on PlayerLevel.CardId = CardProfile.Id
	            join exvs2ob_battle_win_loss_record WinLossRecord
	            on PlayerLevel.CardId = WinLossRecord.CardId;");
			
			migrationBuilder.Sql(@"
	            create view exvs2ob_mobile_suit_usage_view as
	            select MstMobileSuitId, SUM(TotalBattleCount) as AggregatedTotalBattle, SUM(TotalWinCount) as AggregatedTotalWin
	            from exvs2ob_ms_pvp_stat
	            group by MstMobileSuitId
	            order by MstMobileSuitId;");
			
			migrationBuilder.Sql(@"
	            create view exvs2ob_burst_usage_view as
	            select BurstType, SUM(TotalBattle) as AggregatedTotalBattle, SUM(TotalWin) as AggregatedTotalWin
	            from exvs2ob_player_burst_statistic
	            group by BurstType
	            order by BurstType;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop view exvs2ob_solo_pilot_percentile_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_solo_valiant_percentile_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_solo_ace_percentile_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_team_pilot_percentile_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_team_valiant_percentile_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_team_ace_percentile_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_solo_over_percentile_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_team_over_percentile_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_solo_over_rank_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_team_over_rank_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_triad_target_defeated_count_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_triad_wanted_defeated_count_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_triad_high_score_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_triad_clear_time_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_player_level_rank_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_mobile_suit_usage_view;");
            migrationBuilder.Sql(@"drop view exvs2ob_burst_usage_view;");
        }
    }
}
