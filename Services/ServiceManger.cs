using Microsoft.Extensions.DependencyInjection;
using ServicesAbstractions;
using System;

namespace Services
{
    public class ServiceManger : IServiceManger
    {
        private readonly Lazy<IProductServices> _productServices;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAthenticationService> _athenticationService;
        private readonly Lazy<IOrderService> _orderService;

        public ServiceManger(IServiceProvider serviceProvider)
        {
            _productServices = new Lazy<IProductServices>(serviceProvider.GetRequiredService<IProductServices>);
            _basketService = new Lazy<IBasketService>(serviceProvider.GetRequiredService<IBasketService>);
            _athenticationService = new Lazy<IAthenticationService>(serviceProvider.GetRequiredService<IAthenticationService>);
            _orderService = new Lazy<IOrderService>(serviceProvider.GetRequiredService<IOrderService>);
        }

        public IProductServices ProductServices => _productServices.Value;
        public IBasketService BasketService => _basketService.Value;
        public IAthenticationService AthenticationService => _athenticationService.Value;
        public IOrderService OrderService => _orderService.Value;
    }
}
