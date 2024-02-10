using MediatR;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Upload;

public record UploadReplayCommand(string PlayerId, string ReplayTime, HttpRequest HttpRequest) : IRequest<string>;

public class UploadReplayCommandHandler : IRequestHandler<UploadReplayCommand, string>
{
    private readonly ILogger<UploadReplayCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public UploadReplayCommandHandler(ILogger<UploadReplayCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<string> Handle(UploadReplayCommand request, CancellationToken cancellationToken)
    {
        var fileName = "0_" + request.ReplayTime + ".json";
        
        if (request.ReplayTime == "0")
        {
            return await Task.FromResult("Done");   
        }
            
        _logger.LogInformation("Seemingly an auto upload Replay from LM, keep processing...");
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