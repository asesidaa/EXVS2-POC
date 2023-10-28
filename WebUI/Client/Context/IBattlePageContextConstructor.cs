using WebUI.Shared.Context;

namespace WebUI.Client.Context;

public interface IBattlePageContextConstructor
{
    public Task<BattlePageContext> Construct();
}