namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnerFamilyAttr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Family_Id", "dbo.Families");
            AddColumn("dbo.AspNetUsers", "Family_Id1", c => c.Int());
            AddColumn("dbo.Families", "Owner_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Family_Id1");
            CreateIndex("dbo.Families", "Owner_Id");
            AddForeignKey("dbo.Families", "Owner_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Family_Id1", "dbo.Families", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Family_Id1", "dbo.Families");
            DropForeignKey("dbo.Families", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Families", new[] { "Owner_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Family_Id1" });
            DropColumn("dbo.Families", "Owner_Id");
            DropColumn("dbo.AspNetUsers", "Family_Id1");
            AddForeignKey("dbo.AspNetUsers", "Family_Id", "dbo.Families", "Id");
        }
    }
}
