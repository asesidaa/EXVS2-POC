using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Common;

namespace Server.Mappers;

[Mapper]
public static partial class CommandMessageGroupMapper
{
    public static partial Response.LoadCard.MobileUserGroup.CommandMessageGroup ToCommandMessageGroup(this CustomMessage customMessage);
}