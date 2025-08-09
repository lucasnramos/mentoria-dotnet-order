using System;
using Marraia.MongoDb.Repositories;
using Marraia.MongoDb.Repositories.Interfaces;
using MongoDB.Driver;
using Order.Core.Repositories.Interfaces;
using Order.Domain.Entities;

namespace Order.Core.Repositories;

public class OrderRepository : MongoDbRepositoryBase<Orders, Guid>, IOrderRepository
{
    public OrderRepository(IMongoContext context) : base(context)
    {

    }

    public Task<Orders> GetByNumberAsync(string orderNumber)
    {
        return Collection
            .Find(x => x.Number == orderNumber)
            .FirstOrDefaultAsync();
    }
}