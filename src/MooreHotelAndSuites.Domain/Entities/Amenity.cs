using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MooreHotelAndSuites.Domain.Entities
{
     public class Amenity
    {
        public Guid Id { get; set; }
       public string Name { get; set; } = string.Empty;


       
        public ICollection<RoomAmenity> RoomAmenities { get; set; } 
            = new List<RoomAmenity>();
    }
}