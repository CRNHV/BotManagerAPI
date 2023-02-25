using System.Linq;
using BotManager.Lib.Data;
using BotManager.Lib.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BotManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            // builder.Services.AddDbContext<ManagerContext>(opt => opt.UseInMemoryDatabase("database"));
            builder.Services.AddDbContext<ManagerContext>(opt => opt.UseSqlite($"Data Source=../BotManager/db.sqlite"));
            //builder.Services.AddDbContext<ManagerContext>(opt => opt.UseSqlServer($"Server=localhost;Database=BotManager;Trust Server Certificate=true;Trusted_Connection=True;"));

            builder.Services.AddScoped<IBotService, BotService>();

            builder.Services.AddLogging();

            var app = builder.Build();

            var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ManagerContext>();
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
                if (!dbContext.Settings.Any())
                {
                    dbContext.Settings.Add(new Lib.Data.Entities.Settings()
                    {
                        KillCountPerDay = 60
                    });

                    dbContext.SaveChanges();                    
                }
            }

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}