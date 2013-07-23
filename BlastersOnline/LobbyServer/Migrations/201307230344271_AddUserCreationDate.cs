namespace LobbyServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("user", "CreationDate", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow  ));
        }
        
        public override void Down()
        {
            DropColumn("user", "CreationDate");
        }        
    }
}
