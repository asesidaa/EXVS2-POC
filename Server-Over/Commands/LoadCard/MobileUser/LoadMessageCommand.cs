using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Message;
using ServerOver.Models.Cards.Settings;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class LoadMessageCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;

    public LoadMessageCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var messageSetting = _context.MessageSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.MuteFlag = messageSetting.AllowReceiveMessage;
        mobileUserGroup.MessagePosition = (CommandMessagePostion)messageSetting.MessagePosition;
        AppendMessages(mobileUserGroup, messageSetting);
    }
    
    void AppendMessages(Response.LoadCard.MobileUserGroup mobileUserGroup, MessageSetting messageSetting)
    {
        var openingMessage = _context.OpeningMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        AppendOpeningMessage(mobileUserGroup.OpeningMessages, openingMessage);
        
        var playingMessage = _context.PlayingMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        AppendPlayingMessage(mobileUserGroup.PlayingMessages, playingMessage);
        
        var resultMessage = _context.ResultMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        AppendResultMessage(mobileUserGroup.ResultMessages, resultMessage);
        
        var onlineShuffleOpeningMessage = _context.OnlineShuffleOpeningMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        AppendOpeningMessage(mobileUserGroup.OnlineShuffleOpeningMessages, onlineShuffleOpeningMessage);
        
        var onlineShufflePlayingMessage = _context.OnlineShufflePlayingMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        AppendPlayingMessage(mobileUserGroup.OnlineShufflePlayingMessages, onlineShufflePlayingMessage);
        
        var onlineShuffleResultMessage = _context.OnlineShuffleResultMessageDbSet
            .First(x => x.MessageSetting == messageSetting);
        AppendResultMessage(mobileUserGroup.OnlineShuffleResultMessages, onlineShuffleResultMessage);
    }

    void AppendOpeningMessage(List<Response.LoadCard.MobileUserGroup.CommandMessageGroup> messageGroup, Message message)
    {
        messageGroup.AddRange(CreateCommandMessageGroups(
            message,
            WebUIOver.Shared.Dto.Enum.Command.StartUp, WebUIOver.Shared.Dto.Enum.Command.StartDown,
            WebUIOver.Shared.Dto.Enum.Command.StartLeft, WebUIOver.Shared.Dto.Enum.Command.StartRight
        ));
    }
    
    void AppendPlayingMessage(List<Response.LoadCard.MobileUserGroup.CommandMessageGroup> messageGroup, Message message)
    {
        messageGroup.AddRange(CreateCommandMessageGroups(
            message,
            WebUIOver.Shared.Dto.Enum.Command.Up, WebUIOver.Shared.Dto.Enum.Command.Down,
            WebUIOver.Shared.Dto.Enum.Command.Left, WebUIOver.Shared.Dto.Enum.Command.Right
        ));
    }
    
    void AppendResultMessage(List<Response.LoadCard.MobileUserGroup.CommandMessageGroup> messageGroup, Message message)
    {
        messageGroup.AddRange(CreateCommandMessageGroups(
            message,
            WebUIOver.Shared.Dto.Enum.Command.ResultUp, WebUIOver.Shared.Dto.Enum.Command.ResultDown,
            WebUIOver.Shared.Dto.Enum.Command.ResultLeft, WebUIOver.Shared.Dto.Enum.Command.ResultRight
        ));
    }
    
    List<Response.LoadCard.MobileUserGroup.CommandMessageGroup> CreateCommandMessageGroups(Message message, 
        WebUIOver.Shared.Dto.Enum.Command upDirection,
        WebUIOver.Shared.Dto.Enum.Command downDirection,
        WebUIOver.Shared.Dto.Enum.Command leftDirection,
        WebUIOver.Shared.Dto.Enum.Command rightDirection)
    {
        var commandMessageGroups = new List<Response.LoadCard.MobileUserGroup.CommandMessageGroup>();
        
        if (message.TopMessageText != string.Empty || message.TopUniqueMessageId > 0)
        {
            commandMessageGroups.Add(CreateCommandMessageGroup(message.TopMessageText, message.TopUniqueMessageId, upDirection));
        }
        
        if (message.DownMessageText != string.Empty || message.DownUniqueMessageId > 0)
        {
            commandMessageGroups.Add(CreateCommandMessageGroup(message.DownMessageText, message.DownUniqueMessageId, downDirection));
        }
        
        if (message.LeftMessageText != string.Empty || message.LeftUniqueMessageId > 0)
        {
            commandMessageGroups.Add(CreateCommandMessageGroup(message.LeftMessageText, message.LeftUniqueMessageId, leftDirection));
        }
        
        if (message.RightMessageText != string.Empty || message.RightUniqueMessageId > 0)
        {
            commandMessageGroups.Add(CreateCommandMessageGroup(message.RightMessageText, message.RightUniqueMessageId, rightDirection));
        }

        return commandMessageGroups;
    }

    Response.LoadCard.MobileUserGroup.CommandMessageGroup CreateCommandMessageGroup(string message, uint stampId,
        WebUIOver.Shared.Dto.Enum.Command direction)
    {
        return new Response.LoadCard.MobileUserGroup.CommandMessageGroup()
        {
            Command = (uint)direction,
            MessageText = message,
            UniqueMessageId = stampId
        };
    }
}