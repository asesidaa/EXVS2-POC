using WebUI.Shared.Dto.Json;

namespace WebUI.Services;

public interface IDataService
{
    public Task InitializeAsync();

    public IReadOnlyList<MobileSuit> GetMobileSuitSortedById();
    
    public MobileSuit? GetMobileSuitById(uint id);

    public IReadOnlyList<Bgm> GetBgmSortedById();

    public Bgm? GetBgmById(uint id);

    public IReadOnlyList<Gauge> GetGaugeSortedById();

    public Gauge? GetGaugeById(uint id);

    public IReadOnlyList<Navigator> GetNavigatorSortedById();

    public Navigator? GetNavigatorById(uint id);
}