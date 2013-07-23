using System.IO;

namespace LobbyServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LobbyServer.Models.BlastersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(LobbyServer.Models.BlastersContext context)
        {
             var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", string.Empty) + "\\Migrations";

            // Execute SQL to keep users up to date
            context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\seed_users.sql"));


        }
    }
}
