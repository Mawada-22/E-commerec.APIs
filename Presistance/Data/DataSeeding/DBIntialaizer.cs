using Domain.Contarcts;
using Domain.Entities;
using Domain.Entities.Idenetity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistance.Data.DataSeeding
{
    public class DBIntialaizer : IDbInitializer
    {
        private readonly StoreDbcontext _dbcontext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DBIntialaizer(StoreDbcontext dbcontext,RoleManager<IdentityRole>roleManager, UserManager<User>userManager) {

           _dbcontext = dbcontext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task IdenetityIntialaizerAsync()
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
                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01234523",
                    EmailConfirmed = true
                };
                var superAdminUser = new User
                {
                    UserName = "SuperAdmin",
                    Email = "superadmin@gmail.com",
                    PhoneNumber = "01112821822",
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(adminUser, "Mw@12345");
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        Console.WriteLine(error.Description);
                }
                await _userManager.CreateAsync(superAdminUser,"Mw@12345");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");              
            }
            
        }
        public async Task IntialaizerAsync()
        {
            try
            {
                //creates the database if there is not
                if (_dbcontext.Database.GetPendingMigrations().Any())
                    await _dbcontext.Database.MigrateAsync();

                //seeding the data into tables

                //types:
                //1-read files as string  C:\Users\lenovo\Desktop\C#\E-commerecSolution\Presistance\Data\DataSeeding\types (1).json

                var typesData = await File.ReadAllTextAsync(@"..\Presistance\Data\DataSeeding\types (1).json");

                //2-convert into C# objects

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                
                //3-add the data into it's table

                if(types is not null && types.Any() && !_dbcontext.ProductTypes.Any())
                {
                    await _dbcontext.ProductTypes.AddRangeAsync(types);
                    await _dbcontext.SaveChangesAsync();    
                }

                //Brands:
                //1-read files as string  C:\Users\lenovo\Desktop\C#\E-commerecSolution\Presistance\Data\DataSeeding\types (1).json

                var brandsData = await File.ReadAllTextAsync(@"..\Presistance\Data\DataSeeding\brands (1).json");

                //2-convert into C# objects

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                //3-add the data into it's table

                if (brands is not null && brands.Any() && !_dbcontext.ProductBrands.Any())
                {
                    await _dbcontext.ProductBrands.AddRangeAsync(brands);
                    await _dbcontext.SaveChangesAsync();
                }


                //Products:
                //1-read files as string  C:\Users\lenovo\Desktop\C#\E-commerecSolution\Presistance\Data\DataSeeding\types (1).json

                var ProductsData = await File.ReadAllTextAsync(@"..\Presistance\Data\DataSeeding\products (1).json");

                //2-convert into C# objects

                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                //3-add the data into it's table

                if (products is not null && products.Any() && !_dbcontext.Products.Any())
                {
                    await _dbcontext.Products.AddRangeAsync(products);
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
