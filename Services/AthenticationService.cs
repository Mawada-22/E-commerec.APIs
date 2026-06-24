using Domain.Entities.Idenetity;
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
    public class AthenticationService: IAthenticationService
    {

        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;

        public AthenticationService(UserManager<User> userManager,IOptions<JwtOptions> jwtOptions)
        {
            this._userManager = userManager;
            this._jwtOptions = jwtOptions.Value;
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
            var user = await _userManager.FindByEmailAsync(loginDto.EmailAddress);
            if (user == null) { 
            throw new unAuthorizedException($"this email : {loginDto.EmailAddress} is not found");
            }
            //cheak password 
            var result =await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                throw new unAuthorizedException();
            // generate token
            var token = await CreateTokenAsync(user);

            

            return new UserResultDto(user.DisplayName,user.Email!, token);


        }

        public async Task<UserResultDto> registerAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.phoneNumber,
                UserName = registerDto.UserName
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
