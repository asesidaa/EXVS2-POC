using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Models.Cards;
using ServerVanilla.Models.Cards.Common;
using ServerVanilla.Models.Cards.MobileSuit;
using ServerVanilla.Models.Cards.Navi;
using ServerVanilla.Models.Cards.Profile;
using ServerVanilla.Models.Cards.Team;
using ServerVanilla.Models.Cards.Triad;


namespace ServerVanilla.Persistence
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) 
        { 
            
        }
        
        public DbSet<CardProfile> CardProfiles { get; set; }
        public DbSet<MobileSuitUsage> MobileSuitUsageDbSet { get; set; }
        public DbSet<Navi> NaviDbSet { get; set; }
        public DbSet<TagTeamData> TagTeamData { get; set; }
        public DbSet<BattleProfile> BattleProfileDbSet { get; set; }
        
        // Triad Domain
        public DbSet<TriadMiscInfo> TriadMiscInfoDbSet { get; set; }
        public DbSet<TriadPartner> TriadPartnerDbSet { get; set; }
        public DbSet<TriadCourseData> TriadCourseDataDbSet { get; set; }
        
        public DbSet<EchelonSetting> EchelonSettings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder
                .Entity<CardProfile>()
                .HasMany(e => e.FavouriteMobileSuits)
                .WithOne(e => e.CardProfile)
                .OnDelete(DeleteBehavior.ClientCascade);
            
            modelBuilder
                .Entity<CardProfile>()
                .HasMany(e => e.OnlinePairs)
                .WithOne(e => e.CardProfile)
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        public override int SaveChanges()
        {
            var entityEntries = ChangeTracker.Entries().ToList();

            entityEntries.ForEach(entityEntry =>
            {
                if (entityEntry.State == EntityState.Added)
                {
                    Entry(entityEntry.Entity).Property(nameof(BaseEntity.CreateTime)).CurrentValue = DateTime.Now;
                    Entry(entityEntry.Entity).Property(nameof(BaseEntity.UpdateTime)).CurrentValue = DateTime.Now;
                }
                if (entityEntry.State == EntityState.Modified)
                {
                    Entry(entityEntry.Entity).Property(nameof(BaseEntity.UpdateTime)).CurrentValue = DateTime.Now;
                }
            });
            
            return base.SaveChanges();
        }
    }
}