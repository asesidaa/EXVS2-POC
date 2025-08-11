namespace WebUIOver.Shared.Exception;

[Serializable]
public class InvalidCardDataException : System.Exception
{
    public InvalidCardDataException() { }

    public InvalidCardDataException(string message)
        : base(message) { }

    public InvalidCardDataException(string message, System.Exception inner)
        : base(message, inner) { }
}