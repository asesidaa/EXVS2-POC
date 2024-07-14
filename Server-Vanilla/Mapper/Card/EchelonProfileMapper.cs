using Riok.Mapperly.Abstractions;
using ServerVanilla.Models.Cards.Profile;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Mapper.Card;

[Mapper]
public static partial class EchelonProfileMapper
{
    [MapProperty(nameof(BattleProfile.SEchelonFlag), nameof(EchelonProfile.SpecialEchelonFlag))]
    [MapProperty(nameof(BattleProfile.SEchelonMissionFlag), nameof(EchelonProfile.AppliedForSpecialEchelonTest))]
    [MapProperty(nameof(BattleProfile.SEchelonProgress), nameof(EchelonProfile.SpecialEchelonTestProgress))]
    public static partial EchelonProfile ToEchelonProfile(this BattleProfile battleProfile);
}