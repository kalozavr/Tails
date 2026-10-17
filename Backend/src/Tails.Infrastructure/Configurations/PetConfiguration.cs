using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tails.Domain.Pets;

namespace Tails.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100).HasColumnName("name");
            builder.Property(p => p.OwnerPhoneNumber).IsRequired().HasMaxLength(30).HasColumnName("owner_phone_number");
            builder.Property(p => p.Description).HasMaxLength(2000).HasColumnName("description");
            builder.Property(p => p.Color).HasColumnName("color");
            builder.Property(p => p.HealthInfo).HasColumnName("health_info");
            builder.Property(p => p.Address).HasColumnName("address");
            builder.Property(p => p.Weight).HasColumnName("weight");
            builder.Property(p => p.Height).HasColumnName("height");
            builder.Property(p => p.IsNeutered).HasColumnName("is_neutered");
            builder.Property(p => p.IsVaccinated).HasColumnName("is_vaccinated");
            builder.Property(p => p.DateOfBirth).HasColumnName("date_of_birth");
            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.Status).HasConversion<string>().HasColumnName("status");

            // Single VO: stored as two columns inside the pets table
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
}
