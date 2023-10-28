using WebUI.Shared.Context;

namespace WebUI.Client.Context;

public interface IServerBattlePageContextConstructor
{
    public Task<ServerBattlePageContext> Construct();
}