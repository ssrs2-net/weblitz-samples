namespace WeblitzPdf.Samples.BlazorComponentToPdf;

public sealed record OrderItem(string Sku, string Name, int Quantity, decimal Price)
{
    public decimal Amount => Quantity * Price;
}

public sealed record Order(int Id, string Customer, DateOnly Placed, string Status, IReadOnlyList<OrderItem> Items)
{
    public decimal Total => Items.Sum(i => i.Amount);
}

/// <summary>The application's orders (made up): where your own data would come from.</summary>
public sealed class Orders
{
    private readonly Dictionary<int, Order> _orders = new()
    {
        [1042] = new Order(1042, "Fenwick & Moor Logistics", new DateOnly(2026, 11, 18), "Shipped",
        [
            new OrderItem("NW-PAL-120", "Pallet labels, 120 x 80 mm (roll of 500)", 4, 38.50m),
            new OrderItem("NW-SCN-02", "Handheld scanner, Bluetooth", 2, 189.00m),
            new OrderItem("NW-TAP-48", "Packing tape, 48 mm (box of 36)", 3, 54.90m),
        ]),
    };

    public Order? Find(int id) => _orders.GetValueOrDefault(id);
}
