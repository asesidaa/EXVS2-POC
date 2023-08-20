using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class CpuSceneMapper
{
    public static partial TriadCourseResult ToTriadCourseResult(this Response.LoadCard.PilotDataGroup.CpuSceneData cpuSceneData);
}