using MediatR;
using ServerOver.Mapper.Card.History;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle.History;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.History;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.History;

public record GetRecentBattleHistoriesCommand(string AccessCode, string ChipId) : IRequest<List<BattleHistorySummary>>;

public class GetRecentBattleHistoriesCommandHandler : IRequestHandler<GetRecentBattleHistoriesCommand, List<BattleHistorySummary>>
{
    private readonly ServerDbContext _context;
    
    public GetRecentBattleHistoriesCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<List<BattleHistorySummary>> Handle(GetRecentBattleHistoriesCommand request,
        CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var battleHistories = _context.BattleHistoryDbSet
            .Where(x => x.CardId == cardProfile.Id)
            .OrderByDescending(x => (long) x.PlayedAt)
            .Take(100)
            .ToList();

        var battleHistorySummaries = battleHistories
            .Select(battleHistory =>
            {
                var actionItems = _context.BattleActionLogsDbSet
                    .Where(x => x.BattleHistoryId == battleHistory.Id)
                    .OrderBy(x => x.FrameTime)
                    .Select(x => x.ToBattleHistoryActionItem())
                    .ToList();
                
                var battleHistorySummary = new BattleHistorySummary()
                {
                    BattleHistoryId = battleHistory.Id,
                    PlayedAt = battleHistory.PlayedAt,
                    IsWin = battleHistory.IsWin,
                    ElapsedSeconds = battleHistory.ElapsedSeconds,
                    StageId = battleHistory.StageId,
                    Score = battleHistory.Score,
                    BurstType = battleHistory.BurstType,
                    SelfPlayer = CreateSelfPlayer(battleHistory, cardProfile),
                    Teammate = CreateTeammate(battleHistory),
                    Opponents = CreateOpponents(battleHistory),
                    ActionItems = actionItems,
                    MiscStats = battleHistory.ToMiscStats(),
                    DamageStats = battleHistory.ToDamageStats(),
                    BurstStats = battleHistory.ToBurstStats(),
                };

                return battleHistorySummary;
            })
            .ToList();
        
        return Task.FromResult(battleHistorySummaries);
    }

    private BattleHistoryPlayer CreateSelfPlayer(BattleHistory battleHistory, CardProfile cardProfile)
    {
        var selfPlayerRecord = _context.BattleSelfDbSet
            .First(x => x.BattleHistoryId == battleHistory.Id && x.CardId == cardProfile.Id);

        var selfPlayer = selfPlayerRecord.ToBattleHistoryPlayer();
        selfPlayer.ConsecutiveWinCount = battleHistory.ConsecutiveWinCount;
        selfPlayer.BurstType = battleHistory.BurstType;
        selfPlayer.HasCard = true;
        
        var mobileSuit = _context.MobileSuitUsageDbSet
            .FirstOrDefault(x => x.MstMobileSuitId == selfPlayer.MobileSuitId);

        if (mobileSuit is not null)
        {
            selfPlayer.CostumeId = mobileSuit.CostumeId;
        }
        
        return selfPlayer;
    }

    private BattleHistoryPlayer? CreateTeammate(BattleHistory battleHistory)
    {
        var teammate = _context.BattleAllyDbSet
            .FirstOrDefault(x => x.BattleHistoryId == battleHistory.Id);

        if (teammate is null)
        {
            return null;
        }
        
        var teammatePlayer = teammate.ToBattleHistoryPlayer();
        teammatePlayer.HasCard = teammatePlayer.CardId > 0;

        if (teammatePlayer.CardId == 0)
        {
            teammatePlayer.HasCard = false;
            teammatePlayer.ConsecutiveWinCount = 0;
            return teammatePlayer;
        }

        teammatePlayer.HasCard = true;
        var teammateBattleHistory = _context.BattleHistoryDbSet
            .FirstOrDefault(x => x.CardId == teammate.CardId && (long) x.PlayedAt == (long) battleHistory.PlayedAt);
        
        if (teammateBattleHistory is not null)
        {
            teammatePlayer.ConsecutiveWinCount = teammateBattleHistory.ConsecutiveWinCount;
            teammatePlayer.BurstType = teammateBattleHistory.BurstType;
        }
        else
        {
            teammatePlayer.ConsecutiveWinCount = 0;
        }
        
        var mobileSuit = _context.MobileSuitUsageDbSet
            .FirstOrDefault(x => x.MstMobileSuitId == teammatePlayer.MobileSuitId);

        if (mobileSuit is not null)
        {
            teammatePlayer.CostumeId = mobileSuit.CostumeId;
        }
        
        return teammatePlayer;
    }
    
    private List<BattleHistoryPlayer> CreateOpponents(BattleHistory battleHistory)
    {
        var opponents = _context.BattleTargetDbSet
            .Where(x => x.BattleHistoryId == battleHistory.Id)
            .OrderBy(x => x.Id)
            .ToList();

        return opponents.Select(opponent =>
            {
                var opponentPlayer = opponent.ToBattleHistoryPlayer();
                opponentPlayer.HasCard = opponentPlayer.CardId > 0;

                if (opponentPlayer.CardId == 0)
                {
                    opponentPlayer.HasCard = false;
                    opponentPlayer.ConsecutiveWinCount = 0;
                    return opponentPlayer;
                }

                opponentPlayer.HasCard = true;

                var opponentBattleHistory = _context.BattleHistoryDbSet
                    .FirstOrDefault(x => x.CardId == opponent.CardId && (long) x.PlayedAt == (long) battleHistory.PlayedAt);

                if (opponentBattleHistory is not null)
                {
                    opponentPlayer.ConsecutiveWinCount = opponentBattleHistory.ConsecutiveWinCount;
                    opponentPlayer.BurstType = opponentBattleHistory.BurstType;
                }
                else
                {
                    opponentPlayer.ConsecutiveWinCount = 0;
                }
                
                var mobileSuit = _context.MobileSuitUsageDbSet
                    .FirstOrDefault(x => x.MstMobileSuitId == opponentPlayer.MobileSuitId);

                if (mobileSuit is not null)
                {
                    opponentPlayer.CostumeId = mobileSuit.CostumeId;
                }

                return opponentPlayer;
            })
            .ToList();
    }
}