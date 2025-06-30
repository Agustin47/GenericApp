namespace Framework.Common.Result;

public class ErrorCause(string field, string message, string metadata)
{
    public ErrorCause(string field, string message) : this(field, message, string.Empty) { }
    public string Field { get; private set; } = field;
    public string Message { get; private set; } = message;
    public string Metadata { get; private set; } = metadata;
}