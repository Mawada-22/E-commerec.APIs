using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class BasketNotFoundException: NotFoundException
    {
        public BasketNotFoundException(string id):base($"Basket With Id :{id} is not found")
        {

        }
    }
}
