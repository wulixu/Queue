using System.Data.Entity.Migrations;
using System.Globalization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using TronCell.Queue.Web.Models;
namespace TronCell.Queue.Web.DAL
{
    public class QueueInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
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
                new ReceiveArea() { AreaName = "A002窗口", Description = "A002",Category="All",CreateTime=DateTime.Now}
            };
            receiveArea.ForEach(area => context.ReceiveArea.AddOrUpdate(area));
            context.SaveChanges();

            var users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    UserName = "admin",
                    TrueName = "吴礼旭",
                    Email = "wulixu@troncell.com",
                    CreatedTime = DateTime.Now,
                    CarNum = "苏BV909U",
                    IDCard = "360428198305141000",
                    CompanyName = "无锡创思感知"
                },

                new ApplicationUser()
                {
                    UserName = "supplier",
                    TrueName = "周保光",
                    Email = "zhoubaoguang@troncell.com",
                    CreatedTime = DateTime.Now,
                    CarNum = "苏BV505U",
                    IDCard = "360428198305141100",
                    CompanyName = "无锡创思感知"
                },

                new ApplicationUser()
                {
                    UserName = "receiver",
                    TrueName = "杨永光",
                    Email = "lixubang@troncell.com",
                    CreatedTime = DateTime.Now,
                    CarNum = "苏BV605U",
                    IDCard = "360428198305141112",
                    CompanyName = "无锡创思感知"
                }
            };
            users.ForEach(user =>
            {
                var existUser = UserManager.FindByName(user.UserName);
                if (existUser == null)
                {
                    IdentityResult result = UserManager.Create(user, "123456");

                    if (result.Succeeded)
                    {
                        if (user.UserName.ToLower() == roleAdminName.ToLower())
                        {
                            UserManager.AddToRole(user.Id, roleAdminName);
                        }
                        if (user.UserName.ToLower() == roleSupplierName.ToLower())
                        {
                            UserManager.AddToRole(user.Id, roleSupplierName);
                            for (int i = 0; i < 100; i++)
                            {
                                var queue = new QueueCall()
                                {
                                    CreateTime = DateTime.Now,
                                    QueueNum = i.ToString(CultureInfo.InvariantCulture).PadLeft(4,'0'),
                                    StartTime = DateTime.Now,
                                    QueueUserId = user.Id,
                                    State = ProcessStatus.GotQueueNumber,
                                    Priority = PriorityStatus.General,
                                    Deleted = false
                                };
                                context.Queues.AddOrUpdate(queue);
                                
                            }
                        }
                        if (user.UserName.ToLower() == roleReceiverName.ToLower())
                        {
                            UserManager.AddToRole(user.Id, roleReceiverName);
                        }
                        if (user.UserName.ToLower() == roleManName.ToLower())
                        {
                            UserManager.AddToRole(user.Id, roleManName);
                        }
                    }
                }
            });



            
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