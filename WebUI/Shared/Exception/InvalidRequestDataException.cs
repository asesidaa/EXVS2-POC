namespace WebUI.Shared.Exception;

[Serializable]
public class InvalidRequestDataException : System.Exception
{
    public InvalidRequestDataException() { }

    public InvalidRequestDataException(string message)
        : base(message) { }

    public InvalidRequestDataException(string message, System.Exception inner)
        : base(message, inner) { }
}