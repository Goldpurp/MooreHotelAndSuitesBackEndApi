using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class MenuItem
{
    public Guid Id { get; private set; }

   public string Name { get; private set; } = null!;


    public decimal Price { get; private set; }

    public OrderSource Source { get; private set; }

    public bool IsAvailable { get; private set; }

    private MenuItem() { }

    public MenuItem(string name, decimal price, OrderSource source)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Source = source;
        IsAvailable = true;
    }
}

}