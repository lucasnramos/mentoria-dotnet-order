using System;

namespace Order.Core.Services.Application;

public class OrderInput
{
    public IList<CartItemInput> CartItems { get; set; }
}
