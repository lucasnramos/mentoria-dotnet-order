using Marraia.Notifications.Base;
using Marraia.Notifications.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.Hosting;
using Order.Core.Services.Application;
using Order.Core.Services.Application.Interfaces;

namespace Order.Core.Services.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class Order : BaseController
    {
        private readonly IOrderAppService _orderAppService;
        public Order(INotificationHandler<DomainNotification> notification, IOrderAppService orderAppService) : base(notification)
        {
            _orderAppService = orderAppService;
        }

        // [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderInput orderInput)
        {
            var order = await _orderAppService.CreateOrderAsync(orderInput);
            return CreatedContent("/api/order", order);
        }

        [Authorize]
        [HttpGet]
        [Route("saleOrder/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderByNumberAsync([FromRoute] string orderNumber)
        {
            var order = await _orderAppService.GetOrderByNumberAsync(orderNumber);
            return OkOrNotFound(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllOrderAsync()
        {
            var orders = await _orderAppService.GetAllOrdersAsync();
            return OkOrNotFound(orders);
        }
    }
}
