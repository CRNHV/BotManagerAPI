using BotManager.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotManager.Api.Data
{
    public class ManagerContext : DbContext
    {
        public ManagerContext()
        {

        }

        public ManagerContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BotProfile> BotProfile { get; set; }
        public DbSet<BotActivity> BotActivity { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Loot> Loot { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BotProfile>()
                .HasMany(x => x.Activity)
                .WithOne();

            modelBuilder.Entity<BotActivity>()
                .HasMany(x => x.Loot)
                .WithOne();

            modelBuilder.Entity<Settings>().HasData(new Settings()
            {
                Id = 1,
                KillCountPerDay = 60
            });
        }
    }
}
