using AutoMapper;
using Domain.Contarcts;
using ServicesAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManger : IServiceManger
    {
        private readonly Lazy<IProductServices> _productServices;
        private readonly Lazy<IBasketService> _basketService;

        public ServiceManger(IUnitOfWork unitOfWork,IMapper mapper,IBasketRepo basketRepo) {
            _productServices = new Lazy<IProductServices>(()=> new ProductServices(unitOfWork,mapper));
            _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepo, mapper));

        }
        IProductServices IServiceManger.ProductServices => _productServices.Value;
        IBasketService IServiceManger.BasketService => _basketService.Value;
    }
}
