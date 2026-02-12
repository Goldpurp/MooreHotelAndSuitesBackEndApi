using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Domain.ValueObjects;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }

        public Guid OrderId { get; private set; }

        public string MenuName { get; private set; } = null!;

        public decimal UnitPrice { get; private set; }

        public int Quantity { get; private set; }

        public string? Description { get; private set; }

        public decimal Total => UnitPrice * Quantity;

        private OrderItem() { }

        // kitchen/bar
        public OrderItem(MenuItem menu, int quantity)
        {
            Id = Guid.NewGuid();
            MenuName = menu.Name;
            UnitPrice = menu.Price;
            Quantity = quantity;
        }

        // laundry
        public static OrderItem FromLaundry(
            LaundryServiceType type,
            int quantity,
            string description)
        {
            return new OrderItem
            {
                Id = Guid.NewGuid(),
                MenuName = $"Laundry - {type}",
                UnitPrice = LaundryPriceList.GetPrice(type),
                Quantity = quantity,
                Description = description
            };
        }
    }
}
