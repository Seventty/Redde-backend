using Redde.Domain.Entities;
using Redde.Infraestructure.Persistence;

namespace Redde.Infrastructure.Seeders
{
    public class DbSeeder
    {
        public static async Task SeedRolesAsync(ApplicationDbContext context)
        {
            if(!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }
    }
}
