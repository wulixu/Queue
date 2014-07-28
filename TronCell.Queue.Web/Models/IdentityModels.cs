using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

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

        public virtual DateTime CreatedTime { get; set; }

        public string IDCard { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        //[Required(ErrorMessage = "公司名称不能为空")]
        [Display(Name = "公司名称")]
        public string CompanyName { get; set; }

        [Display(Name = "车牌号")]
        public string CarNum { get; set; }
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
            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users");
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");

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
     

    }
}