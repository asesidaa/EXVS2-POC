using System.Linq.Expressions;
using WebUIOver.Shared.Dto.Group;

namespace WebUIOver.Client.Services.Selector;

public interface ISelectorService
{
    public Expression<Func<NaviWithNavigatorGroup, string>> GetNaviSeriesSelector();
    public Expression<Func<NaviWithNavigatorGroup, string>> GetNaviSeiyuuSelector();
    public Expression<Func<MobileSuitWithSkillGroup, string>> GetMsPilotSelector();
}