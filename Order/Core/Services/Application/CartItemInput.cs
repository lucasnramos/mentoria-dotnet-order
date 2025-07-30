using System;

namespace Order.Core.Services.Application;

public class CartItemInput
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
