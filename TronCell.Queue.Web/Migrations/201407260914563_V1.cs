namespace TronCell.Queue.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FittingRoom",
                c => new
                    {
                        FittingRoomId = c.Int(nullable: false, identity: true),
                        RoomName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.FittingRoomId);
            
            CreateTable(
                "dbo.Fitting",
                c => new
                    {
                        FittingId = c.Int(nullable: false, identity: true),
                        FittingRoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FittingId)
                .ForeignKey("dbo.FittingRoom", t => t.FittingRoomId, cascadeDelete: true)
                .Index(t => t.FittingRoomId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fitting", "FittingRoomId", "dbo.FittingRoom");
            DropIndex("dbo.Fitting", new[] { "FittingRoomId" });
            DropTable("dbo.Fitting");
            DropTable("dbo.FittingRoom");
        }
    }
}
