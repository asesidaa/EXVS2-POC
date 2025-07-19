using WebUIOver.Client.Context.ViewCard;

namespace WebUIOver.Client.Command.ViewCard.Filler;

public interface IViewCardContextFiller
{
    Task Fill(ViewCardContext viewCardContext);
}