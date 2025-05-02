using Microsoft.EntityFrameworkCore;
using Redde.Domain.Entities;
using Redde.Domain.Entities.CompanyEntities;
using Redde.Infraestructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Roles.AnyAsync())
        {
            await context.Roles.AddRangeAsync(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Owner" },
                new Role { Id = 3, Name = "Employee" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Users.AnyAsync(u => u.Email == "admin@redde.com"))
        {
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");

            var admin = new User
            {
                Name = "Super Admin",
                Email = "admin@redde.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                RoleId = adminRole.Id,
                Provider = "local"
            };

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }

        if (!await context.CompanyCategories.AnyAsync())
        {
            await context.CompanyCategories.AddRangeAsync(
                new CompanyCategory { Name = "ONG" },
                new CompanyCategory { Name = "Empresa Privada" },
                new CompanyCategory { Name = "Cooperativa" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.PaymentSchemes.AnyAsync())
        {
            await context.PaymentSchemes.AddRangeAsync(
                new PaymentScheme { Name = "Mensual" },
                new PaymentScheme { Name = "Trimestral" },
                new PaymentScheme { Name = "Por proyecto" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.CompanyStates.AnyAsync())
        {
            await context.CompanyStates.AddRangeAsync(
                new CompanyState { Name = "Activa" },
                new CompanyState { Name = "Suspendida" },
                new CompanyState { Name = "Inactiva" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.EconomicActivities.AnyAsync())
        {
            await context.EconomicActivities.AddRangeAsync(
                new EconomicActivity { Name = "Agricultura" },
                new EconomicActivity { Name = "Comercio" },
                new EconomicActivity { Name = "Tecnología" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.GovernmentBranches.AnyAsync())
        {
            await context.GovernmentBranches.AddRangeAsync(
                new GovernmentBranch { Name = "Ministerio de Salud" },
                new GovernmentBranch { Name = "Educación" },
                new GovernmentBranch { Name = "Interior" }
            );
            await context.SaveChangesAsync();
        }
    }
}
