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
        public IEnumerable<string>? Errors { get; set; }
        public ErrorDetails(string message, int code,IEnumerable<string>? errors = null)
        {
            Message = message;
            Code = code;
            Errors = errors;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
