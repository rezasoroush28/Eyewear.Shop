using Eyewear.Shop.API.Models.Auth;
using Eyewear.Shop.Application.Interfaces.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eyewear.Shop.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDto requestOtpDto)
        {
            await _authService.RequestOtpAsync(requestOtpDto.PhoneNumber);
            return Ok();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var token = await _authService.VerifyOtpAsync(dto.PhoneNumber, dto.Code);
            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("complete-profile")]
        public async Task<IActionResult> CompleteProfile([FromBody] CompleteProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _authService.CompleteProfileAsync(Guid.Parse(userId), dto.Name, dto.Email);
            return Ok("Profile updated.");
        }
    }
}
