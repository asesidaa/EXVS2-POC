using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Mappers;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.UploadedContent;

public record GetUploadedImagesCommand(string AccessCode, string ChipId) : IRequest<List<UploadedImage>>;

public class GetUploadedImagesCommandHandler : IRequestHandler<GetUploadedImagesCommand, List<UploadedImage>>
{
    private readonly ServerDbContext context;
    
    public GetUploadedImagesCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<List<UploadedImage>> Handle(GetUploadedImagesCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.UploadImages)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var uploadedImages = cardProfile.UploadImages
            .Select(uploadImage => uploadImage.ToUploadedImage())
            .ToList();

        return Task.FromResult(uploadedImages);
    }
}