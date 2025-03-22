namespace TrackEasy.Shared.Exceptions;

public class TrackEasyException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}