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
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.CategoryName).HasColumnType("varchar")
                .HasMaxLength(20);
                
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()");

            builder.HasData(
                new Category { Id = 1, CategoryName = "Cardio" },
                new Category { Id = 2, CategoryName = "Strength" },
                new Category { Id = 3, CategoryName = "Flexibility" },
                new Category { Id = 4, CategoryName = "Balance" }
            );
        }
    }
}
