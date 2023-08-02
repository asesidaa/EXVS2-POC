using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Json;

namespace WebUI.Client.Services;

public interface IDataService
{
    public Task InitializeAsync();

    public IReadOnlyList<MobileSuit> GetMobileSuitSortedById();
    public IReadOnlyList<MobileSuit> GetCostumeMobileSuitSortedById();
    
    public MobileSuit? GetMobileSuitById(uint id);

    public IReadOnlyList<Bgm> GetBgmSortedById();

    public Bgm? GetBgmById(uint id);

    public IReadOnlyList<Gauge> GetGaugeSortedById();

    public Gauge? GetGaugeById(uint id);

    public IReadOnlyList<Navigator> GetNavigatorSortedById();

    public Navigator? GetNavigatorById(uint id);
    public IReadOnlyList<IdValuePair> GetSortedTriadSkillList();
    public IdValuePair? GetTriadSkill(uint id);
    public IReadOnlyList<IdValuePair> GetSortedTriadTeamBannerList();
    public IdValuePair? GetTriadTeamBanner(uint id);
}