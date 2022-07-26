using ChangeRequestManagement.Entities.Seeds;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChangeRequestManagement.Entities.Context
{
    public class CRMDbContext : DbContext
    {
        public CRMDbContext(DbContextOptions<CRMDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Role { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<ListItem> ListItem { get; set; }
        public DbSet<ListItemCategory> ListItemCategory { get; set; }
        public DbSet<ChangeRequest> ChangeRequest { get; set; }
        public DbSet<ChangeRequestDocument> ChangeRequestDocument { get; set; }
        public DbSet<ChangeRequestStatus> ChangeRequestStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>()
                .HasIndex(i => i.RoleName)
                .IsUnique();

            builder.Entity<Person>()
                .HasIndex(i => i.EmailAddress)
                .IsUnique();

            builder.Entity<Admin>()
                .HasIndex(i => i.PersonId)
                .IsUnique();

            builder.Entity<Client>()
                .HasIndex(i => i.PersonId)
                .IsUnique();

            builder.Entity<Company>()
                .HasIndex(i => i.CompanyName)
                .IsUnique();

            builder.Entity<Project>()
                .HasIndex(i => new { i.ProjectName, i.CompanyId })
                .IsUnique();

            builder.Entity<Module>()
              .HasIndex(i => new { i.ModuleId, i.ProjectId, i.ParentModulelId })
              .IsUnique();

            builder.Entity<ChangeRequest>()
                .HasIndex(i => i.ChangeRequestNumber)
                .IsUnique();

            builder.Entity<ChangeRequestDocument>() 
               .HasIndex(i => i.DocumentPath)
               .IsUnique();

            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.SeedAllData();

            base.OnModelCreating(builder);
        }
    }
}