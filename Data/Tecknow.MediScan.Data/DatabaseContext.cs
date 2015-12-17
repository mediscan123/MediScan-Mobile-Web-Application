using System.Data.Entity;
using Tecknow.MediScan.Entities;

namespace Tecknow.MediScan.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=MediScanConnectionString")
        {

        }

        public DbSet<TenantMaster> TenantsMaster { get; set; }
        public DbSet<ApplicationUserMaster> ApplicationUserMaster { get; set; }
        public DbSet<AddressMaster> AddressMaster { get; set; }
        public DbSet<RoleMaster> RoleMaster { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<TenantMaster>().ToTable("TenanatMaster", "public");
            modelBuilder.Entity<ApplicationUserMaster>().ToTable("ApplicationUserMaster", "public");
            modelBuilder.Entity<AddressMaster>().ToTable("AddressMaster", "public");
            modelBuilder.Entity<RoleMaster>().ToTable("RoleMaster", "public");

            base.OnModelCreating(modelBuilder);


        }
    }
}