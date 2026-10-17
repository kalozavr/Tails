using CSharpFunctionalExtensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tails.Domain.Shared;

public class Requisite : ValueObject
{
    public string Name { get; }
    public string Description { get; }

    // EF Core
    private Requisite() { }

    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Result<Requisite, Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new Error("requisite.name.empty", "Requisite name cannot be empty");

        if (string.IsNullOrWhiteSpace(description))
            return new Error("requisite.description.empty", "Requisite description cannot be empty");

        return new Requisite(name.Trim(), description.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}