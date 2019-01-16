namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FamilyCoreUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Families", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Families", "Name", c => c.String());
        }
    }
}
