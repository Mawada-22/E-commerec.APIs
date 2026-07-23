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
        // Optional - the storefront's register form doesn't send one; the service
        // falls back to the email as the username.
        public string? UserName { get; init; }

        [Required(ErrorMessage ="Email is Reqiuerd")]
        public string Email { get; init; }
        
        [Required(ErrorMessage ="Password is Reqiuerd")]
        public string Password { get; init; }

        public string? phoneNumber { get; init; }



    }
}
