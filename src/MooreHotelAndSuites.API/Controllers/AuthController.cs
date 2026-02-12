using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MooreHotelAndSuites.Infrastructure.Auth;
using MooreHotelAndSuites.Infrastructure.Identity;
using MooreHotelAndSuites.Application.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Application.DTOs.Auth;
using MooreHotelAndSuites.Application.DTOs.Users;
using System.Security.Claims;



namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
[Route("api/users")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwt;
     private readonly IUserManagementService _users;
     private readonly IEmailService _emailService;

    public AuthController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,IUserManagementService users,
        IJwtTokenService jwt,  IEmailService emailService)
    {
        _signInManager = signInManager;
         _users = users;
        _userManager = userManager;
        _jwt = jwt;
        _emailService = emailService;
    }
     

   

   [HttpPost]
public async Task<IActionResult> Create(CreateUserDto dto)
{
   
    var user = await _users.CreateUserAsync(
    dto.Email,
    dto.FullName,
    dto.Password,
    dto.Role ?? "User",
    createdByAdminId: null 
);

  var identityUser = await _userManager.FindByEmailAsync(dto.Email);
if (identityUser == null)
    throw new InvalidOperationException($"User with email {dto.Email} was not created properly.");

var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

    var link = Url.Action(
        nameof(ConfirmEmail),
        "Auth",
        new { userId = identityUser.Id, token },
        Request.Scheme);

    await _emailService.SendAsync(
        dto.Email,
        "Confirm your account",
        $"Welcome. Confirm your account by clicking this link: {link}"
    );

    return Ok(new
    {
        message = "User created. Confirmation email sent",
        email = dto.Email,
        role = dto.Role ?? "User"
    });
}



    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid credentials");

        if (!user.EmailConfirmed)
            return Unauthorized("Email not confirmed");

        if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
            return Unauthorized("Account is locked");

        var tokens = await _jwt.GenerateTokensAsync(user);

        // Save refresh token in DB
        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // or whatever policy
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            accessToken = tokens.AccessToken,
            refreshToken = tokens.RefreshToken
        });
    }
[HttpPost("send-confirmation-email")]
public async Task<IActionResult> SendConfirmationEmail(string email)
{
  var user = await _userManager.FindByEmailAsync(email);
if (user == null)
    return BadRequest("User not found");


if (string.IsNullOrWhiteSpace(user.Email))
    return BadRequest("User does not have a valid email address");


var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

    var confirmationLink = Url.Action(
        nameof(ConfirmEmail),
        "Auth",
        new { userId = user.Id, token },
        Request.Scheme);

    await _emailService.SendAsync(
        user.Email,
        "Confirm your email",
        $"Click here: {confirmationLink}"
    );

    return Ok("Confirmation email sent");
}


    [HttpGet("confirm-email")]
public async Task<IActionResult> ConfirmEmail(string userId, string token)
{
    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        return BadRequest("Invalid confirmation request");

    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
        return BadRequest("User not found");

    var result = await _userManager.ConfirmEmailAsync(user, token);

    if (!result.Succeeded)
        return BadRequest("Email confirmation failed");

    return Ok("Email confirmed successfully");
}


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(TokenRefreshDto model)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == model.RefreshToken);

        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return Unauthorized("Invalid or expired refresh token");

        // Generate new tokens
        var newTokens = await _jwt.GenerateTokensAsync(user);

        // Rotate refresh token
        user.RefreshToken = newTokens.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            accessToken = newTokens.AccessToken,
            refreshToken = newTokens.RefreshToken
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Ok();

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.MinValue;
        await _userManager.UpdateAsync(user);

        return Ok("Logged out");
    }
}

    }


