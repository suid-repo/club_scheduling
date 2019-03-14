namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventCreationTimeAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "CreationTime", c => c.DateTime(nullable: true, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "CreationTime");
        }
    }
}
