using AutoMapper;
using Domain.Contarcts;
using Domain.Entities.Idenetity;
using Microsoft.AspNetCore.Identity;
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
        private readonly Lazy<IAthenticationService> _athenticationService;
        

        public ServiceManger(IUnitOfWork unitOfWork,IMapper mapper,IBasketRepo basketRepo,UserManager<User> userManager) {
            _productServices = new Lazy<IProductServices>(()=> new ProductServices(unitOfWork,mapper));
            _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepo, mapper));
            _athenticationService = new Lazy<IAthenticationService>(() => new AthenticationService(userManager));

        }
        IProductServices IServiceManger.ProductServices => _productServices.Value;
        IBasketService IServiceManger.BasketService => _basketService.Value;
        IAthenticationService IServiceManger.AthenticationService => _athenticationService.Value;
    }

}
