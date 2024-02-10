using System.Linq.Expressions;
using WebUIVanilla.Client.Pages;

namespace WebUIVanilla.Client.Services;

public interface ISelectorService
{
    public Expression<Func<CustomizeCard.NaviWithNavigatorGroup, string>> GetNaviSeriesSelector();
    public Expression<Func<CustomizeCard.NaviWithNavigatorGroup, string>> GetNaviSeiyuuSelector();
    public Expression<Func<CustomizeCard.MobileSuitWithSkillGroup, string>> GetMsPilotSelector();
}