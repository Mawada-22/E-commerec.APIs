using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class unAuthorizedException(string message = "Invalid email or Password") : Exception(message)
    {
    }
}
