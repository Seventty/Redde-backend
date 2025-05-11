using Microsoft.EntityFrameworkCore;
using Redde.Domain.Entities;
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
    }
}
