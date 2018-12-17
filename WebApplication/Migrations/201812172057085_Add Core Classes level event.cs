namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoreClasseslevelevent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LevelEvents",
                c => new
                    {
                        Level_Id = c.Int(nullable: false),
                        Event_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Level_Id, t.Event_Id })
                .ForeignKey("dbo.Levels", t => t.Level_Id, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .Index(t => t.Level_Id)
                .Index(t => t.Event_Id);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "BirthDay", c => c.String());
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Level_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Event_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "Level_Id");
            CreateIndex("dbo.AspNetUsers", "Event_Id");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Level_Id", "dbo.Levels", "Id");
            AddForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.AspNetUsers", "Level_Id", "dbo.Levels");
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.LevelEvents", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.LevelEvents", "Level_Id", "dbo.Levels");
            DropIndex("dbo.LevelEvents", new[] { "Event_Id" });
            DropIndex("dbo.LevelEvents", new[] { "Level_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Event_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Level_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Event_Id");
            DropColumn("dbo.AspNetUsers", "Level_Id");
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "BirthDay");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("dbo.LevelEvents");
            DropTable("dbo.Levels");
            DropTable("dbo.Events");
        }
    }
}
