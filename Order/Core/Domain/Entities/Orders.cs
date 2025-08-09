using System;
using Marraia.MongoDb.Core;
using MongoDB.Bson.Serialization.Attributes;
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

    public string Number { get; private set; }
    public string Status { get; private set; }
    public DateTime Date { get; private set; }

    [BsonElement("Customer")]
    public CustomerValueObjects Customer { get; private set; }

    [BsonElement("CartItems")]
    public IList<CartValueObject> CartItems { get; private set; } = new List<CartValueObject>();
    public decimal TotalAmount { get; private set; }

    public void AddCartItem(CartValueObject cartItem)
    {
        CartItems.Add(cartItem);
    }

    public void CreateOrderNumber()
    {
        var id = Id.ToString("N").Split('-').Last();
        Number = $"ORD-{Date:yyyyMMddHHmm}-{id}";
        Status = "Pending";
    }

    public void CalculateTotalAmount()
    {
        TotalAmount = CartItems.Sum(item => item.Price * item.Quantity);
    }
}
