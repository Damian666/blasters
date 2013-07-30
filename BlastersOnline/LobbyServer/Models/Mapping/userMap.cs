using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BlastersShared.Models;

namespace LobbyServer.Models.Mapping
{
    public class userMap : EntityTypeConfiguration<User>
    {
        public userMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("user", "blasters");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.BlastersMembersID).HasColumnName("blastersmembers_id").IsRequired();
            this.Property(t => t.Name).HasColumnName("name").IsRequired();
            this.Property(t => t.CreationDate).HasColumnName("CreationDate").IsRequired();

            // Relationships
            this.HasRequired(t => t.BlastersMember)
                .WithMany(t => t.users)
                .HasForeignKey(d => d.BlastersMembersID);



        }
    }
}
