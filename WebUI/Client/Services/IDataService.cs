using WebUI.Shared.Dto.Common;

namespace WebUI.Client.Services;

public interface IDataService
{
    public Task InitializeAsync();
    public IReadOnlyList<IdValuePair> GetDisplayOptionsSortedById();
    public IReadOnlyList<IdValuePair> GetEchelonDisplayOptionsSortedById();

    public IReadOnlyList<MobileSuit> GetMobileSuitSortedById();
    public IReadOnlyList<MobileSuit> GetCostumeMobileSuitSortedById();
    
    public MobileSuit? GetMobileSuitById(uint id);

    public IReadOnlyList<IdValuePair> GetBgmSortedById();

    public IdValuePair? GetBgmById(uint id);

    public IReadOnlyList<IdValuePair> GetGaugeSortedById();

    public IdValuePair? GetGaugeById(uint id);

    public IReadOnlyList<Navigator> GetNavigatorSortedById();

    public Navigator? GetNavigatorById(uint id);
    public IReadOnlyList<IdValuePair> GetSortedTriadSkillList();
    public IdValuePair? GetTriadSkill(uint id);
    public IReadOnlyList<IdValuePair> GetSortedTriadTeamBannerList();
    public IdValuePair? GetTriadTeamBanner(uint id);
    public IReadOnlyList<IdValuePair> GetSortedGamepadOptionList();
    public List<TriadCourseConfig> GetTriadStageConfigs();
    public IdValuePair? GetCustomizeCommentSentenceById(uint id);
    public IReadOnlyList<IdValuePair> GetCustomizeCommentSentenceSortedById();
    public IdValuePair? GetCustomizeCommentPhraseById(uint id);
    public IReadOnlyList<IdValuePair> GetCustomizeCommentPhraseSortedById();
}