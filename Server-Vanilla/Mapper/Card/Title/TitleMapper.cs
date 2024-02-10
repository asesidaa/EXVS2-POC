using Riok.Mapperly.Abstractions;
using ServerVanilla.Models.Cards.Title;

namespace ServerVanilla.Mapper.Card.Title;

[Mapper]
public static partial class TitleMapper
{
    [MapProperty(nameof(DefaultTitle.TitleTextId), nameof(WebUIVanilla.Shared.Dto.Common.Title.TextId))]
    [MapProperty(nameof(DefaultTitle.TitleEffectId), nameof(WebUIVanilla.Shared.Dto.Common.Title.EffectId))]
    [MapProperty(nameof(DefaultTitle.TitleOrnamentId), nameof(WebUIVanilla.Shared.Dto.Common.Title.OrnamentId))]
    [MapProperty(nameof(DefaultTitle.TitleBackgroundPartsId), nameof(WebUIVanilla.Shared.Dto.Common.Title.BackgroundPartsId))]
    public static partial WebUIVanilla.Shared.Dto.Common.Title ToTitle(this DefaultTitle titleCustomize);
}