using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class EchelonProfileMapper
{
    [MapProperty(nameof(Response.PreLoadCard.LoadPlayer.SEchelonFlag), nameof(EchelonProfile.SpecialEchelonFlag))]
    [MapProperty(nameof(Response.PreLoadCard.LoadPlayer.SEchelonMissionFlag), nameof(EchelonProfile.AppliedForSpecialEchelonTest))]
    [MapProperty(nameof(Response.PreLoadCard.LoadPlayer.SEchelonProgress), nameof(EchelonProfile.SpecialEchelonTestProgress))]
    public static partial EchelonProfile ToEchelonProfile(this Response.PreLoadCard.LoadPlayer loadPlayer);
}