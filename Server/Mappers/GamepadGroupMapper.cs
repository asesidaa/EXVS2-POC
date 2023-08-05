using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class GamepadGroupMapper
{
    public static partial GamepadConfig ToGamePadConfig(this Response.LoadCard.MobileUserGroup.GamepadGroup gamepadGroup);
    public static partial Response.LoadCard.MobileUserGroup.GamepadGroup ToGamepadGroup(this GamepadConfig gamepadConfig);
}