using nue.protocol.exvs;
using ServerOver.Constants.ClassMatch;
using ServerOver.Persistence;

namespace ServerOver.Commands.ClassMatch.TeamThreshold;

public class TeamAceThresholdFiller : IClassMatchFillerCommand
{
    private readonly ServerDbContext _context;

    public TeamAceThresholdFiller(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(Response.LoadClassMatch loadClassMatchResponse)
    {
        var rateThreshold = new Response.LoadClassMatch.ClassMatchRateThreshold()
        {
            TeamType = ClassTeamType.TeamOnlineClass,
            ClassId = RankType.Ace
        };

        var topPlayer = _context.TeamClassMatchRecordDbSet
            .OrderByDescending(x => x.Rate)
            .FirstOrDefault(x => x.ClassId == RankType.Ace);

        var topRate = RatePosition.DefaultRate;

        if (topPlayer is not null)
        {
            topRate = (uint) Math.Floor(topPlayer.Rate);
        }
        
        // 1st Item: Top
        rateThreshold.RatePositions.Add(new Response.LoadClassMatch.ClassMatchRateThreshold.RatePosition()
        {
            Percentage = 0u,
            Rate = topRate
        });
        
        var percentiles = _context.TeamAcePercentileViews.ToList();

        RatePosition.FillableRatePositions.ToList()
            .ForEach(percentage =>
            {
                var targetPercentile = percentiles.FirstOrDefault(x => x.RatePercentile == percentage);

                var targetRate = 0;

                if (targetPercentile is not null)
                {
                    targetRate = (int) Math.Floor(targetPercentile.RatePoint);
                }
                
                rateThreshold.RatePositions.Add(new Response.LoadClassMatch.ClassMatchRateThreshold.RatePosition()
                {
                    Percentage = percentage,
                    Rate = (uint) targetRate
                });
            });
        
        // Bottom Most
        rateThreshold.RatePositions.Add(new Response.LoadClassMatch.ClassMatchRateThreshold.RatePosition()
        {
            Percentage = 100,
            Rate = 0
        });
        
        loadClassMatchResponse.RateThresholds3s.Add(rateThreshold);
    }
}