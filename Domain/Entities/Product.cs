using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product:ModelBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }

        public decimal Price { get; set; }

        //navigtional property for product brand + its FK
        public ProductBrand productBrand { get; set; }

        public int ProductBrandId { get; set; }

        //navigtional property for product type+ its FK

        public ProductType productType { get; set; }    
        public int ProductTypeId { get; set; }

    }
}
