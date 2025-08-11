using System;
using Marraia.Queue.Interfaces;
using Order.Core.Domain.Entities;
using Order.Core.Repositories.Interfaces;
using Order.Core.Services.Application.Interfaces;
using Order.Domain.Entities;
using Order.Domain.Entities.ValueObjects;

namespace Order.Core.Services.Application;

public class OrderAppService : IOrderAppService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventBus _eventBus;

    public OrderAppService(IOrderRepository orderRepository,
                           IProductRepository productRepository,
                           IHttpContextAccessor httpContextAccessor,
                           IEventBus eventBus)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
        _eventBus = eventBus;
    }

    public async Task<Orders> CreateOrderAsync(OrderInput orderInput)
    {
        var customer = GetLoggedCustomer();

        var order = new Orders(customer);
        InsertItemsToOrder(orderInput.CartItems, order);
        order.CreateOrderNumber();
        order.CalculateTotalAmount();
        await _orderRepository.InsertAsync(order);
        var orderMessage = new OrderMessage
        {
            CustomerName = customer.Name,
            CustomerEmail = customer.Email,
            OrderId = order.Id.ToString(),
            TotalAmount = order.TotalAmount
        };
        // TODO: messagebus from configuration
        await _eventBus.PublishAsync(orderMessage, "messagebus.payment");
        return order;
    }

    public async Task<IEnumerable<Orders>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Orders> GetOrderByNumberAsync(string orderNumber)
    {
        return await _orderRepository.GetByNumberAsync(orderNumber);
    }

    private void InsertItemsToOrder(IList<CartItemInput> cartItems, Orders order)
    {
        foreach (var item in cartItems)
        {
            var product = _productRepository.GetByIdAsync(item.ProductId).Result;
            if (product == null) continue;
            var cartItem = new CartValueObject
            {
                Id = product.Id,
                Name = product.Title,
                Price = product.Price,
                Quantity = item.Quantity
            };
            order.AddCartItem(cartItem);
        }
    }

    private CustomerValueObjects GetLoggedCustomer()
    {
        var customer = _httpContextAccessor.HttpContext?.User;
        return new CustomerValueObjects
        {
            Name = customer?.Identity?.Name ?? "",
            Email = customer?.FindFirst("name")?.Value ?? ""
        };
    }
}
