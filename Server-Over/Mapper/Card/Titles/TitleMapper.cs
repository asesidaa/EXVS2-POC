using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Titles;
using ServerOver.Models.Cards.Titles.MobileSuit;
using WebUIOver.Shared.Dto.Common;

namespace ServerOver.Mapper.Card.Titles;

[Mapper]
public static partial class TitleMapper
{
    [MapProperty(nameof(BaseTitle.TitleTextId), nameof(Title.TextId))]
    [MapProperty(nameof(BaseTitle.TitleEffectId), nameof(Title.EffectId))]
    [MapProperty(nameof(BaseTitle.TitleOrnamentId), nameof(Title.OrnamentId))]
    [MapProperty(nameof(BaseTitle.TitleBackgroundPartsId), nameof(Title.BackgroundPartsId))]
    [MapProperty(nameof(BaseTitle.CustomText), nameof(Title.CustomText))]
    public static partial Title ToTitle(this BaseTitle titleCustomize);
    
    [MapProperty(nameof(Title.TextId), nameof(MobileSuitDefaultTitle.TitleTextId))]
    [MapProperty(nameof(Title.EffectId), nameof(MobileSuitDefaultTitle.TitleEffectId))]
    [MapProperty(nameof(Title.OrnamentId), nameof(MobileSuitDefaultTitle.TitleOrnamentId))]
    [MapProperty(nameof(Title.BackgroundPartsId), nameof(MobileSuitDefaultTitle.TitleBackgroundPartsId))]
    [MapProperty(nameof(Title.CustomText), nameof(MobileSuitDefaultTitle.CustomText))]
    public static partial MobileSuitDefaultTitle ToMobileSuitDefaultTitle(this Title title);
    
    [MapProperty(nameof(Title.TextId), nameof(MobileSuitTriadTitle.TitleTextId))]
    [MapProperty(nameof(Title.EffectId), nameof(MobileSuitTriadTitle.TitleEffectId))]
    [MapProperty(nameof(Title.OrnamentId), nameof(MobileSuitTriadTitle.TitleOrnamentId))]
    [MapProperty(nameof(Title.BackgroundPartsId), nameof(MobileSuitTriadTitle.TitleBackgroundPartsId))]
    [MapProperty(nameof(Title.CustomText), nameof(MobileSuitTriadTitle.CustomText))]
    public static partial MobileSuitTriadTitle ToMobileSuitTriadTitle(this Title title);
    
    [MapProperty(nameof(Title.TextId), nameof(MobileSuitClassMatchTitle.TitleTextId))]
    [MapProperty(nameof(Title.EffectId), nameof(MobileSuitClassMatchTitle.TitleEffectId))]
    [MapProperty(nameof(Title.OrnamentId), nameof(MobileSuitClassMatchTitle.TitleOrnamentId))]
    [MapProperty(nameof(Title.BackgroundPartsId), nameof(MobileSuitClassMatchTitle.TitleBackgroundPartsId))]
    [MapProperty(nameof(Title.CustomText), nameof(MobileSuitClassMatchTitle.CustomText))]
    public static partial MobileSuitClassMatchTitle ToMobileSuitClassMatchTitle(this Title title);
}