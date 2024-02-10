using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerVanilla.Models.Cards.Common;

namespace ServerVanilla.Mapper.Card.Common;

[Mapper]
public static partial class EchelonSettingMapper
{
    public static partial Response.LoadGameData.EchelonTable ToEchelonTable(this EchelonSetting echelonSetting);
}