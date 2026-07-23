using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserValidationException :  Exception
    {
        public IEnumerable<string> _Errors { get; set; } 
        public UserValidationException(IEnumerable<string> Errors) : base("One or more validation errors occurred.")
        {
            _Errors = Errors;
        }
    }
}
