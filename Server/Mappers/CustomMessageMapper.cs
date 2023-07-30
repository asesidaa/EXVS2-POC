using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class CustomMessageMapper
{
    public static partial CustomMessage ToCustomMessage(this Response.LoadCard.MobileUserGroup.CommandMessageGroup commandMessageGroup);
}