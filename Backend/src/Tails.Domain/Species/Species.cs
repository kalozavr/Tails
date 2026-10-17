using CSharpFunctionalExtensions;

namespace Tails.Domain.Species;

public class Species : Entity<Guid>
{
    // EF Core
    private Species() { }

    private readonly List<Breed> _breeds = [];

    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public Species(Guid id, string name, IEnumerable<Breed>? breeds = null) : base(id)
    {
        Name = name;
        if (breeds is not null)
        {
            _breeds.AddRange(breeds); 
        }
    }

    public void AddBreed(Breed breed) => _breeds.Add(breed);
}