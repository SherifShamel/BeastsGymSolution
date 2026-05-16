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
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.PlanName).HasColumnType("varchar(100)");
            builder.Property(p => p.Description).HasMaxLength(200);
            builder.Property(p => p.Price).HasColumnType("decimal(10,2)");

            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GetDate()");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("DurationCheckValue", "Duration Between 1 AND 365");
            });
        }
    }
}
