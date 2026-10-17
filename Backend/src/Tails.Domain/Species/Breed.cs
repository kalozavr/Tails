using CSharpFunctionalExtensions;

namespace Tails.Domain.Species;

public class Breed : Entity<Guid>
{
    // EF Core
    private Breed() { }

    public string Name { get; private set; }

    public Breed(Guid id, string name) : base(id)
    {
        Name = name;
    }
}