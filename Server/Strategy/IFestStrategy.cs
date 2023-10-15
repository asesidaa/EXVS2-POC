using nue.protocol.exvs;

namespace Server.Strategy;

public interface IFestStrategy
{
    Response.LoadGameData.XrossFesSetting determine();
}