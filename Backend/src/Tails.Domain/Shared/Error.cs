namespace Tails.Domain.Shared;

public enum ErrorType
{
    Validation,
    NotFound,
    Failure,
    Conflict
}

public record Error(string Code, string Message, ErrorType Type, string? InvalidField = null)
{
    private const string Separator = "||";

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);

    public static Error NotFound(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.NotFound, invalidField);

    public static Error Failure(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Failure, invalidField);

    public static Error Conflict(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Conflict, invalidField);

    // Used to carry an Error through FluentValidation's string message
    public string Serialize() => string.Join(Separator, Code, Message, Type, InvalidField);

    public static bool TryDeserialize(string value, out Error error)
    {
        error = default!;
        var parts = value.Split(Separator);
        if (parts.Length != 4 || !Enum.TryParse<ErrorType>(parts[2], out var type))
            return false;

        error = new Error(parts[0], parts[1], type, string.IsNullOrEmpty(parts[3]) ? null : parts[3]);
        return true;
    }
}