using System.Collections.ObjectModel;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Group;
using WebUIOver.Shared.Dto.Response;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class MsAlternativeSkinAggregator : ICustomizeCardContextFiller
{
    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var msWithAltSkins = customizeCardContext.MobileSuitWithSkillGroups
            .Where(x => x.MobileSuit.Skins != null && x.MobileSuit.Skins.Count > 0)
            .GroupJoin(
                customizeCardContext.MsSkillGroups,
                firstItem => firstItem.MobileSuit.Id,
                secondItem => secondItem.MstMobileSuitId,
                (firstItem, matchingSecondItems) => new
                {
                    FirstItem = firstItem,
                    MatchingSecondItem = matchingSecondItems.FirstOrDefault()
                })
            .Select(joinedItem =>
            {
                if (joinedItem.MatchingSecondItem != null)
                {
                    joinedItem.FirstItem.SkillGroup = joinedItem.MatchingSecondItem;
                }
                else
                {
                    var msId = joinedItem.FirstItem.MobileSuit.Id;

                    joinedItem.FirstItem.SkillGroup = new MsSkillGroup()
                    {
                        MstMobileSuitId = msId,
                        MsUsedNum = 0,
                        CostumeId = 0,
                        SkinId = 0,
                        TriadBuddyPoint = 0
                    };
                }
                return joinedItem.FirstItem;
            })
            .ToList();

        customizeCardContext.AlternativeSkinMobileSuitsSkillGroups =
            new ObservableCollection<MobileSuitWithSkillGroup>(msWithAltSkins);
    }
}