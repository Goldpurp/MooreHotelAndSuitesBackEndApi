using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Infrastructure.Identity;
using System.Security.Claims;
using MooreHotelAndSuites.Application.Interfaces.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/account")]
[Authorize] // staff must be logged in
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    public AccountController(UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

       
        user.MustChangePassword = false;
        await _userManager.UpdateAsync(user);

        return Ok(new { message = "Password changed successfully" });
    }
    
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Ok(); // don't reveal whether email exists

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetLink = $"https://yourapp.com/reset-password?email={dto.Email}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendAsync(dto.Email, "Password Reset", $"Reset your password using this link: {resetLink}");

        return Ok(new { message = "If the email exists, a reset link has been sent" });
    }


    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return BadRequest("Invalid request");

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Optionally force MustChangePassword to false
        user.MustChangePassword = false;
        await _userManager.UpdateAsync(user);

        return Ok(new { message = "Password has been reset successfully" });
    }
}

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
public class ForgotPasswordDto
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
