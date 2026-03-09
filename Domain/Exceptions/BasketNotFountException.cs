using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class BasketNotFountException: NotFoundException
    {
        public BasketNotFountException(string id):base($"Basket With Id :{id} is not found")
        {

        }
    }
}
