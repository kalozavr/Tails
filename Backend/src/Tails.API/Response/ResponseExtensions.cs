using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Tails.Domain.Shared;

namespace Tails.API.Response;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this Error error) => new[] { error }.ToResponse();

    public static ActionResult ToResponse(this IEnumerable<Error> errors)
    {
        var list = errors.ToList();
        var statusCode = list.Count == 0
            ? StatusCodes.Status500InternalServerError
            : GetStatusCode(list[0].Type);

        return new ObjectResult(Envelope.Failure(list)) { StatusCode = statusCode };
    }

    public static ActionResult ToResponse<T>(this Result<T, Error> result) =>
        result.IsSuccess
            ? new OkObjectResult(Envelope.Ok(result.Value))
            : result.Error.ToResponse();

    private static int GetStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}