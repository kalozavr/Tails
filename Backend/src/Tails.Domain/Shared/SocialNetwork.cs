using CSharpFunctionalExtensions;

namespace Tails.Domain.Shared;

public class SocialNetwork : ValueObject
{
    public string Name { get; }
    public string Link { get; }

    // EF Core
    private SocialNetwork() { }

    private SocialNetwork(string name, string link)
    {
        Name = name;
        Link = link;
    }

    public static Result<SocialNetwork, Error> Create(string name, string link)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new Error("socialnetwork.name.empty", "Social network name cannot be empty");

        if (string.IsNullOrWhiteSpace(link))
            return new Error("socialnetwork.link.empty", "Social network link cannot be empty");

        return new SocialNetwork(name.Trim(), link.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Link;
    }
}