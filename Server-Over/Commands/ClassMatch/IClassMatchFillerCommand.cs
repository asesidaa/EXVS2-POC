using nue.protocol.exvs;

namespace ServerOver.Commands.ClassMatch;

public interface IClassMatchFillerCommand
{
    void Fill(Response.LoadClassMatch loadClassMatchResponse);
}