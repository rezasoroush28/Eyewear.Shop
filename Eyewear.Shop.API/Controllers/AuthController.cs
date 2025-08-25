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
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDto requestOtpDto, CancellationToken cancellationToken)
        {
            var result = await _authService.RequestOtpAsync(requestOtpDto.PhoneNumber, cancellationToken);
            if(result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto, CancellationToken cancellationToken)
        {
            var tokenResult = await _authService.VerifyOtpAsync(dto.PhoneNumber, dto.Code, cancellationToken);
            if (tokenResult.IsSuccess)
                return BadRequest(tokenResult.ErrorMessage);
            return Ok(new { tokenResult.Data });
        }

        [Authorize]
        [HttpPost("complete-profile")]
        public async Task<IActionResult> CompleteProfile([FromBody] CompleteProfileDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var result = await _authService.CompleteProfileAsync(Guid.Parse(userId), dto.Name, dto.Email, cancellationToken);
            if (result.IsSuccess)
                return BadRequest(result.ErrorMessage);
            return Ok("Profile updated.");
        }
    }
}
