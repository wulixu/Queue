using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TronCell.Queue.Web.Models;

namespace TronCell.Queue.Web
{
    public class QueueDataContext : DbContext
    {
        public DbSet<QueueCall> Queues { get; set; }
        public DbSet<ReceiveArea> ReceiveArea { get; set; }
        public QueueDataContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<QueueDataContext>(new CreateDatabaseIfNotExists<QueueDataContext>());
        }
        public QueueDataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //Database.SetInitializer<RetailDataContext>(new DropCreateDatabaseIfModelChanges<RetailDataContext>());
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
}