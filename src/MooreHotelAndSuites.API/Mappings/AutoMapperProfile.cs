using AutoMapper;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Hotel, Hotel>();
            CreateMap<Room, Room>();
            CreateMap<Guest, Guest>();
            CreateMap<Booking, Booking>();
        }
    }
}
