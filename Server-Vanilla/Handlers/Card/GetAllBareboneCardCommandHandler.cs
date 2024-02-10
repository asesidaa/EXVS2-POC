using System.ComponentModel;
using MediatR;
using ServerVanilla.Common.Utils;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Response;
using WebUIVanilla.Shared.Models;

namespace ServerVanilla.Handlers.Card;

public record GetAllBareboneCardCommand() : IRequest<PaginatedList<BareboneCardProfile>>
{
    [DefaultValue(1)]
    public int Page { get; init; } = 1;

    [DefaultValue(10)]
    public int PageSize { get; init; } = 10;

    [Description("Like search on userName and chipId fields.")]
    public string? SearchKeyword { get; init; }
}

public class GetAllBareboneCardCommandHandler :
    IRequestHandler<GetAllBareboneCardCommand, PaginatedList<BareboneCardProfile>>
{
    private readonly ILogger<GetAllBareboneCardCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public GetAllBareboneCardCommandHandler(ILogger<GetAllBareboneCardCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<PaginatedList<BareboneCardProfile>> Handle(
        GetAllBareboneCardCommand request,
        CancellationToken cancellationToken)
    {
        var cardProfileQuery = _context.CardProfiles
            .Where(x => !x.IsNewCard);

        if (!string.IsNullOrEmpty(request.SearchKeyword))
        {
            cardProfileQuery = cardProfileQuery
                .Where(
                    x => x.ChipId.ToLower().Contains(request.SearchKeyword.ToLower()) ||
                        x.UserName.ToLower().Contains(request.SearchKeyword.ToLower()));
        }

        var cardProfiles = await cardProfileQuery.ToPaginatedListAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        var mappedCards = cardProfiles.Data.Select(ToBareboneCardProfile()).ToList();

        return new PaginatedList<BareboneCardProfile>(
            mappedCards,
            cardProfiles.TotalCount,
            request.Page,
            request.PageSize);
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