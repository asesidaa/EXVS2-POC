using MediatR;
using nue.protocol.exvs;
using ServerVanilla.Command.LoadGameData;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Game;

public record LoadGameDataQuery(Request Request) : IRequest<Response>;

public class LoadGameDataQueryHandler : IRequestHandler<LoadGameDataQuery, Response>
{
    private readonly ServerDbContext _context;

    public LoadGameDataQueryHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<Response> Handle(LoadGameDataQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        
        var loadGameData = new Response.LoadGameData();
        
        List<ILoadGameDataFiller> loadGameDataCommands = new(){
            new MobileSuitFiller(),
            new NaviFiller(),
            new ReleaseGameRulesFiller(),
            new EchelonTableFiller(_context),
            new EchelonMatchingTableSFiller(),
            new EchelonMatchingTableTFiller(),
            new SoloVsAdjustFiller(),
            new TeamVsAdjustFiller(),
            new RuleFiller(),
            new ReplayFiller(),
            new TriadSettingFiller(),
            new ExMatchingTableFiller()
        };
        
        loadGameDataCommands.ForEach(command => command.Fill(loadGameData));
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            load_game_data = loadGameData
        };
        
        return Task.FromResult(response);
    }
}