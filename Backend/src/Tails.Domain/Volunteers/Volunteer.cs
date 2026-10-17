using CSharpFunctionalExtensions;
using Tails.Domain.Pets;
using Tails.Domain.Shared;

namespace Tails.Domain.Volunteers;

public class Volunteer : Entity<Guid>
{
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<Requisite> _requisites = [];
    private readonly List<Pet> _pets = [];

    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int ExperienceYears { get; private set; }

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<Pet> Pets => _pets;

    private Volunteer(Guid id, string fullName, string email, string phoneNumber, int experienceYears)
        : base(id)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        ExperienceYears = experienceYears;
    }

    public static Result<Volunteer, Error> Create(
        Guid id, string fullName, string email, string phoneNumber, int experienceYears)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return new Error("volunteer.fullname.empty", "Full name cannot be empty");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return new Error("volunteer.email.invalid", "Email is invalid");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            return new Error("volunteer.phone.empty", "Phone number cannot be empty");

        if (experienceYears < 0)
            return new Error("volunteer.experience.negative", "Experience cannot be negative");

        return new Volunteer(id, fullName.Trim(), email.Trim(), phoneNumber.Trim(), experienceYears);
    }

    // The three counting methods: take a list of pets, count by status
    public int CountPetsFoundHome(IEnumerable<Pet> pets) =>
        pets.Count(p => p.Status == PetStatus.FoundHome);

    public int CountPetsLookingForHome(IEnumerable<Pet> pets) =>
        pets.Count(p => p.Status == PetStatus.LookingForHome);

    public int CountPetsInTreatment(IEnumerable<Pet> pets) =>
        pets.Count(p => p.Status == PetStatus.NeedsHelp);

    public void UpdateDescription(string description) => Description = description;

    public UnitResult<Error> AddSocialNetwork(SocialNetwork socialNetwork)
    {
        if (socialNetwork is null)
            return new Error("volunteer.socialnetwork.null", "Social network cannot be null");

        if (_socialNetworks.Contains(socialNetwork))
            return new Error("volunteer.socialnetwork.duplicate", "This social network was already added");

        _socialNetworks.Add(socialNetwork);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddRequisite(Requisite requisite)
    {
        if (requisite is null)
            return new Error("volunteer.requisite.null", "Requisite cannot be null");

        if (_requisites.Contains(requisite))
            return new Error("volunteer.requisite.duplicate", "This requisite was already added");

        _requisites.Add(requisite);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        if (pet is null)
            return new Error("volunteer.pet.null", "Pet cannot be null");

        if (_pets.Any(p => p.Id == pet.Id))
            return new Error("volunteer.pet.duplicate", "This pet was already added");

        _pets.Add(pet);
        return UnitResult.Success<Error>();
    }
}