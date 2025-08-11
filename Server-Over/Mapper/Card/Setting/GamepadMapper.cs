using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Settings;
using WebUIOver.Shared.Dto.Common;

namespace ServerOver.Mapper.Card.Setting;

[Mapper]
public static partial class GamepadMapper
{
    public static partial Response.LoadCard.MobileUserGroup.GamepadGroup ToGamepadGroup(this GamepadSetting gamepadSetting);
    public static partial GamepadConfig ToGamepadConfig(this GamepadSetting gamepadSetting);
}