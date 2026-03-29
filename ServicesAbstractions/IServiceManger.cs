using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IServiceManger
    {
        //Signiture for each and every service
        IProductServices ProductServices { get; }
        IBasketService BasketService { get; }
        IAthenticationService AthenticationService { get; }

    }
}
