using Microsoft.EntityFrameworkCore;
using Redde.Domain.Entities;
using Redde.Domain.Entities.CompanyEntities;

namespace Redde.Infraestructure.Persistence

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies {  get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CompanyCategory> CompanyCategories { get; set; }
        public DbSet<PaymentScheme> PaymentSchemes { get; set; }
        public DbSet<CompanyState> CompanyStates { get; set; }
        public DbSet<EconomicActivity> EconomicActivities { get; set; }
        public DbSet<GovernmentBranch> GovernmentBranches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany()
                .HasForeignKey(u => u.CompanyId);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.PaymentScheme)
                .WithMany()
                .HasForeignKey(c => c.PaymentSchemeId);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.State)
                .WithMany()
                .HasForeignKey(c => c.StateId);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.EconomicActivity)
                .WithMany()
                .HasForeignKey(c => c.EconomicActivityId);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.GovernmentBranch)
                .WithMany()
                .HasForeignKey(c => c.GovernmentBranchId);
        }
    }
}
