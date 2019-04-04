namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsFakeAccountadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsFakeAccount", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsFakeAccount");
        }
    }
}
