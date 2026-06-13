using BeastsGym.BLL.Classes;
using BeastsGym.BLL.Interfaces;
using BeastsGym.BLL.Utilities;
using BeastsGym.DAL.Contexts;
using BeastsGym.DAL.Repositories.classes;
using BeastsGym.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeastsGym
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BeastsGymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            //builder.Services.AddScoped(typeof(IgenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMemberServices, MemberServices>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionServices, SessionServices>();

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
