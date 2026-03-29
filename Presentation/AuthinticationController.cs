using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class AuthinticationController(IServiceManger _serviceManger) : ApiController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>>Login(LoginDto loginDto)
        {
            var result = await _serviceManger.AthenticationService.LoginAsync(loginDto);
            return Ok(result);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>>Register(RegisterDto registerDto)
        {
            var result = await _serviceManger.AthenticationService.registerAsync(registerDto);
            return Ok(result);
        }

    }
}
