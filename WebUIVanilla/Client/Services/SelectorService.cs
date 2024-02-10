using System.Linq.Expressions;
using WebUIVanilla.Client.Pages;

namespace WebUIVanilla.Client.Services;

public class SelectorService : ISelectorService
{
    public Expression<Func<CustomizeCard.NaviWithNavigatorGroup, string>> GetNaviSeriesSelector()
    {
        var lang = Thread.CurrentThread.CurrentCulture.Name;

        if (lang == "en-US")
        {
            return x => x.Navigator.Series;
        }

        if (lang == "ja")
        {
            return x => x.Navigator.SeriesJP;
        }

        return x => x.Navigator.SeriesCN;
    }
    
    public Expression<Func<CustomizeCard.NaviWithNavigatorGroup, string>> GetNaviSeiyuuSelector()
    {
        var lang = Thread.CurrentThread.CurrentCulture.Name;

        if (lang == "en-US")
        {
            return x => x.Navigator.Seiyuu;
        }

        if (lang == "ja")
        {
            return x => x.Navigator.SeiyuuJP;
        }

        return x => x.Navigator.SeiyuuCN;
    }

    public Expression<Func<CustomizeCard.MobileSuitWithSkillGroup, string>> GetMsPilotSelector()
    {
        var lang = Thread.CurrentThread.CurrentCulture.Name;

        if (lang == "en-US")
        {
            return x => x.MobileSuit.Pilot;
        }

        if (lang == "ja")
        {
            return x => x.MobileSuit.PilotJP;
        }

        return x => x.MobileSuit.PilotCN;
    }
}