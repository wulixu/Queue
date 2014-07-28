namespace TronCell.Queue.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QueueCall",
                c => new
                    {
                        QueueCallId = c.Int(nullable: false, identity: true),
                        QueueNum = c.String(),
                        State = c.Int(nullable: false),
                        Priority = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        QueueUserId = c.String(nullable: false, maxLength: 128),
                        OperationId = c.String(maxLength: 128),
                        ReceiveAreaId = c.Int(),
                    })
                .PrimaryKey(t => t.QueueCallId)
                .ForeignKey("dbo.Users", t => t.OperationId)
                .ForeignKey("dbo.Users", t => t.QueueUserId, cascadeDelete: true)
                .ForeignKey("dbo.ReceiveArea", t => t.ReceiveAreaId)
                .Index(t => t.QueueUserId)
                .Index(t => t.OperationId)
                .Index(t => t.ReceiveAreaId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                        CreatedTime = c.DateTime(),
                        IDCard = c.String(),
                        CompanyName = c.String(),
                        CarNum = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.ReceiveArea",
                c => new
                    {
                        ReceiveAreaId = c.Int(nullable: false, identity: true),
                        AreaName = c.String(),
                        Description = c.String(),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.ReceiveAreaId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserLogins", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserClaims", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.QueueCall", "ReceiveAreaId", "dbo.ReceiveArea");
            DropForeignKey("dbo.QueueCall", "QueueUserId", "dbo.Users");
            DropForeignKey("dbo.QueueCall", "OperationId", "dbo.Users");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.QueueCall", new[] { "ReceiveAreaId" });
            DropIndex("dbo.QueueCall", new[] { "OperationId" });
            DropIndex("dbo.QueueCall", new[] { "QueueUserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReceiveArea");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.QueueCall");
        }
    }
}
