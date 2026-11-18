using FluentValidation.Results;
using Tails.Domain.Shared;

namespace Tails.API.Validation;

public static class ValidationExtensions
{
    public static IReadOnlyList<Error> ToErrors(this ValidationResult validationResult) =>
        validationResult.Errors
            .Select(failure => Error.TryDeserialize(failure.ErrorMessage, out var error)
                ? error with { InvalidField = error.InvalidField ?? failure.PropertyName }
                : Error.Validation("validation.failed", failure.ErrorMessage, failure.PropertyName))
            .ToList();
}