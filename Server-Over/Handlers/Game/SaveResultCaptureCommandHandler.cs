using MediatR;
using nue.protocol.exvs;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SaveResultCaptureCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class SaveResultCaptureCommandHandler : IRequestHandler<SaveResultCaptureCommand, Response>
{
    private readonly ServerDbContext _context;

    public SaveResultCaptureCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<Response> Handle(SaveResultCaptureCommand request, CancellationToken cancellationToken)
    {
        var saveRequest = request.Request.save_result_capture;
        var userId = saveRequest.PilotId;
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == saveRequest.SessionId && x.Id == userId);
        
        if (cardProfile == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.ErrServer,
                save_result_capture = new Response.SaveResultCapture()
            });
        }
        
        cardProfile.UploadToken = Guid.NewGuid().ToString("n").Substring(0, 16);
        cardProfile.UploadTokenExpiry = DateTime.Now.AddMinutes(2);
        
        var saveResultCapture = new Response.SaveResultCapture();
        saveResultCapture.Urls.Add($"http://{request.BaseAddress}/upload/uploadImage/{cardProfile.Id}/{cardProfile.UploadToken}");
        
        _context.SaveChanges();
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_result_capture = saveResultCapture
        });
    }
}