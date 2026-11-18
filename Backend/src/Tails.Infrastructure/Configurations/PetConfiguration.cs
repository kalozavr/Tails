using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tails.Domain.Pets;
using Tails.Domain.Shared;

namespace Tails.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.Name).IsRequired().HasMaxLength(Pet.MaxNameLength).HasColumnName("name");
        builder.Property(p => p.Description).HasMaxLength(Pet.MaxDescriptionLength).HasColumnName("description");
        builder.Property(p => p.Color).HasColumnName("color");
        builder.Property(p => p.HealthInfo).HasColumnName("health_info");
        builder.Property(p => p.Weight).HasColumnName("weight");
        builder.Property(p => p.Height).HasColumnName("height");
        builder.Property(p => p.IsNeutered).HasColumnName("is_neutered");
        builder.Property(p => p.IsVaccinated).HasColumnName("is_vaccinated");
        builder.Property(p => p.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(p => p.CreatedAt).HasColumnName("created_at");
        builder.Property(p => p.Status).HasConversion<string>().HasColumnName("status");

        // NEW: single-value VO, stored in one column
        builder.Property(p => p.OwnerPhoneNumber)
            .HasConversion(v => v.Value, v => PhoneNumber.Create(v).Value)
            .IsRequired()
            .HasMaxLength(PhoneNumber.MaxLength)
            .HasColumnName("owner_phone_number");

        // NEW: multi-field VO, stored as columns in the pets table
        builder.ComplexProperty(p => p.Address, b =>
        {
            b.Property(a => a.City).IsRequired().HasMaxLength(Address.MaxCityLength).HasColumnName("address_city");
            b.Property(a => a.Street).IsRequired().HasMaxLength(Address.MaxStreetLength).HasColumnName("address_street");
            b.Property(a => a.House).IsRequired().HasMaxLength(Address.MaxHouseLength).HasColumnName("address_house");
        });

        // Same as before
        builder.OwnsOne(p => p.SpeciesBreed, b =>
        {
            b.Property(x => x.SpeciesId).HasColumnName("species_id");
            b.Property(x => x.BreedId).HasColumnName("breed_id");
        });

        builder.OwnsMany(p => p.Requisites, b =>
        {
            b.ToJson("requisites");
            b.Property(r => r.Name);
            b.Property(r => r.Description);
        });
    }
}