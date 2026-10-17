using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tails.Domain.Volunteers;

namespace Tails.Infrastructure.Configurations
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>           
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).HasColumnName("id");

            builder.Property(v => v.FullName).IsRequired().HasMaxLength(200).HasColumnName("full_name");
            builder.Property(v => v.Email).IsRequired().HasMaxLength(200).HasColumnName("email");
            builder.Property(v => v.PhoneNumber).IsRequired().HasMaxLength(30).HasColumnName("phone_number");
            builder.Property(v => v.Description).HasMaxLength(2000).HasColumnName("description");
            builder.Property(v => v.ExperienceYears).HasColumnName("experience_years");

            // Collections of VOs become JSON columns
            builder.OwnsMany(v => v.SocialNetworks, b =>
            {
                b.ToJson("social_networks");
                b.Property(s => s.Name);
                b.Property(s => s.Link);
            });

            builder.OwnsMany(v => v.Requisites, b =>
            {
                b.ToJson("requisites");
                b.Property(r => r.Name);
                b.Property(r => r.Description);
            });

            // Pets belong to the volunteer (FK column volunteer_id on pets)
            builder.HasMany(v => v.Pets)
                .WithOne()
                .HasForeignKey("volunteer_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
