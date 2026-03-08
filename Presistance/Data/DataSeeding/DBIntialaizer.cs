using Domain.Contarcts;
using Domain.Entities;
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

        public DBIntialaizer(StoreDbcontext dbcontext) {

           _dbcontext = dbcontext;
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
