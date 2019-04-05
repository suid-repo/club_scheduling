namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LevelIdAdded : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "Level_Id", newName: "LevelId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Level_Id", newName: "IX_LevelId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_LevelId", newName: "IX_Level_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "LevelId", newName: "Level_Id");
        }
    }
}
