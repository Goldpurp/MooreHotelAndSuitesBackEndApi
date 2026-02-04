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
     public async Task<Guest?> FindByNameAsync(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return null;

        return await _repo.FindByNameAsync(fullName.Trim());
    }

    public async Task<Guest?> FindByPhoneAsync(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        return await _repo.FindByPhoneAsync(phone.Trim());
    }
   public async Task<int> EnsureGuestAsync(
    string fullName,
    string email,
    string phone)
{
    
    var byPhone = await _repo.FindByPhoneAsync(phone);
    if (byPhone != null)
        return byPhone.Id;

    
    var byName = await _repo.FindByNameAsync(fullName);
    if (byName != null)
        return byName.Id;

  
    var guest = new Guest(fullName.Trim(), email.Trim(), phone.Trim());

    await _repo.AddAsync(guest);

    return guest.Id;
}

    public async Task<Guest?> GetByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }
}

}
