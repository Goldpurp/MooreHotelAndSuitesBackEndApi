using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MooreHotelAndSuites.Infrastructure.Auth;
using MooreHotelAndSuites.Infrastructure.Identity;

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
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            // ✅ Use correct field name _userManager
            var user = await _userManager.FindByNameAsync(req.Email) 
                       ?? await _userManager.FindByEmailAsync(req.Email);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = await _jwt.GenerateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new { token, roles });
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
