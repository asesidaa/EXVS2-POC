using WebUIOver.Client.Context.CustomizeCard;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public interface ICustomizeCardContextFiller
{
    Task Fill(CustomizeCardContext customizeCardContext);
}