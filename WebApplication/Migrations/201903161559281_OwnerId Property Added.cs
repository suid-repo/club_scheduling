namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnerIdPropertyAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Families", "Owner_Id", "dbo.AspNetUsers");
            RenameColumn(table: "dbo.Families", name: "Owner_Id", newName: "OwnerId");
            RenameIndex(table: "dbo.Families", name: "IX_Owner_Id", newName: "IX_OwnerId");
            AddForeignKey("dbo.Families", "OwnerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Families", "OwnerId", "dbo.AspNetUsers");
            RenameIndex(table: "dbo.Families", name: "IX_OwnerId", newName: "IX_Owner_Id");
            RenameColumn(table: "dbo.Families", name: "OwnerId", newName: "Owner_Id");
            AddForeignKey("dbo.Families", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
