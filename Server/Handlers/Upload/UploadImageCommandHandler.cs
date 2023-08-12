using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Upload;

public record UploadImageCommand(string CardId, string AccessToken, HttpRequest HttpRequest) : IRequest<string>;

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, string>
{
    private readonly ServerDbContext _context;
    private readonly IConfiguration _config;

    public UploadImageCommandHandler(ServerDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    
    public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        if (_config.GetValue<bool>("CardServerConfig:IgnoreUploadSaving"))
        {
            return await Task.FromResult("Done");
        }
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.UploadImages)
            .FirstOrDefault(x => x.Id.ToString() == request.CardId 
                                 && x.UploadToken == request.AccessToken 
                                 && x.UploadTokenExpiry >= DateTime.Now.AddMinutes(-1) && x.UploadTokenExpiry < DateTime.Now.AddMinutes(2));

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var fileName = request.CardId + "_" + request.AccessToken + ".jpg";
        
        cardProfile.UploadImages.Add(new UploadImage
        {
            Filename = fileName
        });
        
        using var ms = new MemoryStream(2048);
        await request.HttpRequest.Body.CopyToAsync(ms);
        var byteArray = ms.ToArray();

        var targetPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploadimage/" + fileName);
        var folderPath = Path.GetDirectoryName(targetPath) ?? throw new InvalidOperationException("Destination Folder is invalid");
        Directory.CreateDirectory(folderPath);
        
        await using StreamWriter outputFile = new StreamWriter(targetPath);
        outputFile.BaseStream.Write(byteArray, 0, byteArray.Length);

        cardProfile.UploadToken = Guid.NewGuid().ToString("n").Substring(0, 16);
        cardProfile.UploadTokenExpiry = DateTime.Now;
        
        _context.SaveChanges();

        return await Task.FromResult("Done");
    }
}