using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BlastersShared.Models;

namespace LobbyServer.Models.Mapping
{


    public class GameSessionEntryMap : EntityTypeConfiguration<GameSessionEntry>
    {
        public GameSessionEntryMap()
        {
            // Primary Key
            this.HasKey(t => t.SessionId);

            // Properties
            this.Property(t => t.MatchName)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.SessionId).HasColumnName("SessionId");

            // Table & Column Mappings
            this.ToTable("gamesession_logs", "blasters");
            this.Property(t => t.SessionId).HasColumnName("SessionId").IsRequired();
            Property(t => t.StartDate).HasColumnName("StartDate").IsRequired();
            Property(t => t.EndDate).HasColumnName("EndDate").IsRequired();


        }
    }

}
