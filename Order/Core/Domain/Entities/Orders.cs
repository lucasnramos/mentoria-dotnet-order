using System;
using Marraia.MongoDb.Core;
using Order.Domain.Entities.ValueObjects;

namespace Order.Domain.Entities;

public class Orders : Entity<Guid>
{
    public Orders(CustomerValueObjects customer)
    {
        Id = Guid.NewGuid();
        Date = DateTime.UtcNow;
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
    }

    public Guid Id { get; }
    public string Number { get; private set; }
    public string Status { get; }
    public DateTime Date { get; }
    public CustomerValueObjects Customer { get; }
    public IList<CartValueObject> CartItems { get; } = new List<CartValueObject>();
    public decimal TotalAmount { get; private set; }

    public void AddCartItem(CartValueObject cartItem)
    {
        CartItems.Add(cartItem);
    }

    public void CreateOrderNumber()
    {
        var id = Id.ToString("N").Split('-').Last();
        Number = $"ORD-{Date:yyyyMMddHHmmss}-{id}";
    }

    public void CalculateTotalAmount()
    {
        TotalAmount = CartItems.Sum(item => item.Price * item.Quantity);
    }
}
