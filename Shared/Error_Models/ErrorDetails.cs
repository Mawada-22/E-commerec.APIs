using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Error_Models
{
    public class ErrorDetails
    {
        public string Message { get; set; } 
        public int Code { get; set; }
        public ErrorDetails(string message, int code)
        {
            Message = message;
            Code = code;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
