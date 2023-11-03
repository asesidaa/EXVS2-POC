using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Upload;

public record UploadReplayCommand(string PlayerId, string ReplayTime, HttpRequest HttpRequest) : IRequest<string>;

public class UploadReplayCommandHandler : IRequestHandler<UploadReplayCommand, string>
{
    private readonly ServerDbContext _context;
    private readonly IConfiguration _config;

    public UploadReplayCommandHandler(ServerDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    
    public async Task<string> Handle(UploadReplayCommand request, CancellationToken cancellationToken)
    {
        if (request.PlayerId == "0" && request.ReplayTime == "0")
        {
            return await Task.FromResult("Done");
        }
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.UploadImages)
            .FirstOrDefault(x => x.Id.ToString() == request.PlayerId);
        
        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var fileName = request.PlayerId + "_" + request.ReplayTime + ".json";
        
        using var ms = new MemoryStream(2048);
        await request.HttpRequest.Body.CopyToAsync(ms);
        var byteArray = ms.ToArray();

        var targetPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/replay/" + fileName);
        var folderPath = Path.GetDirectoryName(targetPath) ?? throw new InvalidOperationException("Destination Folder is invalid");
        Directory.CreateDirectory(folderPath);
        
        await using StreamWriter outputFile = new StreamWriter(targetPath);
        outputFile.BaseStream.Write(byteArray, 0, byteArray.Length);
        
        return await Task.FromResult("Done");
    }
}