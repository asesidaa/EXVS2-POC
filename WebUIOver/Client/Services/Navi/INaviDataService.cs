using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Navi;

public interface INaviDataService
{
    public Task InitializeAsync();
    public IReadOnlyList<Navigator> GetNavigatorSortedById();
    public List<Navigator> GetWritableNavigatorSortedById();
    public Navigator? GetNavigatorById(uint id);
}