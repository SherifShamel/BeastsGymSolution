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
    internal class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(x => x.PlanId);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
