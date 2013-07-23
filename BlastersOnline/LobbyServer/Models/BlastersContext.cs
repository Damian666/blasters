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
        }

        public DbSet<blastersmember> blastersmembers { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new blastersmemberMap());
            modelBuilder.Configurations.Add(new userMap());
        }
    }
}
