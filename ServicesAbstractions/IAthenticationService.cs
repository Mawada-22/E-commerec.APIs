using Shared.DTOs;
using Domain.Entities.Idenetity;

namespace ServicesAbstractions
{
    public interface IAthenticationService
    {
        //login & register
        public Task<UserResultDto> LoginAsync(LoginDto loginDto);
        public Task<UserResultDto> registerAsync(RegisterDto registerDto);
        Task<string> CreateTokenAsync(User user);
    }

}

