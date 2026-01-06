using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // "Free Wi-Fi"
        public string? Icon { get; set; }                 // "wifi"
        public bool IsActive { get; set; } = true;        // can disable from admin
    }
}