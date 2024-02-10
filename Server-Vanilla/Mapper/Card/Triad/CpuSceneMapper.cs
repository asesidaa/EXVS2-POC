using Riok.Mapperly.Abstractions;
using ServerVanilla.Models.Cards.Triad;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Mapper.Card.Triad;

[Mapper]
public static partial class CpuSceneMapper
{
    public static partial TriadCourseResult ToTriadCourseResult(this TriadCourseData triadCourseData);
}