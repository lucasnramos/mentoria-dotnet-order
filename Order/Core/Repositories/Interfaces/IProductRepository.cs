using System;
using Order.Core.Domain.Entities;

namespace Order.Core.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);
}
