using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.Models.ToDoClass.DTO;
using ToDoApp.Services;

namespace ToDoApp.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
         

        public AuthenticationController(IJwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _tokenService.LoginAsync(dto);
            if (token == null) return Unauthorized("Invalid username or password");

            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            string ans = await _tokenService.CreateUser(dto);
            return Ok(new { Message = ans, RegisteredAt = DateTime.UtcNow });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var principal = _tokenService.ValidateJwt(refreshToken);
            if (principal == null)
                return Unauthorized("Invalid refresh token");

            // ensure it's really a refresh token
            if (principal.FindFirst("token_type")?.Value != "refresh")
                return Unauthorized("Not a refresh token");

            int userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _tokenService.GetUser(userId); 
            if (user == null)
                return Unauthorized("User not found");

            var newAccessToken = _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }


    }
}
