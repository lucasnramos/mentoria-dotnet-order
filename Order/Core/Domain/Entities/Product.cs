using System;

namespace Order.Core.Domain.Entities;

public class Product
{

    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string ThumbnailUrl { get; set; }

}
