using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Collections.Generic;

namespace TronCell.Queue.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreatedTime { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        //[Required(ErrorMessage = "公司名称不能为空")]
        [Display(Name = "公司名称")]
        public string CompanyName { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "车牌号")]
        public string CarNum { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name="真实姓名")]
        public string TrueName { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Required]
        public bool Deleted { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<QueueCall> Queues { get; set; }
        public DbSet<ReceiveArea> ReceiveArea { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            // Change the name of the table to be Users instead of AspNetUsers
            //modelBuilder.Entity<IdentityUser>()
            //    .ToTable("Users");
            //modelBuilder.Entity<ApplicationUser>()
            //    .ToTable("Users");

            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).Property(p => p.Name).IsRequired();
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            //modelBuilder.Entity<IdentityUserLogin>().HasKey(u => new { u.UserId, u.LoginProvider, u.ProviderKey });


            //modelBuilder.Entity<IdentityUser>()
            //    .ToTable("Users");
            //modelBuilder.Entity<ApplicationUser>()
            //    .ToTable("Users");

            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<QueueCall>()
                .HasRequired(a => a.QueueUser)
                .WithMany()
                .HasForeignKey(u => u.QueueUserId);

            modelBuilder.Entity<QueueCall>()
                .HasOptional(a => a.Operation)
                .WithMany()
                .HasForeignKey(u => u.OperationId).WillCascadeOnDelete(false);


            modelBuilder.Entity<QueueCall>()
                .HasOptional(a => a.ReceiveArea)
                .WithMany()
                .HasForeignKey(u => u.ReceiveAreaId).WillCascadeOnDelete(false);

        }
    }

    public class IdentityManager
    {
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);

        }

        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.Create(new IdentityRole(name)).Succeeded;

        }

        // 将使用者加入角色中
        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;


        }

        public List<IdentityRole> GetRoles()
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.Roles.ToList();
        }
    }
}