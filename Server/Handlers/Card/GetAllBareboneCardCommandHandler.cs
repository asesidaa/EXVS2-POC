using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Common.Utils;
using Server.Models.Cards;
using Server.Persistence;
using System.ComponentModel;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card;

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
    private readonly ServerDbContext context;

    public GetAllBareboneCardCommandHandler(ILogger<GetAllBareboneCardCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        this.context = context;
    }

    public async Task<PaginatedList<BareboneCardProfile>> Handle(
        GetAllBareboneCardCommand request,
        CancellationToken cancellationToken)
    {
        var cardProfileQuery = context.CardProfiles.Include(x => x.UserDomain).Where(x => !x.IsNewCard);

        if (!string.IsNullOrEmpty(request.SearchKeyword))
        {
            var userName = $"\"PlayerName\":\"{request.SearchKeyword.ToLower()}";

            cardProfileQuery = cardProfileQuery
                .Where(
                    x => x.ChipId.ToLower().Contains(request.SearchKeyword.ToLower()) ||
                        x.UserDomain.UserJson.ToLower().Contains(userName.ToLower()));
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
        return cardProfile =>
        {
            var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(
                cardProfile.UserDomain.UserJson);

            return new BareboneCardProfile
            {
                CardId = cardProfile.Id,
                ChipId = cardProfile.ChipId, 
                UserName = user?.PlayerName ?? string.Empty
            };
        };
    }
}