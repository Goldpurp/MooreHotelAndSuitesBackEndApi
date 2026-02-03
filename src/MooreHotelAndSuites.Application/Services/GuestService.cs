using MooreHotelAndSuites.Application.DTOs.Guests;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Services
{
   public class GuestService : IGuestService
{
    private readonly IGuestRepository _repo;

    public GuestService(IGuestRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> EnsureGuestAsync(
        string fullName,
        string? email,
        string phone)
    {
        // Normalize inputs
        email = email?.Trim().ToLower();
        fullName = fullName.Trim();
        phone = phone.Trim();

      
        if (!string.IsNullOrEmpty(email))
        {
            var existing = await _repo.FindByEmailAsync(email);
            if (existing != null)
                return existing.Id;
        }

        var existingByPhone = await _repo.FindByPhoneAsync(phone);
        if (existingByPhone != null)
            return existingByPhone.Id;

       
        var guest = new Guest
        {
            FullName = fullName,
            Email = email ?? "",
            PhoneNumber = phone
        };

        await _repo.AddAsync(guest);
        return guest.Id;
    }

    public async Task<Guest?> GetByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }
}

}
