using nue.protocol.exvs;

namespace Server.Handlers.Game.LoadGameData;

public interface ILoadGameDataCommand
{
    void Fill(Response.LoadGameData loadGameData);
}