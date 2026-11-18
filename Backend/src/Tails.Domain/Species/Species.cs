using CSharpFunctionalExtensions;
using Tails.Domain.Shared;

namespace Tails.Domain.Species;

public class Species : Entity<Guid>
{
    public const int MaxNameLength = 100;

    private readonly List<Breed> _breeds = [];

    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    // EF Core
    private Species() { }

    private Species(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Result<Species, Error> Create(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > MaxNameLength)
            return new Error("species.name.invalid", $"Species name is required (max {MaxNameLength} characters)");

        return new Species(id, name.Trim());
    }

    public UnitResult<Error> AddBreed(Breed breed)
    {
        if (breed is null)
            return new Error("species.breed.null", "Breed cannot be null");

        if (_breeds.Any(b => b.Id == breed.Id ||
                             string.Equals(b.Name, breed.Name, StringComparison.OrdinalIgnoreCase)))
            return new Error("species.breed.duplicate", "This breed was already added");

        _breeds.Add(breed);
        return UnitResult.Success<Error>();
    }
}