using CSharpFunctionalExtensions;
using Tails.Domain.Shared;

namespace Tails.Domain.Pets;

public class SpeciesBreed : ValueObject
{
    public Guid SpeciesId { get; }
    public Guid BreedId { get; }

    // EF Core
    private SpeciesBreed() { }

    private SpeciesBreed(Guid speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public static Result<SpeciesBreed, Error> Create(Guid speciesId, Guid breedId)
    {
        if (speciesId == Guid.Empty)
        {
            return new Error("pet.species.empty", "Species id cannot be empty");
        }

        if (breedId == Guid.Empty)
        {
            return new Error("pet.breed.empty", "Breed id cannot be empty");
        }

        return new SpeciesBreed(speciesId, breedId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SpeciesId;
        yield return BreedId;
    }
}