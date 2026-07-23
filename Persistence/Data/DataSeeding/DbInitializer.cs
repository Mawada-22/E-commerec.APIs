using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Data.DataSeeding
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _dbcontext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly string _dataSeedingPath;
        private readonly IConfiguration _configuration;

        public DbInitializer(StoreDbContext dbcontext,RoleManager<IdentityRole>roleManager, UserManager<User>userManager, IHostEnvironment hostEnvironment, IConfiguration configuration) {

           _dbcontext = dbcontext;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            //ContentRootPath is stable regardless of how/where the process is launched from (IIS, Docker, dotnet publish, etc.),
            //unlike the process current-directory that the previous relative path relied on.
            _dataSeedingPath = Path.GetFullPath(Path.Combine(hostEnvironment.ContentRootPath, "..", "Persistence", "Data", "DataSeeding"));
        }

        public async Task IdentityInitializeAsync()
        {
            //Seed defalute user and Role
            //1- Seed Roles
            if (!_roleManager.Roles.Any())
            {
                //Admin and Superadmin
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
               await  _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            //2-Seed Users with roles
            if (!_userManager.Users.Any())
            {
                // Never a hardcoded password: comes from user-secrets/environment
                // (SeedUsers:Password). Without it, user seeding is skipped - the
                // app still runs, it just has no default admin accounts.
                var seedPassword = _configuration["SeedUsers:Password"];
                if (string.IsNullOrWhiteSpace(seedPassword))
                {
                    Console.WriteLine("SeedUsers:Password is not configured - skipping default Admin/SuperAdmin seeding.");
                    return;
                }

                var adminUser = new User
                {
                    UserName = "Admin",
                    DisplayName="Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01234523",
                    EmailConfirmed = true
                };
                var superAdminUser = new User
                {
                    UserName = "SuperAdmin",
                    DisplayName="SuperAdmin",
                    Email = "superadmin@gmail.com",
                    PhoneNumber = "01112821822",
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(adminUser, seedPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        Console.WriteLine(error.Description);
                }
                await _userManager.CreateAsync(superAdminUser, seedPassword);
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");              
            }
            
        }
        public async Task InitializeAsync()
        {
            try
            {
                //creates the database if there is not
                if (_dbcontext.Database.GetPendingMigrations().Any())
                    await _dbcontext.Database.MigrateAsync();

                //seeding the data into tables

                //types:
                var typesData = await File.ReadAllTextAsync(Path.Combine(_dataSeedingPath, "types.json"));

                //2-convert into C# objects

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                
                //3-add the data into it's table

                if(types is not null && types.Any() && !_dbcontext.ProductTypes.Any())
                {
                    await _dbcontext.ProductTypes.AddRangeAsync(types);
                    await _dbcontext.SaveChangesAsync();    
                }

                //Brands:
                var brandsData = await File.ReadAllTextAsync(Path.Combine(_dataSeedingPath, "brands.json"));

                //2-convert into C# objects

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                //3-add the data into it's table

                if (brands is not null && brands.Any() && !_dbcontext.ProductBrands.Any())
                {
                    await _dbcontext.ProductBrands.AddRangeAsync(brands);
                    await _dbcontext.SaveChangesAsync();
                }


                //Products:
                var ProductsData = await File.ReadAllTextAsync(Path.Combine(_dataSeedingPath, "products.json"));

                //2-convert into C# objects

                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                //3-add the data into it's table

                if (products is not null && products.Any() && !_dbcontext.Products.Any())
                {
                    await _dbcontext.Products.AddRangeAsync(products);
                    await _dbcontext.SaveChangesAsync();
                }

                //DeliveryMethods:
                var deliveryData = await File.ReadAllTextAsync(Path.Combine(_dataSeedingPath, "delivery.json"));

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                if (deliveryMethods is not null && deliveryMethods.Any() && !_dbcontext.DeliveryMethods.Any())
                {
                    await _dbcontext.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _dbcontext.SaveChangesAsync();
                }

            }
            catch (Exception) 
            
            {
                throw;
            }

        }
    }
}
