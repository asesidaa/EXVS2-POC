using MediatR;
using ServerOver.Common.Utils;
using ServerOver.Dtos.Request;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Models;

namespace ServerOver.Handlers.UI.Card;

public record GetAllBareboneCardCommand(GetAllBareboneCardRequest Request) : IRequest<PaginatedList<BareboneCardProfile>>;

public class GetAllBareboneCardCommandHandler : IRequestHandler<GetAllBareboneCardCommand, PaginatedList<BareboneCardProfile>>
{
    private readonly ILogger<GetAllBareboneCardCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public GetAllBareboneCardCommandHandler(ILogger<GetAllBareboneCardCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<PaginatedList<BareboneCardProfile>> Handle(GetAllBareboneCardCommand request, CancellationToken cancellationToken)
    {
        var getAllRequest = request.Request;
        var searchKeyword = getAllRequest.SearchKeyword;
        
        var cardProfileQuery = _context.CardProfiles
            .Where(x => !x.IsNewCard);

        if (!string.IsNullOrEmpty(searchKeyword))
        {
            cardProfileQuery = cardProfileQuery
                .Where(
                    x => x.ChipId.ToLower().Contains(searchKeyword.ToLower()) ||
                         x.UserName.ToLower().Contains(searchKeyword.ToLower()));
        }

        var cardProfiles = await cardProfileQuery.ToPaginatedListAsync(
            getAllRequest.Page,
            getAllRequest.PageSize,
            x => x.Id,
            cancellationToken);

        var mappedCards = cardProfiles.Data
            .Select(ToBareboneCardProfile())
            .ToList();

        return new PaginatedList<BareboneCardProfile>(
            mappedCards,
            cardProfiles.TotalCount,
            getAllRequest.Page,
            getAllRequest.PageSize);
    }

    private Func<CardProfile, BareboneCardProfile> ToBareboneCardProfile()
    {
        return cardProfile => new BareboneCardProfile
        {
            CardId = cardProfile.Id,
            ChipId = cardProfile.ChipId,
            UserName = cardProfile.UserName
        };
    }
}