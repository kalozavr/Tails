using CSharpFunctionalExtensions;

namespace Tails.Domain.Shared;

public class Address : ValueObject
{
    public const int MaxCityLength = 100;
    public const int MaxStreetLength = 150;
    public const int MaxHouseLength = 20;

    public string City { get; }
    public string Street { get; }
    public string House { get; }

    // EF Core
    private Address() { }

    private Address(string city, string street, string house)
    {
        City = city;
        Street = street;
        House = house;
    }

    public static Result<Address, Error> Create(string city, string street, string house)
    {
        if (string.IsNullOrWhiteSpace(city) || city.Trim().Length > MaxCityLength)
            return new Error("address.city.invalid", $"City is required (max {MaxCityLength} characters)");

        if (string.IsNullOrWhiteSpace(street) || street.Trim().Length > MaxStreetLength)
            return new Error("address.street.invalid", $"Street is required (max {MaxStreetLength} characters)");

        if (string.IsNullOrWhiteSpace(house) || house.Trim().Length > MaxHouseLength)
            return new Error("address.house.invalid", $"House is required (max {MaxHouseLength} characters)");

        return new Address(city.Trim(), street.Trim(), house.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Street;
        yield return House;
    }
}