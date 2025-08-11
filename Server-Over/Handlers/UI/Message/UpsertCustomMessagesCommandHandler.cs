using MediatR;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Message;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Message;

public record UpsertCustomMessagesCommand(UpsertCustomMessagesRequest Request) : IRequest<BasicResponse>;

public class UpsertCustomMessagesCommandHandler : IRequestHandler<UpsertCustomMessagesCommand, BasicResponse>
{
    private readonly ServerDbContext _context;
    
    public UpsertCustomMessagesCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpsertCustomMessagesCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }
        
        var requestMessageSetting = updateRequest.MessageSetting;

        var playerMessageSetting = _context.MessageSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        playerMessageSetting.MessagePosition = (uint) requestMessageSetting.MessagePosition;
        playerMessageSetting.AllowReceiveMessage = requestMessageSetting.AllowReceiveMessage;
        
        var openingMessage = _context.OpeningMessageDbSet
            .First(x => x.MessageSetting == playerMessageSetting);
        
        var playingMessage = _context.PlayingMessageDbSet
            .First(x => x.MessageSetting == playerMessageSetting);
        
        var resultMessage = _context.ResultMessageDbSet
            .First(x => x.MessageSetting == playerMessageSetting);
        
        var onlineShuffleOpeningMessage = _context.OnlineShuffleOpeningMessageDbSet
            .First(x => x.MessageSetting == playerMessageSetting);
        
        var onlineShufflePlayingMessage = _context.OnlineShufflePlayingMessageDbSet
            .First(x => x.MessageSetting == playerMessageSetting);
        
        var onlineShuffleResultMessage = _context.OnlineShuffleResultMessageDbSet
            .First(x => x.MessageSetting == playerMessageSetting);
        
        UpsertCommandMessageGroup(requestMessageSetting.StartGroup, openingMessage);
        UpsertCommandMessageGroup(requestMessageSetting.InBattleGroup, playingMessage);
        UpsertCommandMessageGroup(requestMessageSetting.ResultGroup, resultMessage);
        UpsertCommandMessageGroup(requestMessageSetting.OnlineShuffleStartGroup, onlineShuffleOpeningMessage);
        UpsertCommandMessageGroup(requestMessageSetting.OnlineShuffleInBattleGroup, onlineShufflePlayingMessage);
        UpsertCommandMessageGroup(requestMessageSetting.OnlineShuffleResultGroup, onlineShuffleResultMessage);
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    void UpsertCommandMessageGroup(CustomMessageGroup customMessageGroup, Models.Cards.Message.Message destinationMessage)
    {
        destinationMessage.TopMessageText = customMessageGroup.UpMessage.MessageText;
        destinationMessage.TopUniqueMessageId = customMessageGroup.UpMessage.UniqueMessageId;
        
        destinationMessage.DownMessageText = customMessageGroup.DownMessage.MessageText;
        destinationMessage.DownUniqueMessageId = customMessageGroup.DownMessage.UniqueMessageId;
        
        destinationMessage.LeftMessageText = customMessageGroup.LeftMessage.MessageText;
        destinationMessage.LeftUniqueMessageId = customMessageGroup.LeftMessage.UniqueMessageId;
        
        destinationMessage.RightMessageText = customMessageGroup.RightMessage.MessageText;
        destinationMessage.RightUniqueMessageId = customMessageGroup.RightMessage.UniqueMessageId;
    }
}