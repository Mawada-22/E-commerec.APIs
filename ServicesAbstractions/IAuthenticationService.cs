using Shared.DTOs;
using Domain.Entities.Identity;

namespace ServicesAbstractions
{
    public interface IAuthenticationService
    {
        //login & register
        public Task<UserResultDto> LoginAsync(LoginDto loginDto);
        public Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        Task<string> CreateTokenAsync(User user);

        //storefront support: reload the signed-in user (fresh token), async email
        //check for the register form, and the user's saved shipping address
        Task<UserResultDto> GetCurrentUserAsync(string email);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<AddressDto?> GetUserAddressAsync(string email);
        Task<AddressDto> UpdateUserAddressAsync(string email, AddressDto addressDto);

        //forgot/reset password: issue an Identity reset token, then consume it
        Task<string> ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }

}

