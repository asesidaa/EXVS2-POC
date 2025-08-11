using nue.protocol.exvs;

namespace ServerOver.Commands.LoadGameData;

public interface ILoadGameDataCommand
{
    void Fill(Response.LoadGameData loadGameData);
}