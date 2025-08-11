using nue.protocol.exvs;
using ServerOver.Common.Enum;
using ServerOver.Constants;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Mission;
using ServerOver.Persistence;
using ServerOver.Strategy;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class ChallengeMissionCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;
    private readonly DailyChallengeMissionStrategy _dailyChallengeMissionStrategy = new ();

    public ChallengeMissionCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var currentTime = DateTime.Now;
        var currentYear = (uint) currentTime.Year;
        var currentMonth = (uint) currentTime.Month;
        var currentDay = (uint) currentTime.Day;
        var currentDateOfWeek = currentTime.DayOfWeek;

        var endOfDateOffset = new DateTimeOffset(
            DateTime.Now.Date.AddDays(1).AddTicks(-1)
        );
        var targetCompleteDate = (ulong) endOfDateOffset.ToUnixTimeSeconds();

        var missionChallengeProfile = _context.ChallengeMissionProfileDbSet
            .FirstOrDefault(p => p.CardId == cardProfile.Id);
        
        var missionTypes = _dailyChallengeMissionStrategy.DetermineMissionTypes(currentDateOfWeek);

        if (missionChallengeProfile == null)
        {
            var newMissionChallengeProfile = new ChallengeMissionProfile()
            {
                CardId = cardProfile.Id,
                EffectiveYear = currentYear,
                EffectiveMonth = currentMonth,
                EffectiveDay = currentDay,
                TotalBattleCount = 0,
                TotalBattleWinCount = 0,
                MaxConsecutiveWinCount = 0,
                TotalDefeatCount = 0,
                TotalDamageCount = 0,
                CardProfile = cardProfile
            };
            
            _context.ChallengeMissionProfileDbSet.Add(newMissionChallengeProfile);
            _context.SaveChanges();
            
            missionTypes.ForEach(missionType => { AddNewMission(mobileUserGroup, missionType, targetCompleteDate); });

            return;
        }
        
        if (missionChallengeProfile.EffectiveYear == currentYear
            && missionChallengeProfile.EffectiveMonth == currentMonth
            && missionChallengeProfile.EffectiveDay == currentDay)
        {
            missionTypes.ForEach(missionType =>
            {
                mobileUserGroup.ChallengeMisDatas.Add(
                    GetChallengeMisData(missionType, missionChallengeProfile, targetCompleteDate)
                );
            });

            return;
        }
        
        missionChallengeProfile.EffectiveYear = currentYear;
        missionChallengeProfile.EffectiveMonth = currentMonth;
        missionChallengeProfile.EffectiveDay = currentDay;
        missionChallengeProfile.TotalBattleCount = 0;
        missionChallengeProfile.TotalBattleWinCount = 0;
        missionChallengeProfile.MaxConsecutiveWinCount = 0;
        missionChallengeProfile.TotalDefeatCount = 0;
        missionChallengeProfile.TotalDamageCount = 0;
        _context.SaveChanges();
        
        missionTypes.ForEach(missionType =>
        {
            mobileUserGroup.ChallengeMisDatas.Add(
                GetChallengeMisData(missionType, missionChallengeProfile, targetCompleteDate)
            );
        });
    }

    private void AddNewMission(Response.PreLoadCard.MobileUserGroup mobileUserGroup, MissionType missionType, ulong targetCompleteDate)
    {
        var missionSetting = MissionSettings.GetBy(missionType);

        mobileUserGroup.ChallengeMisDatas.Add(new Response.PreLoadCard.MobileUserGroup.ChallengeMisData()
        {
            MissionId = missionSetting.Id,
            MissionTypeId = missionSetting.Id,
            ShouldCompleteAt = targetCompleteDate,
            CurrentNum = 0,
            DisiredNum = missionSetting.DesiredNum,
            Backcolor = 0,
            Status = 0 
        });
    }

    private Response.PreLoadCard.MobileUserGroup.ChallengeMisData GetChallengeMisData(
        MissionType missionType, ChallengeMissionProfile challengeMissionProfile, ulong targetCompleteDate)
    {
        var missionSetting = MissionSettings.GetBy(missionType);
        
        var challengeMissionData = new Response.PreLoadCard.MobileUserGroup.ChallengeMisData()
        {
            MissionId = missionSetting.Id,
            MissionTypeId = missionSetting.Id,
            ShouldCompleteAt = targetCompleteDate,
            CurrentNum = GetCurrentNum(missionType, challengeMissionProfile),
            DisiredNum = missionSetting.DesiredNum,
            Backcolor = 0
        };

        challengeMissionData.Status = (uint) (challengeMissionData.CurrentNum >= challengeMissionData.DisiredNum ? 1 : 0);

        if (challengeMissionData.CurrentNum >= challengeMissionData.DisiredNum)
        {
            challengeMissionData.CurrentNum = challengeMissionData.DisiredNum;
        }
        
        return challengeMissionData;
    }

    private uint GetCurrentNum(MissionType missionType, ChallengeMissionProfile challengeMissionProfile)
    {
        return missionType switch
        {
            MissionType.TotalBattleCount => challengeMissionProfile.TotalBattleCount,
            MissionType.TotalBattleWinCount => challengeMissionProfile.TotalBattleWinCount,
            MissionType.MaxConsecutiveWinCount => challengeMissionProfile.MaxConsecutiveWinCount,
            MissionType.TotalDefeatCount => challengeMissionProfile.TotalDefeatCount,
            MissionType.TotalDamageCount => challengeMissionProfile.TotalDamageCount,
            _ => 0
        };
    }
}