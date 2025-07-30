using System;
using Marraia.MongoDb.Repositories.Interfaces;
using Order.Domain.Entities;

namespace Order.Core.Repositories.Interfaces;

public interface IOrderRepository : IRepositoryBase<Orders, Guid>
{
    Task<Orders> GetByNumberAsync(string orderNumber);
}
