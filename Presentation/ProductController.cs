
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{        
    [ApiController]
    [Route("/[controller]")] //baseurl \controller
    public class ProductController(IServiceManger _serviceManger): ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(string? sort,int? BrandId,int? TypeId)
        {
            var Products = await _serviceManger.ProductServices.GetAllProducts(sort, BrandId, TypeId);
            return Ok(Products);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrandss()
        {
            var Brands =await  _serviceManger.ProductServices.GetAllBrands();
            return Ok(Brands);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var Tyeps = await _serviceManger.ProductServices.GetAllTypes();
            return Ok(Tyeps);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var Product =await _serviceManger.ProductServices.GetProductById(id);
            return Ok(Product);
        }
    }
}
