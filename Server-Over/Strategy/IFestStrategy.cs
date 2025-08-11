using nue.protocol.exvs;

namespace ServerOver.Strategy;

public interface IFestStrategy
{
    Response.LoadGameData.XrossFesSetting determine();
}