namespace TronCell.Queue.Web.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TronCell.Queue.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TronCell.Queue.Web.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TronCell.Queue.Web.Models.ApplicationDbContext context)
        {
            base.Seed(context);


            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Create Admin Role
            string roleAdminName = "Admin";
            string roleManName = "Manager";
            string roleReceiverName = "Receiver";
            string roleSupplierName = "Supplier";
            IdentityResult roleResult;

            // Check to see if Role Exists, if not create it


            if (!RoleManager.RoleExists(roleManName))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleManName));
            }

            if (!RoleManager.RoleExists(roleReceiverName))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleReceiverName));
            }

            if (!RoleManager.RoleExists(roleSupplierName))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleSupplierName));
            }

            if (!RoleManager.RoleExists(roleAdminName))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleAdminName));
            }
            context.SaveChanges();



            var receiveArea = new List<ReceiveArea>()
            {
                new ReceiveArea() { AreaName = "A001窗口", Description = "A001", Category="All",CreateTime=DateTime.Now},
                new ReceiveArea() { AreaName = "A002窗口", Description = "A002", Category="All",CreateTime=DateTime.Now}
            };
            receiveArea.ForEach(area => context.ReceiveArea.Add(area));
            context.SaveChanges();



            var user = new ApplicationUser()
            {
                UserName = "admin",
                TrueName="吴礼旭",
                Email = "wulixu@troncell.com",
                CreatedTime = DateTime.Now,
                CarNum = "苏BV909U",
                IDCard = "360428198305141000",
                CompanyName = "无锡创思感知"
            };
            IdentityResult result = UserManager.Create(user, "1qaz@WSX");


            ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            if (result.Succeeded)
            {
                _userManager.AddToRole(user.Id, roleAdminName);
                //user.Roles.Add(new IdentityUserRole()
            }
            //var fitting = new List<Fitting>()
            //{
            //    new Fitting() {FittingRoomId = 1 },
            //    new Fitting() {FittingRoomId = 2},
            //    new Fitting() {FittingRoomId = 1},
            //    new Fitting() {FittingRoomId = 2}
            //};
            //fitting.ForEach(fitting01 => context.Fittings.Add(fitting01));


            context.SaveChanges();
        }
    }
}
