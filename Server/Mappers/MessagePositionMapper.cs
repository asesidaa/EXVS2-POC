using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using WebUI.Shared.Dto.Enum;

namespace Server.Mappers;

[Mapper]
public static partial class MessagePositionMapper
{
    public static partial MessagePosition ToMessagePosition(this CommandMessagePostion commandMessagePostion);
    public static partial CommandMessagePostion ToCommandMessagePostion(this MessagePosition messagePosition);
}