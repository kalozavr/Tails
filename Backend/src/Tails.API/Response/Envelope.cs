using Tails.Domain.Shared;

namespace Tails.API.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    public object? Result { get; }
    public List<ResponseError> ErrorInfo { get; }
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        ErrorInfo = errors.ToList();
        TimeGenerated = DateTime.UtcNow;
    }

    public static Envelope Ok(object? result = null) => new(result, []);

    public static Envelope Failure(IEnumerable<Error> errors) =>
        new(null, errors.Select(e => new ResponseError(e.Code, e.Message, e.InvalidField)));
}