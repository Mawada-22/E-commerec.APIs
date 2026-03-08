using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Error_Models
{
    public class ValidationErrorResponse
    {
        public int stateCode { get; set; }
        public string message { get; set; } 
        public IEnumerable<ValidationError> Errors { get; set; }

    }

}
