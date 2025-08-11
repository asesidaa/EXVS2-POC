using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Gamepad;

public interface IGamepadDataService
{
    public Task InitializeAsync();
    public IReadOnlyList<IdValuePair> GetSortedGamepadOptionList();
}