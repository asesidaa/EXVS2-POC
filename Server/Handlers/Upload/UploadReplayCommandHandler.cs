using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Upload;

public record UploadReplayCommand(string PlayerId, string ReplayTime, HttpRequest HttpRequest) : IRequest<string>;

public class UploadReplayCommandHandler : IRequestHandler<UploadReplayCommand, string>
{
    private readonly ILogger<UploadReplayCommandHandler> _logger;
    private readonly ServerDbContext _context;
    private readonly IConfiguration _config;

    public UploadReplayCommandHandler(ILogger<UploadReplayCommandHandler> logger, ServerDbContext context, IConfiguration config)
    {
        _logger = logger;
        _context = context;
        _config = config;
    }
    
    public async Task<string> Handle(UploadReplayCommand request, CancellationToken cancellationToken)
    {
        var fileName = "0_" + request.ReplayTime + ".json";
        
        if (request.PlayerId == "0")
        {
            if (request.ReplayTime == "0")
            {
                return await Task.FromResult("Done");   
            }
            
            _logger.LogInformation("Seemingly an auto upload Replay from LM, keep processing...");
            await SaveUploadedReplay(request, fileName);
            
            return await Task.FromResult("Done");
        }
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.UploadImages)
            .FirstOrDefault(x => x.Id.ToString() == request.PlayerId);
        
        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        fileName = request.PlayerId + "_" + request.ReplayTime + ".json";
        
        await SaveUploadedReplay(request, fileName);

        return await Task.FromResult("Done");
    }

    private async Task SaveUploadedReplay(UploadReplayCommand request, string fileName)
    {
        using var ms = new MemoryStream(2048);
        await request.HttpRequest.Body.CopyToAsync(ms);
        var byteArray = ms.ToArray();

        var targetPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/replay/" + fileName);
        var folderPath = Path.GetDirectoryName(targetPath) ??
                         throw new InvalidOperationException("Destination Folder is invalid");
        Directory.CreateDirectory(folderPath);

        await using StreamWriter outputFile = new StreamWriter(targetPath);
        outputFile.BaseStream.Write(byteArray, 0, byteArray.Length);
    }
}