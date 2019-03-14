namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QueuedClasses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.Families",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QueuedItems",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        QueuedId = c.Int(nullable: false),
                        Time = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.QueuedId })
                .ForeignKey("dbo.Queueds", t => t.QueuedId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.QueuedId);
            
            CreateTable(
                "dbo.Queueds",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId);
            
            AddColumn("dbo.AspNetUsers", "Family_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Family_Id");
            AddForeignKey("dbo.AspNetUsers", "Family_Id", "dbo.Families", "Id");
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.QueuedItems", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.QueuedItems", "QueuedId", "dbo.Queueds");
            DropForeignKey("dbo.Queueds", "EventId", "dbo.Events");
            DropForeignKey("dbo.AspNetUsers", "Family_Id", "dbo.Families");
            DropIndex("dbo.Queueds", new[] { "EventId" });
            DropIndex("dbo.QueuedItems", new[] { "QueuedId" });
            DropIndex("dbo.QueuedItems", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Family_Id" });
            DropColumn("dbo.AspNetUsers", "Family_Id");
            DropTable("dbo.Queueds");
            DropTable("dbo.QueuedItems");
            DropTable("dbo.Families");
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
