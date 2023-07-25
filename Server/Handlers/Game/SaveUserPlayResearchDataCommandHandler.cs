using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveUserPlayResearchDataCommand(Request Request) : IRequest<Response>;

public class SaveUserPlayResearchDataCommandHandler : IRequestHandler<SaveUserPlayResearchDataCommand, Response>
{
    public Task<Response> Handle(SaveUserPlayResearchDataCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_user_play_research_data = new Response.SaveUserPlayResearchData()
        });
    }
}
