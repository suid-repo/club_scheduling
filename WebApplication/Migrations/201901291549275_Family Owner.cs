namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FamilyOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Families", "Owner_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Families", "Owner_Id");
            AddForeignKey("dbo.Families", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Families", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Families", new[] { "Owner_Id" });
            DropColumn("dbo.Families", "Owner_Id");
        }
    }
}
