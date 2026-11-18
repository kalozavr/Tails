using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Tails.Domain.Shared;

namespace Tails.Domain.Pets
{
    public class Pet : Entity<Guid>
    {
        // EF Core
        private Pet() { }

        public const int MaxNameLength = 100;
        public const int MaxDescriptionLength = 2000;
        private readonly List<Requisite> _requisites = [];

        // Required
        public string Name { get; private set; }
        public SpeciesBreed SpeciesBreed { get; private set; }
        public PhoneNumber OwnerPhoneNumber { get; private set; }

        // Default values
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public PetStatus Status { get; private set; } = PetStatus.NeedsHelp;

        // Optional
        public string Description { get; private set; } = string.Empty;
        public string Color { get; private set; } = string.Empty;
        public string HealthInfo { get; private set; } = string.Empty;
        public Address Address { get; private set; }
        public double Weight { get; private set; } = default;
        public double Height { get; private set; } = default;
        public bool IsNeutered { get; private set; } = default;
        public DateTime DateOfBirth { get; private set; } = default;
        public bool IsVaccinated { get; private set; } = default;
        public IReadOnlyList<Requisite> Requisites => _requisites;

        private Pet(Guid id, string name, SpeciesBreed speciesBreed, PhoneNumber ownerPhoneNumber, Address address)
        : base(id)
        {
            Name = name;
            SpeciesBreed = speciesBreed;
            OwnerPhoneNumber = ownerPhoneNumber;
            Address = address;
        }

        public static Result<Pet, Error> Create(Guid id, string name, SpeciesBreed speciesBreed, PhoneNumber ownerPhoneNumber, Address address)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > MaxNameLength)
            {
                return new Error("pet.name.invalid", $"Pet name is required (max {MaxNameLength} characters)");
            }

            if (speciesBreed is null)
            {
                return new Error("pet.speciesbreed.null", "Species and breed must be specified");
            }

            if (ownerPhoneNumber is null)
            {
                return new Error("pet.phone.null", "Owner phone number must be specified");
            }

            if (address is null)
            {
                return new Error("pet.address.null", "Address must be specified");
            }

            return new Pet(id, name.Trim(), speciesBreed, ownerPhoneNumber, address);
        }

        public UnitResult<Error> SetPhysicalInfo(string color, double weight, double height)
        {
            if (weight <= 0)
            {
                return new Error("pet.weight.invalid", "Weight must be greater than zero");
            }

            if (height <= 0)
            {
                return new Error("pet.height.invalid", "Height must be greater than zero");
            }

            Color = color;
            Weight = weight;
            Height = height;
            return UnitResult.Success<Error>();
        }

        public UnitResult<Error> SetDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > DateTime.UtcNow)
            {
                return new Error("pet.birthdate.future", "Date of birth cannot be in the future");
            }

            DateOfBirth = dateOfBirth;
            return UnitResult.Success<Error>();
        }

        public void UpdateHealth(string healthInfo, bool isNeutered, bool isVaccinated)
        {
            HealthInfo = healthInfo;
            IsNeutered = isNeutered;
            IsVaccinated = isVaccinated;
        }

        public void UpdateDescription(string description) => Description = description;

        public void ChangeStatus(PetStatus status) => Status = status;

        public UnitResult<Error> AddRequisite(Requisite requisite)
        {
            if (requisite is null)
            {
                return new Error("pet.requisite.null", "Requisite cannot be null");
            }

            if (_requisites.Contains(requisite))
            {
                return new Error("pet.requisite.duplicate", "This requisite was already added");
            }

            _requisites.Add(requisite);
            return UnitResult.Success<Error>();
        }
    }
}
