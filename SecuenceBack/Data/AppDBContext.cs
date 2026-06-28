using SecuenceBack.Models;
using Microsoft.EntityFrameworkCore;
//using SecuenceBack.Utils;
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
        //public virtual DbSet<Applications> Applications { get; set; }
        //public virtual DbSet<Candidates> Candidates { get; set; }
        //public virtual DbSet<Catalogs> Catalogs { get; set; }
        public virtual DbSet<RolPermissionsRel> RolPermissionsRel {  get; set; }
        public virtual DbSet<PermissionsTbl> PermissionsTbl { get; set; }
        //public virtual DbSet<Questions> Questions { get; set; }
        //public virtual DbSet<QuestionsByApplications> QuestionsByApplications { get; set; }
        public virtual DbSet<RolTbl> RolTbl { get; set; }
        //public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<UserMedicHCRel> UserMedicHCRel { get; set; }
        public virtual DbSet<UserTbl> UserTbl { get; set; }
        //public virtual DbSet<AnswersByQuestionsByApplications> AnswersByQuestionsByApplications { get; set; }
        //public virtual DbSet<Logs> Logs { get; set; }
        //public virtual DbSet<TikestCv> TikestCvs { get; set; }
        //public virtual DbSet<Icons> Icons { get; set; }

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
                //modelBuilder.Entity<AnswersByQuestionsByApplications>().ToTable("AnswersByQuestionsByApplicationsDev");
                //modelBuilder.Entity<Applications>().ToTable("ApplicationsDev");
                //modelBuilder.Entity<Candidates>().ToTable("CandidatesDev");
                //modelBuilder.Entity<Catalogs>().ToTable("CatalogsDev");
                modelBuilder.Entity<PermissionsTbl>().ToTable("PermissionsTbl");
                modelBuilder.Entity<RolPermissionsRel>().ToTable("RolPermissionsRel");
                //modelBuilder.Entity<Questions>().ToTable("QuestionsDev");
                //modelBuilder.Entity<Logs>().ToTable("LogsDev");
                //modelBuilder.Entity<QuestionsByApplications>().ToTable("QuestionsByApplicationsDev");
                modelBuilder.Entity<RolTbl>().ToTable("RolTbl");
                //modelBuilder.Entity<Tags>().ToTable("TagsDev");
                modelBuilder.Entity<UserTbl>().ToTable("UserTbl");
                modelBuilder.Entity<UserMedicHCRel>().ToTable("UserMedicHCRel");
                //modelBuilder.Entity<TikestCv>().ToTable("TikectCvDev");
                //modelBuilder.Entity<Icons>().ToTable("IconsDev");
             }
            else
            {
                //modelBuilder.Entity<AnswersByQuestionsByApplications>().ToTable("AnswersByQuestionsByApplications");
                //modelBuilder.Entity<Applications>().ToTable("Applications");
                //modelBuilder.Entity<Candidates>().ToTable("Candidates");
                //modelBuilder.Entity<Catalogs>().ToTable("Catalogs");
                modelBuilder.Entity<PermissionsTbl>().ToTable("PermissionsTbl");
                modelBuilder.Entity<RolPermissionsRel>().ToTable("RolPermissionsRel");
                //modelBuilder.Entity<Questions>().ToTable("Questions");
                //modelBuilder.Entity<Logs>().ToTable("Logs");
                //modelBuilder.Entity<QuestionsByApplications>().ToTable("QuestionsByApplications");
                modelBuilder.Entity<RolTbl>().ToTable("RolTbl");
                //modelBuilder.Entity<Tags>().ToTable("Tags");
                modelBuilder.Entity<UserMedicHCRel>().ToTable("UserMedicHCRel");
                modelBuilder.Entity<UserTbl>().ToTable("UserTbl");
                //modelBuilder.Entity<TikestCv>().ToTable("TikectCv");
                //modelBuilder.Entity<Icons>().ToTable("Icons");
             }
        }
    }
}
