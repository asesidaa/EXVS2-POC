using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerVanilla.Models.Cards.Settings;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Mapper.Card.Setting;

[Mapper]
public static partial class GamepadMapper
{
    public static partial Response.LoadCard.MobileUserGroup.GamepadGroup ToGamepadGroup(this GamepadSetting gamepadSetting);
    public static partial GamepadConfig ToGamepadConfig(this GamepadSetting gamepadSetting);
}