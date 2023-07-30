using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class TitleMapper
{
    [MapProperty(nameof(TitleCustomize.TitleTextId), nameof(Title.TextId))]
    [MapProperty(nameof(TitleCustomize.TitleEffectId), nameof(Title.EffectId))]
    [MapProperty(nameof(TitleCustomize.TitleOrnamentId), nameof(Title.OrnamentId))]
    [MapProperty(nameof(TitleCustomize.TitleBackgroundPartsId), nameof(Title.BackgroundPartsId))]
    public static partial Title ToTitle(this TitleCustomize titleCustomize);
}