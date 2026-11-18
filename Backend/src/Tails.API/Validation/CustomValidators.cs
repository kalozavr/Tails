using FluentValidation;
using Tails.Domain.Shared;

namespace Tails.API.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        Error error) =>
        rule.WithMessage(error.Serialize());

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        string code,
        string message,
        string? invalidField = null) =>
        rule.WithError(Error.Validation(code, message, invalidField));
}