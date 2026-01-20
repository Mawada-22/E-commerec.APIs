using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IProductServices
    {
        public Task<PaginationResult<ProductDto>> GetAllProducts(ProductSpecParams Params);
        public Task<IEnumerable<TypeDto>> GetAllTypes();
        public Task<IEnumerable<BrandDto>> GetAllBrands();
        public Task<ProductDto> GetProductById(int id);
    }
}
