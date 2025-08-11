using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Titles.User;

namespace ServerOver.Mapper.Card.Titles.User;

[Mapper]
public static partial class UserTitleMapper
{
    public static partial TitleCustomize ToTitleCustomize(this UserTitle msTitle);
}