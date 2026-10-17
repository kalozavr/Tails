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

        private readonly List<Requisite> _requisites = [];

        // Required
        public string Name { get; private set; }
        public SpeciesBreed SpeciesBreed { get; private set; }
        public string OwnerPhoneNumber { get; private set; }

        // Default values
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public PetStatus Status { get; private set; } = PetStatus.NeedsHelp;

        // Optional
        public string Description { get; private set; } = string.Empty;
        public string Color { get; private set; } = string.Empty;
        public string HealthInfo { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public double Weight { get; private set; } = default;
        public double Height { get; private set; } = default;
        public bool IsNeutered { get; private set; } = default;
        public DateTime DateOfBirth { get; private set; } = default;
        public bool IsVaccinated { get; private set; } = default;
        public IReadOnlyList<Requisite> Requisites => _requisites;

        private Pet(Guid id, string name, SpeciesBreed speciesBreed, string ownerPhoneNumber)
        : base(id)
        {
            Name = name;
            SpeciesBreed = speciesBreed;
            OwnerPhoneNumber = ownerPhoneNumber;
        }

        public static Result<Pet, Error> Create(
        Guid id, string name, SpeciesBreed speciesBreed, string ownerPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new Error("pet.name.empty", "Pet name cannot be empty");
            }

            if (speciesBreed is null)
            {
                return new Error("pet.speciesbreed.null", "Species and breed must be specified");
            }

            if (string.IsNullOrWhiteSpace(ownerPhoneNumber))
            {
                return new Error("pet.phone.empty", "Owner phone number cannot be empty");
            }

            return new Pet(id, name.Trim(), speciesBreed, ownerPhoneNumber.Trim());
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

        public void SetAddress(string address) => Address = address;

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
