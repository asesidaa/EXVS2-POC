using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public interface ILoadGameDataFiller
{
    void Fill(Response.LoadGameData loadGameData);
}