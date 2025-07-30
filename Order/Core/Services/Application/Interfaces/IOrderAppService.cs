using System;
using Order.Domain.Entities;

namespace Order.Core.Services.Application.Interfaces;

public interface IOrderAppService
{
    Task<Orders> CreateOrderAsync(OrderInput orderInput);
    Task<Orders> GetOrderByNumberAsync(string orderNumber);
}
