using System;
using Order.Domain.Entities.ValueObjects;

namespace Order.Domain.Entities;

public class Orders
{
    public Orders(CustomerValueObjects customer)
    {
        Id = Guid.NewGuid();
        Date = DateTime.UtcNow;
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
    }

    public Guid Id { get; }
    public string Number { get; }
    public string Status { get; }
    public DateTime Date { get; }
    public CustomerValueObjects Customer { get; }
    public IList<CartValueObject> CartItems { get; } = new List<CartValueObject>();
    public decimal TotalAmount { get; private set; }

    public void AddCartItem(CartValueObject cartItem)
    {
        CartItems.Add(cartItem);
    }
}
