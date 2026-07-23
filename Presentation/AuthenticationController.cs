using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class AuthenticationController(IServiceManager _serviceManger) : ApiController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>>Login(LoginDto loginDto)
        {
            var result = await _serviceManger.AuthenticationService.LoginAsync(loginDto);
            return Ok(result);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>>Register(RegisterDto registerDto)
        {
            var result = await _serviceManger.AuthenticationService.RegisterAsync(registerDto);
            return Ok(result);
        }

        // Storefront app-load: rehydrates the signed-in user (with a fresh token)
        // from the persisted JWT.
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _serviceManger.AuthenticationService.GetCurrentUserAsync(email!);
            return Ok(result);
        }

        // Step 1 of password reset: issues the reset token. Dev-only behavior:
        // the token is returned in the response because no email sender is wired
        // yet - in production it must be emailed as a link instead.
        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPassword([FromQuery] string email)
        {
            var token = await _serviceManger.AuthenticationService.ForgotPasswordAsync(email);
            return Ok(new { email, token });
        }

        // Step 2: consumes the token and sets the new password.
        [HttpPost("resetpassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            await _serviceManger.AuthenticationService.ResetPasswordAsync(resetPasswordDto);
            return Ok();
        }

        // Async validator on the register form.
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
            => Ok(await _serviceManger.AuthenticationService.CheckEmailExistsAsync(email));

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManger.AuthenticationService.GetUserAddressAsync(email!);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _serviceManger.AuthenticationService.UpdateUserAddressAsync(email!, addressDto);
            return Ok(result);
        }
    }
}
