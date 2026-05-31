using BeastsGym.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name).IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.PhoneNumber).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email LIKE '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "PhoneNumber LIKE '01[0-2,5][0-9]{8}'");
            });

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(30);
                address.Property(a => a.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(30);
                address.Property(a => a.BuildingNumber).HasColumnName("BuildingNumber").HasColumnType("int");
            });
        }
    }
}
