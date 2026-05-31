using BeastsGym.DAL.Configurations;
using BeastsGym.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Contexts
{
    public class BeastsGymDbContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=BeastsGymDb;Trusted_Connection=True;TrustServerCertificate=True;");
        //}
        public BeastsGymDbContext(DbContextOptions<BeastsGymDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Booking> Bookings { get; set; }


    }
}
