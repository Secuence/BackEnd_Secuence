using SecuenceBack.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace SecuenceBack.Data
{
    public class AppDBContext : DbContext
    {

        private readonly IConfiguration _appSettings;
        public AppDBContext(DbContextOptions<AppDBContext> options, IConfiguration configuration) : base(options)
        {
            _appSettings = configuration;
        }

        public virtual DbSet<RolPermissionsRel> RolPermissionsRel { get; set; }
        public virtual DbSet<PermissionsTbl> PermissionsTbl { get; set; }
        public virtual DbSet<RolTbl> RolTbl { get; set; }
        public virtual DbSet<UserMedicHCRel> UserMedicHCRel { get; set; }
        public virtual DbSet<UserTbl> UserTbl { get; set; }
        public virtual DbSet<MedicTbl> MedicTbl { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name = defaultConnectionDev");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CORREGIDO: modelBuilder.Model es la propiedad correcta
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                        v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    ));
                }
            }

            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                modelBuilder.Entity<PermissionsTbl>().ToTable("PermissionsTbl");
                modelBuilder.Entity<RolPermissionsRel>().ToTable("RolPermissionsRel");
                modelBuilder.Entity<RolTbl>().ToTable("RolTbl");
                modelBuilder.Entity<UserTbl>().ToTable("UserTbl");
                modelBuilder.Entity<UserMedicHCRel>().ToTable("UserMedicHCRel");
                modelBuilder.Entity<MedicTbl>().ToTable("MedicTbl");
            }
            else
            {
                modelBuilder.Entity<PermissionsTbl>().ToTable("PermissionsTbl");
                modelBuilder.Entity<RolPermissionsRel>().ToTable("RolPermissionsRel");
                modelBuilder.Entity<RolTbl>().ToTable("RolTbl");
                modelBuilder.Entity<UserMedicHCRel>().ToTable("UserMedicHCRel");
                modelBuilder.Entity<UserTbl>().ToTable("UserTbl");
                modelBuilder.Entity<MedicTbl>().ToTable("MedicTbl");
            }
        }
    }
}
