using System;

namespace Order.Domain.Entities.ValueObjects;

public class CustomerValueObjects
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
