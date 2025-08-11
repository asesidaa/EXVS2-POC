using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Client.Services.Mastery;
using WebUIOver.Client.Services.Navi;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Group;
using WebUIOVer.Shared.Dto.Response;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class NaviProfileFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;
    private INaviDataService _naviDataService;
    private IFamiliarityDataService _familiarityDataService;

    public NaviProfileFiller(HttpClient httpClient, INaviDataService naviDataService, IFamiliarityDataService familiarityDataService)
    {
        _httpClient = httpClient;
        _naviDataService = naviDataService;
        _familiarityDataService = familiarityDataService;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var naviResult = await _httpClient.GetFromJsonAsync<NaviProfile>($"/ui/navi/getNaviProfile/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        naviResult.ThrowIfNull();

        customizeCardContext.NaviProfile = naviResult;
        
        var naviList = _naviDataService.GetNavigatorSortedById().Select(navigator => new NaviWithNavigatorGroup()
        {
            Navigator = navigator
        }).ToList();
        
        FillNaviWithMastery(naviResult);

        var naviWithAltCostumes = naviList
            .Where(x => x.Navigator.Costumes != null && x.Navigator.Costumes.Count > 0)
            .GroupJoin(
                naviResult.UserNavis,
                firstItem => firstItem.Navigator.Id,
                secondItem => secondItem.Id,
                (firstItem, matchingSecondItems) => new
                {
                    FirstItem = firstItem,
                    MatchingSecondItem = matchingSecondItems.FirstOrDefault()
                })
            .Select(joinedItem =>
            {
                if (joinedItem.MatchingSecondItem != null)
                {
                    joinedItem.FirstItem.Navi = joinedItem.MatchingSecondItem;
                }
                else
                {
                    var naviId = joinedItem.FirstItem.Navigator.Id;

                    joinedItem.FirstItem.Navi = new Navi()
                    {
                        Id = naviId,
                        CostumeId = 0,
                        Familiarity = 0
                    };
                }
                
                return joinedItem.FirstItem;
            })
            .ToList();
        
        customizeCardContext.NaviObservableCollection = new ObservableCollection<NaviWithNavigatorGroup>(naviWithAltCostumes);
    }

    private void FillNaviWithMastery(NaviProfile naviResult)
    {
        _naviDataService.GetWritableNavigatorSortedById()
            .ForEach(writableNavi =>
            {
                if (writableNavi.Id == 0)
                {
                    writableNavi.ClosenessPoint = -1;
                    return;
                }

                var naviData = naviResult.UserNavis
                    .FirstOrDefault(userNavi => userNavi.Id == writableNavi.Id);

                if (naviData is null)
                {
                    writableNavi.ClosenessPoint = 0;
                    writableNavi.FamiliarityDomain = _familiarityDataService.GetNaviFamiliaritySortedById().First();
                    return;
                }

                writableNavi.ClosenessPoint = (int)naviData.Familiarity;
                writableNavi.FamiliarityDomain = _familiarityDataService.GetNaviFamiliaritySortedById()
                    .Reverse()
                    .First(naviFamiliarity => naviData.Familiarity >= naviFamiliarity.MinimumPoint);
            });
    }
}