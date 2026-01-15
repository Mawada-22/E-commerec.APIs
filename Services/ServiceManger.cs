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
        public readonly Lazy<IProductServices> _productServices;
        public ServiceManger(IUnitOfWork unitOfWork,IMapper mapper) {
            _productServices = new Lazy<IProductServices>(()=> new ProductServices(unitOfWork,mapper));

        }
        IProductServices IServiceManger.ProductServices => _productServices.Value;
    }
}
