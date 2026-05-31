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
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacityCheck", "Capacity BETWEEN 1 AND 25");
                tb.HasCheckConstraint("SessionEndDateCheck", "EndDate >= StartDate");
            });
        }
    }
}
