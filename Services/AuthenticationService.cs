using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ServicesAbstractions;
using Shared;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Services
{
    public class AuthenticationService: IAuthenticationService
    {

        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly AutoMapper.IMapper _mapper;

        public AuthenticationService(UserManager<User> userManager,IOptions<JwtOptions> jwtOptions, AutoMapper.IMapper mapper)
        {
            this._userManager = userManager;
            this._jwtOptions = jwtOptions.Value;
            this._mapper = mapper;
        }

        public async Task<UserResultDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new UnAuthorizedException();
            return new UserResultDto(user.DisplayName, user.Email!, await CreateTokenAsync(user));
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
            => await _userManager.FindByEmailAsync(email) is not null;

        public async Task<AddressDto?> GetUserAddressAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new UnAuthorizedException();
            // UserManager doesn't load navigations; project the address straight
            // from the store's queryable instead.
            var address = _userManager.Users
                .Where(u => u.Id == user.Id)
                .Select(u => u.address)
                .FirstOrDefault();
            return address is null ? null : _mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new UnAuthorizedException();

            var existing = _userManager.Users
                .Where(u => u.Id == user.Id)
                .Select(u => u.address)
                .FirstOrDefault();

            if (existing is null)
            {
                user.address = _mapper.Map<Address>(addressDto);
            }
            else
            {
                existing.FirstName = addressDto.FirstName;
                existing.LastName = addressDto.LastName;
                existing.Street = addressDto.Street;
                existing.City = addressDto.City;
                existing.Country = addressDto.Country;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new UserValidationException(result.Errors.Select(e => e.Description).ToList());

            return addressDto;
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new UnAuthorizedException($"No account is registered with {email}.");

            // NOTE: in production this token should be EMAILED to the user inside
            // a reset link - never returned from the API. Returned directly here
            // because no SMTP sender is configured yet (dev-only shortcut).
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email)
                ?? throw new UnAuthorizedException($"No account is registered with {resetPasswordDto.Email}.");

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (!result.Succeeded)
                throw new UserValidationException(result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<string> CreateTokenAsync(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Name,user.UserName!),
            new Claim(ClaimTypes.Email,user.Email!)
        };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtOptions.DurationInDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            //cheak on emeil existing
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) { 
            throw new UnAuthorizedException($"this email : {loginDto.Email} is not found");
            }
            //cheak password 
            var result =await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                throw new UnAuthorizedException();
            // generate token
            var token = await CreateTokenAsync(user);

            

            return new UserResultDto(user.DisplayName,user.Email!, token);


        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.phoneNumber,
                UserName = registerDto.UserName ?? registerDto.Email
            };
            var result=await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new UserValidationException(errors);
            }
            var token = await CreateTokenAsync(user);
            return new UserResultDto(registerDto.DisplayName, registerDto.Email!, token);
        }
    }
}
