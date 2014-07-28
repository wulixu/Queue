using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Queue.Entities.Models;

namespace TronCell.Queue.Web
{
    public class RetailDataContext : DbContext
    {

        //public DbSet<CGroup> CGroups { get; set; }
        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<Contact> Contacts { get; set; }

        public DbSet<Fitting> Fittings { get; set; }
        public DbSet<FittingRoom> FittingRooms { get; set; }

        public RetailDataContext()
            : this("DefaultConnection")
        {
            
        }
        public RetailDataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            //Database.SetInitializer<RetailDataContext>(new DropCreateDatabaseIfModelChanges<RetailDataContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Fitting>()
                .HasRequired(f => f.FittingRoom)
                .WithMany(f => f.Fittings)
                .HasForeignKey(p => p.FittingRoomId);
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Contact>().HasOptional(c => c.Address)
            //    .WithOptionalDependent(add => add.Contact);
            //modelBuilder.Entity<CGroup>().HasMany(c => c.Contacts)
            //    .WithRequired(c => c.CGroup).WillCascadeOnDelete(false);

        }
    }
}