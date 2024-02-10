using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Handlers.Card.Message;

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
            .Include(x => x.OpeningMessage)
            .Include(x => x.PlayingMessage)
            .Include(x => x.ResultMessage)
            .Include(x => x.OnlineOpeningMessage)
            .Include(x => x.OnlinePlayingMessage)
            .Include(x => x.OnlineResultMessage)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var customMessageGroupSetting = new CustomMessageGroupSetting
        {
            StartGroup = CreateCustomMessageGroup(cardProfile.OpeningMessage, 
                WebUIVanilla.Shared.Dto.Enum.Command.StartUp, WebUIVanilla.Shared.Dto.Enum.Command.StartDown, 
                WebUIVanilla.Shared.Dto.Enum.Command.StartLeft, WebUIVanilla.Shared.Dto.Enum.Command.StartRight),
            InBattleGroup = CreateCustomMessageGroup(cardProfile.PlayingMessage, 
                WebUIVanilla.Shared.Dto.Enum.Command.Up, WebUIVanilla.Shared.Dto.Enum.Command.Down, 
                WebUIVanilla.Shared.Dto.Enum.Command.Left, WebUIVanilla.Shared.Dto.Enum.Command.Right),
            ResultGroup = CreateCustomMessageGroup(cardProfile.ResultMessage, 
                WebUIVanilla.Shared.Dto.Enum.Command.ResultUp, WebUIVanilla.Shared.Dto.Enum.Command.ResultDown, 
                WebUIVanilla.Shared.Dto.Enum.Command.ResultLeft, WebUIVanilla.Shared.Dto.Enum.Command.ResultRight),
            OnlineShuffleStartGroup = CreateCustomMessageGroup(cardProfile.OnlineOpeningMessage, 
                WebUIVanilla.Shared.Dto.Enum.Command.StartUp, WebUIVanilla.Shared.Dto.Enum.Command.StartDown, 
                WebUIVanilla.Shared.Dto.Enum.Command.StartLeft, WebUIVanilla.Shared.Dto.Enum.Command.StartRight),
            OnlineShuffleInBattleGroup = CreateCustomMessageGroup(cardProfile.OnlinePlayingMessage, 
                WebUIVanilla.Shared.Dto.Enum.Command.Up, WebUIVanilla.Shared.Dto.Enum.Command.Down, 
                WebUIVanilla.Shared.Dto.Enum.Command.Left, WebUIVanilla.Shared.Dto.Enum.Command.Right),
            OnlineShuffleResultGroup = CreateCustomMessageGroup(cardProfile.OnlineResultMessage, 
                WebUIVanilla.Shared.Dto.Enum.Command.ResultUp, WebUIVanilla.Shared.Dto.Enum.Command.ResultDown, 
                WebUIVanilla.Shared.Dto.Enum.Command.ResultLeft, WebUIVanilla.Shared.Dto.Enum.Command.ResultRight)
        };
        
        return Task.FromResult(customMessageGroupSetting);
    }
    
    CustomMessageGroup CreateCustomMessageGroup(Models.Cards.Message.Message message, 
        WebUIVanilla.Shared.Dto.Enum.Command upDirection,
        WebUIVanilla.Shared.Dto.Enum.Command downDirection,
        WebUIVanilla.Shared.Dto.Enum.Command leftDirection,
        WebUIVanilla.Shared.Dto.Enum.Command rightDirection)
    {
        return new CustomMessageGroup
        {
            UpMessage = CreateDefaultCustomMessage(message.TopMessageText, message.TopUniqueMessageId, upDirection),
            DownMessage = CreateDefaultCustomMessage(message.DownMessageText, message.DownUniqueMessageId, downDirection),
            LeftMessage = CreateDefaultCustomMessage(message.LeftMessageText, message.LeftUniqueMessageId, leftDirection),
            RightMessage = CreateDefaultCustomMessage(message.RightMessageText, message.RightUniqueMessageId, rightDirection)
        };
    }
    
    CustomMessage CreateDefaultCustomMessage(string messageText, uint uniqueMessageId, WebUIVanilla.Shared.Dto.Enum.Command command)
    {
        return new CustomMessage
        {
            Command = command,
            MessageText = messageText,
            UniqueMessageId = uniqueMessageId
        };
    }
}