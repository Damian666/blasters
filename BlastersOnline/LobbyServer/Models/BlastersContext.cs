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
            Database.SetInitializer<BlastersContext>(null);
        }

        public BlastersContext()
            : base("Name=blastersContext")
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
