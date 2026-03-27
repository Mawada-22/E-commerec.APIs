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


            return new UserResultDto(user.UserName,user.Email!,"Token");


        }

        public Task<UserResultDto> registerAsync(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }
    }
}
