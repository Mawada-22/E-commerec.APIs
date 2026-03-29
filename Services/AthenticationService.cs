using Domain.Entities.Idenetity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using ServicesAbstractions;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AthenticationService(UserManager<User> _userManager) : IAthenticationService
    {
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


            return new UserResultDto(user.DisplayName,user.Email!,"Token");


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
            return new UserResultDto(registerDto.DisplayName, registerDto.Email!, "Token");
        }
    }
}
