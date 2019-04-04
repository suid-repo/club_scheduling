namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QueuedObjectRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserEvents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserEvents", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Queueds", "EventId", "dbo.Events");
            DropForeignKey("dbo.QueuedItems", "QueuedId", "dbo.Queueds");
            DropForeignKey("dbo.QueuedItems", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.QueuedItems", new[] { "UserId" });
            DropIndex("dbo.QueuedItems", new[] { "QueuedId" });
            DropIndex("dbo.Queueds", new[] { "EventId" });
            DropIndex("dbo.ApplicationUserEvents", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserEvents", new[] { "Event_Id" });
            CreateTable(
                "dbo.MemberEvents",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Time = c.Long(nullable: false),
                        isRegistered = c.Boolean(nullable: false, defaultValue:false),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Events", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Events", "ApplicationUser_Id");
            AddForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.QueuedItems");
            DropTable("dbo.Queueds");
            DropTable("dbo.ApplicationUserEvents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserEvents",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Event_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Event_Id });
            
            CreateTable(
                "dbo.Queueds",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.QueuedItems",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        QueuedId = c.Int(nullable: false),
                        Time = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.QueuedId });
            
            DropForeignKey("dbo.MemberEvents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MemberEvents", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.MemberEvents", new[] { "UserId" });
            DropIndex("dbo.MemberEvents", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Events", "ApplicationUser_Id");
            DropTable("dbo.MemberEvents");
            CreateIndex("dbo.ApplicationUserEvents", "Event_Id");
            CreateIndex("dbo.ApplicationUserEvents", "ApplicationUser_Id");
            CreateIndex("dbo.Queueds", "EventId");
            CreateIndex("dbo.QueuedItems", "QueuedId");
            CreateIndex("dbo.QueuedItems", "UserId");
            AddForeignKey("dbo.QueuedItems", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QueuedItems", "QueuedId", "dbo.Queueds", "EventId", cascadeDelete: true);
            AddForeignKey("dbo.Queueds", "EventId", "dbo.Events", "Id");
            AddForeignKey("dbo.ApplicationUserEvents", "Event_Id", "dbo.Events", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserEvents", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
