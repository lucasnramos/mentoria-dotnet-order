using System;

namespace Order.Core.Domain.Entities;

public class OrderMessage
{

    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string OrderId { get; set; }
    public decimal TotalAmount { get; set; }
}
