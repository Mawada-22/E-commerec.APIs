using Microsoft.Extensions.DependencyInjection;
using ServicesAbstractions;
using System;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductServices> _productServices;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IPaymentService> _paymentService;

        public ServiceManager(IServiceProvider serviceProvider)
        {
            _paymentService = new Lazy<IPaymentService>(serviceProvider.GetRequiredService<IPaymentService>);
            _productServices = new Lazy<IProductServices>(serviceProvider.GetRequiredService<IProductServices>);
            _basketService = new Lazy<IBasketService>(serviceProvider.GetRequiredService<IBasketService>);
            _authenticationService = new Lazy<IAuthenticationService>(serviceProvider.GetRequiredService<IAuthenticationService>);
            _orderService = new Lazy<IOrderService>(serviceProvider.GetRequiredService<IOrderService>);
        }

        public IProductServices ProductServices => _productServices.Value;
        public IBasketService BasketService => _basketService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IOrderService OrderService => _orderService.Value;
        public IPaymentService PaymentService => _paymentService.Value;
    }
}
