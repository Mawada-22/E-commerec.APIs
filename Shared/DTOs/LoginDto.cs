using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public  record LoginDto
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; init; }
        [Required]
        public string Password { get; init; }  

        

    }
}
