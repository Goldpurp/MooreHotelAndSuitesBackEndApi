using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MooreHotelAndSuites.Infrastructure.Auth;
using MooreHotelAndSuites.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Application.DTOs.Auth;


namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwt;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IJwtTokenService jwt)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwt = jwt;
        }
[HttpPost("login")]
public async Task<IActionResult> Login(LoginDto model)
{
    var user = await _userManager.FindByNameAsync(model.Username);
    if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        return Unauthorized("Invalid credentials");

    var tokens = await _jwt.GenerateTokensAsync(user);

    return Ok(new
    {
        accessToken = tokens.AccessToken,
        refreshToken = tokens.RefreshToken
    });
}
[HttpPost("refresh")]

public async Task<IActionResult> Refresh(TokenRefreshDto model)
{
    var user = await _userManager.Users
        .FirstOrDefaultAsync(u => u.RefreshToken == model.RefreshToken);

    if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        return Unauthorized("Invalid or expired refresh token");

    var newAccessToken = await _jwt.GenerateAccessTokenAsync(user);

    return Ok(new
    {
        accessToken = newAccessToken
    });
}

          
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Ok();

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return Ok("Logged out");
        }
    }

    
}
