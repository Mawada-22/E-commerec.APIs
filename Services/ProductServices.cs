using AutoMapper;
using Domain.Contarcts;
using Domain.Entities;
using ServicesAbstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductServices(IUnitOfWork _unitOfWork,IMapper _mapper) : IProductServices
    {
        //1-get data from unit of work 
        //2- map to dto return value
        //3- return teh reslut 
        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            //1- 
            var products = await _unitOfWork.GetRepo<int, Product>().GatAllAsync();
            //2-
            var ProducstResult = _mapper.Map<IEnumerable<ProductDto>>(products);
            //3-
            return ProducstResult;

        }

        public async Task<IEnumerable<BrandDto>> GetAllrands()
        {
            //1- 
            var Brands = await _unitOfWork.GetRepo<int, ProductBrand>().GatAllAsync();
            //2-
            var BrandstResult = _mapper.Map<IEnumerable<BrandDto>>(Brands);
            //3-
            return BrandstResult;
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypes()
        {
            //1- 
            var Types = await _unitOfWork.GetRepo<int, ProductType>().GatAllAsync();
            //2-
            var TypestResult = _mapper.Map<IEnumerable<TypeDto>>(Types);
            //3-
            return TypestResult;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            //1-
            var Product = await _unitOfWork.GetRepo<int,Product>().GetByIdAsync(id);
            //2-
            var ProductResult = _mapper.Map<ProductDto>(Product);
            //3-
            return ProductResult;
        }
    }
}
