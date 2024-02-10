using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.Message;

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
            .Include(x => x.OpeningMessage)
            .Include(x => x.PlayingMessage)
            .Include(x => x.ResultMessage)
            .Include(x => x.OnlineOpeningMessage)
            .Include(x => x.OnlinePlayingMessage)
            .Include(x => x.OnlineResultMessage)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var messageSetting = updateRequest.MessageSetting;
        
        UpsertCommandMessageGroup(messageSetting.StartGroup, cardProfile.OpeningMessage);
        UpsertCommandMessageGroup(messageSetting.InBattleGroup, cardProfile.PlayingMessage);
        UpsertCommandMessageGroup(messageSetting.ResultGroup, cardProfile.ResultMessage);
        UpsertCommandMessageGroup(messageSetting.OnlineShuffleStartGroup, cardProfile.OnlineOpeningMessage);
        UpsertCommandMessageGroup(messageSetting.OnlineShuffleInBattleGroup, cardProfile.OnlinePlayingMessage);
        UpsertCommandMessageGroup(messageSetting.OnlineShuffleResultGroup, cardProfile.OnlineResultMessage);
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    void UpsertCommandMessageGroup(CustomMessageGroup? customMessageGroup, Models.Cards.Message.Message destinationMessage)
    {
        if (customMessageGroup is null)
        {
            return;
        }
        
        if (customMessageGroup.UpMessage is not null)
        {
            destinationMessage.TopMessageText = customMessageGroup.UpMessage.MessageText;
            destinationMessage.TopUniqueMessageId = customMessageGroup.UpMessage.UniqueMessageId;
        } 
        
        if (customMessageGroup.DownMessage is not null)
        {
            destinationMessage.DownMessageText = customMessageGroup.DownMessage.MessageText;
            destinationMessage.DownUniqueMessageId = customMessageGroup.DownMessage.UniqueMessageId;
        } 
        
        if (customMessageGroup.LeftMessage is not null)
        {
            destinationMessage.LeftMessageText = customMessageGroup.LeftMessage.MessageText;
            destinationMessage.LeftUniqueMessageId = customMessageGroup.LeftMessage.UniqueMessageId;
        } 
        
        if (customMessageGroup.RightMessage is not null)
        {
            destinationMessage.RightMessageText = customMessageGroup.RightMessage.MessageText;
            destinationMessage.RightUniqueMessageId = customMessageGroup.RightMessage.UniqueMessageId;
        }
    }
}