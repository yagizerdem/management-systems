using Common;
using DAL.Context;
using Entity.Identity;
using Microsoft.AspNetCore.Identity;

namespace Hotel_Management
{
    public class AdminSeeder
    {

        public static async Task SeedAdminAsync(IServiceProvider provider)
        {
            HotelManagementContext context = provider.GetRequiredService<HotelManagementContext>();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {

                IConfiguration configuration = provider.GetService<IConfiguration>()
                    ?? throw new AppException("Configuration not found")
                    {
                        IsOperational = false,
                        ErrorCode = ErrorCode.INVALID_CONFIGURATION,
                    };

                string adminEmail = configuration["Admin:Email"]
                    ?? throw new AppException("Admin email is not configured")
                    {
                        IsOperational = false,
                        ErrorCode = ErrorCode.INVALID_CONFIGURATION,
                    };

                string adminPassword = configuration["Admin:Password"]
                    ?? throw new AppException("Admin password is not configured")
                    {
                        IsOperational = false,
                        ErrorCode = ErrorCode.INVALID_CONFIGURATION,
                    };

                string adminFirstName = configuration["Admin:FirstName"]
                    ?? throw new AppException("Admin first name is not configured")
                    {
                        IsOperational = false,
                        ErrorCode = ErrorCode.INVALID_CONFIGURATION,
                    };

                string adminLastName = configuration["Admin:LastName"]
                    ?? throw new AppException("Admin last name is not configured")
                    {
                        IsOperational = false,
                        ErrorCode = ErrorCode.INVALID_CONFIGURATION,
                    };


                UserManager<AppUser> userManager = provider.GetRequiredService<UserManager<AppUser>>();
                RoleManager<IdentityRole<Guid>> roleManager = provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                AppUser? existingAdmin = await userManager.FindByEmailAsync(adminEmail);
                if (existingAdmin != null)
                {
                    return;
                }

                AppUser admin = new AppUser()
                {
                    Email = adminEmail,
                    FirstName = adminFirstName,
                    LastName = adminLastName,
                    UserName = adminEmail
                };

                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (!result.Succeeded)
                {
                    throw new AppException("Failed to create admin user")
                    {
                        ErrorCode = ErrorCode.UNKNOWN_ERROR,
                        IsOperational = false,
                    };
                }

                if (!await roleManager.RoleExistsAsync(Roles.Admin))
                {
                    throw new AppException("Role 'Admin' does not exist")
                    {
                        ErrorCode = ErrorCode.UNKNOWN_ERROR,
                        IsOperational = false,
                    };
                }

                result = await userManager.AddToRoleAsync(admin, Roles.Admin);
                if(!result.Succeeded)
                {
                    throw new AppException("Failed to add admin user to role 'Admin'")
                    {
                        ErrorCode = ErrorCode.UNKNOWN_ERROR,
                        IsOperational = false,
                    };
                }   

                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

    }
}
