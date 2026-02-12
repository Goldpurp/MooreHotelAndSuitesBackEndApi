using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Domain.ValueObjects
{
    public static class LaundryPriceList
    {
        public static decimal GetPrice(LaundryServiceType type)
        {
            return type switch
            {
                LaundryServiceType.Wash => 2000m,
                LaundryServiceType.Iron => 1000m,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
