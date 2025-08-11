using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Triad;
using WebUIOver.Shared.Dto.Triad;

namespace ServerOver.Mapper.Card.Triad;

[Mapper]
public static partial class CpuSceneMapper
{
    public static partial TriadCourseResult ToTriadCourseResult(this TriadCourseData triadCourseData);
}