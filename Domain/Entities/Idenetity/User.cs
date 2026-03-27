using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Idenetity
{
    public class User : IdentityUser
    {
        public string UserName { get; set; }
        public Address address { get; set; }

    }
}
