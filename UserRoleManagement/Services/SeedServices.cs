using Microsoft.AspNetCore.Identity;
using UserRoleManagement.Data;

namespace UserRoleManagement.Services
{
    public class SeedServices
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService <UserManager<Users>> ();
            var logger = scope.ServiceProvider.GetRequiredService <ILogger<SeedServices>> ();
            try
            {
                //Ensure Database is ready
                logger.LogInformation("Ensuring Database is created");
                await context.Database.EnsureCreatedAsync();

                //Add roles
                logger.LogInformation("Seeding roles");
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "User");

                //Add admin user
                logger.LogInformation("Seeding admin user");
                var adminEmail = "admin@mts.com";
                if(await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new Users
                    {
                        FullName = "Vicky B N",
                        UserName = adminEmail,
                        NormalizedUserName = adminEmail.ToUpper(),
                        Email = adminEmail,
                        NormalizedEmail = adminEmail.ToUpper(),
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var result = await userManager.CreateAsync(adminUser, "Vicky@123");
                    if(result.Succeeded)
                    {
                        logger.LogInformation("Assigning Admin role to the admin user");
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        logger.LogInformation("Failed to create admin user:{Errors}", string.Join(",", result.Errors.Select(e => e.Description)));
                    }

                }
            }
            catch (Exception ex)
            {

                logger.LogInformation(ex,"Am error occured while seeding the database");
            }
        }

        public static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if(!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role '{roleName}':{string.Join(", ",result.Errors.Select(e=>e.Description))}");
                }
            }
        }
    }
}
