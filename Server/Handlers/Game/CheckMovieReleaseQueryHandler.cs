using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record CheckMovieReleaseQuery(Request Request) : IRequest<Response>;

public class CheckMovieReleaseQueryHandler : IRequestHandler<CheckMovieReleaseQuery, Response>
{
    public Task<Response> Handle(CheckMovieReleaseQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            check_movie_release = new Response.CheckMovieRelease()
        });
    }
}
