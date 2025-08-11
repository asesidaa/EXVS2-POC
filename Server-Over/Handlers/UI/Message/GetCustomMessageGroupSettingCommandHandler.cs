using MediatR;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Enum;
using WebUIOver.Shared.Dto.Message;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Message;

public record GetCustomMessageGroupSettingCommand(string AccessCode, string ChipId) : IRequest<CustomMessageGroupSetting>;

public class GetCustomMessageGroupSettingCommandHandler : IRequestHandler<GetCustomMessageGroupSettingCommand, CustomMessageGroupSetting>
{
    private readonly ServerDbContext _context;
    
    public GetCustomMessageGroupSettingCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<CustomMessageGroupSetting> Handle(GetCustomMessageGroupSettingCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var messageSetting = _context.MessageSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        var openingMessage = _context.OpeningMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        
        var playingMessage = _context.PlayingMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        
        var resultMessage = _context.ResultMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        
        var onlineShuffleOpeningMessage = _context.OnlineShuffleOpeningMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        
        var onlineShufflePlayingMessage = _context.OnlineShufflePlayingMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        
        var onlineShuffleResultMessage = _context.OnlineShuffleResultMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        
        var customMessageGroupSetting = new CustomMessageGroupSetting
        {
            MessagePosition = (MessagePosition) messageSetting.MessagePosition,
            AllowReceiveMessage = messageSetting.AllowReceiveMessage,
            StartGroup = CreateCustomMessageGroup(openingMessage, 
                Command.StartUp, Command.StartDown, 
                Command.StartLeft, Command.StartRight),
            InBattleGroup = CreateCustomMessageGroup(playingMessage, 
                Command.Up, Command.Down, 
                Command.Left, Command.Right),
            ResultGroup = CreateCustomMessageGroup(resultMessage, 
                Command.ResultUp, Command.ResultDown, 
                Command.ResultLeft, Command.ResultRight),
            OnlineShuffleStartGroup = CreateCustomMessageGroup(onlineShuffleOpeningMessage, 
                Command.StartUp, Command.StartDown, 
                Command.StartLeft, Command.StartRight),
            OnlineShuffleInBattleGroup = CreateCustomMessageGroup(onlineShufflePlayingMessage, 
                Command.Up, Command.Down, 
                Command.Left, Command.Right),
            OnlineShuffleResultGroup = CreateCustomMessageGroup(onlineShuffleResultMessage, 
                Command.ResultUp, Command.ResultDown, 
                Command.ResultLeft, Command.ResultRight)
        };
        
        return Task.FromResult(customMessageGroupSetting);
    }
    
    CustomMessageGroup CreateCustomMessageGroup(Models.Cards.Message.Message message, 
        Command upDirection,
        Command downDirection,
        Command leftDirection,
        Command rightDirection)
    {
        return new CustomMessageGroup
        {
            UpMessage = CreateDefaultCustomMessage(message.TopMessageText, message.TopUniqueMessageId, upDirection),
            DownMessage = CreateDefaultCustomMessage(message.DownMessageText, message.DownUniqueMessageId, downDirection),
            LeftMessage = CreateDefaultCustomMessage(message.LeftMessageText, message.LeftUniqueMessageId, leftDirection),
            RightMessage = CreateDefaultCustomMessage(message.RightMessageText, message.RightUniqueMessageId, rightDirection)
        };
    }
    
    CustomMessage CreateDefaultCustomMessage(string messageText, uint uniqueMessageId, Command command)
    {
        return new CustomMessage
        {
            Command = command,
            MessageText = messageText,
            UniqueMessageId = uniqueMessageId
        };
    }
}