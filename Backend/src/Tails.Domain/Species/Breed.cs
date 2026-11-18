using CSharpFunctionalExtensions;
using Tails.Domain.Shared;

namespace Tails.Domain.Species;

public class Breed : Entity<Guid>
{
    public const int MaxNameLength = 100;

    public string Name { get; private set; }

    private Breed() { }
    private Breed(Guid id, string name) : base(id) => Name = name;

    public static Result<Breed, Error> Create(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > MaxNameLength)
            return Error.Validation("breed.name.invalid", $"Breed name is required (max {MaxNameLength} characters)");

        return new Breed(id, name.Trim());
    }
}