using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.DTOs.Admin;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Constants;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = Roles.Admin)]  
    public class AdminManagementController : ControllerBase
    {
        private readonly IAdminManagementService _admin;

        public AdminManagementController(IAdminManagementService admin)
        {
            _admin = admin;
        }


        // DASHBOARD


        [HttpGet("stats")]
        public async Task<ActionResult<AdminStatsDto>> GetStats()
        {
            var stats = await _admin.GetStatsAsync();
            return Ok(stats);
        }


        // USERS & STAFF


        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
        {
            var users = await _admin.GetEmployeesAsync();
            return Ok(users);
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            var guests = await _admin.GetGuestsAsync();
            return Ok(guests);
        }


        // STAFF ONBOARDING


        [HttpPost("staff")]
        public async Task<IActionResult> OnboardStaff([FromBody] OnboardStaffDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

         var allowed = Roles.StaffRoles;

    if (!allowed.Contains(dto.Role))
        return BadRequest($"Role must be one of: {string.Join(", ", allowed)}");

          
            await _admin.OnboardStaffAsync(dto);

            return Ok(new
            {
                message = "Staff account created successfully",
                role = dto.Role,
                email = dto.Email
            });
        }


        // ACCOUNT MANAGEMENT


        [HttpPost("activate/{userId}")]
        public async Task<IActionResult> Activate(string userId)
        {
            await _admin.ActivateAccountAsync(userId);
            return NoContent();
        }

        [HttpPost("deactivate/{userId}")]
        public async Task<IActionResult> Deactivate(string userId)
        {
            await _admin.DeactivateAccountAsync(userId);
            return NoContent();
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            await _admin.DeleteAccountAsync(userId);
            return NoContent();
        }
    }
}
