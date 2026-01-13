using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Application.Interfaces.Repositories;

namespace MooreHotelAndSuites.Application.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _repo;
        public BookingService(IBookingRepository repo) => _repo = repo;
        public Task<Booking?> GetAsync(Guid id) => _repo.GetByIdAsync(id);
        public Task CreateAsync(Booking b) => _repo.AddAsync(b);
    }
}
