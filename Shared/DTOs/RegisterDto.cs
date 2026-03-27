using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record RegisterDto
    {
        [Required(ErrorMessage ="Display name is Required")]
        public string DisplayName { get; init; }
        [Required(ErrorMessage ="UserName is Reqiuerd")]
        public string UserName { get; init; }

        [Required(ErrorMessage ="Email is Reqiuerd")]
        public string Email { get; init; }
        
        [Required(ErrorMessage ="Password is Reqiuerd")]
        public string Password { get; init; }

        public string? phoneNumber { get; init; }



    }
}
