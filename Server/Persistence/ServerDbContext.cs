using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;

namespace Server.Persistence
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) 
        { 
            
        }

        public DbSet<CardProfile> CardProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}