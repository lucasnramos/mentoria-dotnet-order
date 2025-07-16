using System;

namespace Order.Domain.Entities.ValueObjects;

public class CartValueObject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
