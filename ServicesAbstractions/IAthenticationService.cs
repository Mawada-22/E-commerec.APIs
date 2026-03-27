using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IAthenticationService
    {
        //login & register
        public Task<UserResultDto> LoginAsync(LoginDto loginDto);
        public Task<UserResultDto> registerAsync(RegisterDto registerDto);

    }
}
