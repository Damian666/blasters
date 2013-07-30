using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using BlastersShared.Models;
using LobbyServer.Models.Mapping;

namespace LobbyServer.Models
{
    public partial class BlastersContext : DbContext
    {
        static BlastersContext()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<BlastersContext>());
        }

        public BlastersContext()
            : base("blastersContext")
        {
            Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<blastersmember> blastersmembers { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<GameSessionEntry> GameSessionEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new blastersmemberMap());
            modelBuilder.Configurations.Add(new userMap());
            modelBuilder.Configurations.Add(new GameSessionEntryMap());



        }
    }
}
