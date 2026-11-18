using CSharpFunctionalExtensions;

namespace Tails.Domain.Shared;

public class PhoneNumber : ValueObject
{
    public const int MinDigits = 7;
    public const int MaxDigits = 15;
    public const int MaxLength = 16; // digits plus an optional leading '+'

    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new Error("phone.empty", "Phone number cannot be empty");

        var normalized = value.Replace(" ", "").Replace("-", "");
        var digits = normalized.StartsWith('+') ? normalized[1..] : normalized;

        if (digits.Length < MinDigits || digits.Length > MaxDigits || !digits.All(char.IsDigit))
            return new Error("phone.invalid", "Phone number format is invalid");

        return new PhoneNumber(normalized);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}