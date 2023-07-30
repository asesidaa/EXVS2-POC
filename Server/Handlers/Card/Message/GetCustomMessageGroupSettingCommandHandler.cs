using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.Message;

public record GetCustomMessageGroupSettingCommand(string AccessCode, string ChipId) : IRequest<CustomMessageGroupSetting>;

public class GetCustomMessageGroupSettingCommandHandler : IRequestHandler<GetCustomMessageGroupSettingCommand, CustomMessageGroupSetting>
{
    private readonly ServerDbContext context;
    
    public GetCustomMessageGroupSettingCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<CustomMessageGroupSetting> Handle(GetCustomMessageGroupSettingCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var mobileUserGroup = JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain.MobileUserGroupJson);
        
        if (mobileUserGroup is null)
        {
            throw new InvalidCardDataException("Card Data is invalid");
        }
        
        var customMessageGroupSetting = new CustomMessageGroupSetting
        {
            StartGroup = CreateCustomMessageGroup(ToCustomMessages(mobileUserGroup.OpeningMessages)),
            InBattleGroup = CreateCustomMessageGroup(ToCustomMessages(mobileUserGroup.PlayingMessages)),
            ResultGroup = CreateCustomMessageGroup(ToCustomMessages(mobileUserGroup.ResultMessages))
        };
        
        return Task.FromResult(customMessageGroupSetting);
    }

    List<CustomMessage> ToCustomMessages(List<Response.LoadCard.MobileUserGroup.CommandMessageGroup> commandMessageGroups)
    {
        return commandMessageGroups
            .Select(message => message.ToCustomMessage())
            .ToList();
    }
        
    CustomMessageGroup CreateCustomMessageGroup(List<CustomMessage> messageList)
    {
        return new CustomMessageGroup
        {
            UpMessage = CreateDefaultCustomMessage(messageList, Command.Up),
            DownMessage = CreateDefaultCustomMessage(messageList, Command.Down),
            LeftMessage = CreateDefaultCustomMessage(messageList, Command.Left),
            RightMessage = CreateDefaultCustomMessage(messageList, Command.Right)
        };
    }

    CustomMessage CreateDefaultCustomMessage(List<CustomMessage> messageList, Command command)
    {
        return messageList
            .FirstOrDefault(
                customMessage => customMessage.Command == command,
                new CustomMessage
                {
                    Command = command,
                    MessageText = "",
                    UniqueMessageId = 0
                });
    }
}