using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.DTOs.Admin;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminManagementController : ControllerBase
    {
        private readonly IAdminManagementService _admin;

        public AdminManagementController(IAdminManagementService admin)
        {
            _admin = admin;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<AdminStatsDto>> GetStats()
            => Ok(await _admin.GetStatsAsync());

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
            => Ok(await _admin.GetEmployeesAsync());

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
            => Ok(await _admin.GetClientsAsync());

        [HttpPost("staff")]
        public async Task<IActionResult> OnboardStaff(OnboardStaffDto dto)
        {
            await _admin.OnboardStaffAsync(dto);
            return Ok();
        }

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
