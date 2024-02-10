using nue.protocol.exvs;

namespace ServerVanilla.Mapper.Card.Triad;

using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class TriadCourseDataMapper
{
    public static partial Response.LoadCard.PilotDataGroup.CpuSceneData ToCpuSceneData(this Models.Cards.Triad.TriadCourseData triadCourseData);
}