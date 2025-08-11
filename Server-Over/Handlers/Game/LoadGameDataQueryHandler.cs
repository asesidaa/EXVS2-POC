using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Commands.LoadGameData;
using ServerOver.Models.Config;

namespace ServerOver.Handlers.Game;

public record LoadGameDataQuery(Request Request) : IRequest<Response>;

public class LoadGameDataQueryHandler : IRequestHandler<LoadGameDataQuery, Response>
{
    private readonly CardServerConfig _config;
    
    public LoadGameDataQueryHandler(IOptions<CardServerConfig> options)
    {
        _config = options.Value;
    }
    
    public Task<Response> Handle(LoadGameDataQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        
        var loadGameData = new Response.LoadGameData
        {
            ReleaseCpuScenes = new[] { 1u, 2u, 3u, 4u, 5u, 6u, 7u },
            TrainingTimeLimit = _config.GameConfigurations.TrainingMinutes,
            NewcardCampaignFlag = true,
            BaseWinPoint = 20,
            BaseLosePoint = 20,
            WinBonus = 20,
            WinResultTopBonus = 20,
            LoseResultTopBonus = 20,
            ObPassBonusGp = 100,
            LoadGameDataVer = 8,
        };
        
        var loadGameDataCommands = new List<ILoadGameDataCommand>{
            new ReleasedContentCommand(_config),
            new PlayerLevelSettingCommand(),
            new VersusInfoCommand(),
            new TriadCourseCommand(),
            new TriadWantedDataCommand(_config),
            new FesDataCommand(_config),
            new FreeMatchDataCommand()
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