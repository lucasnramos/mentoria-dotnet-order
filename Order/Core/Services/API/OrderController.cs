using Marraia.Notifications.Base;
using Marraia.Notifications.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderInput orderInput)
        {
            var order = _orderAppService.CreateOrderAsync(orderInput);
            return CreatedContent("/api/order", order);
        }

        [HttpGet]
        [Route("/saleOrder/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderByNumberAsync([FromRoute] string orderNumber)
        {
            var order = await _orderAppService.GetOrderByNumberAsync(orderNumber);
            return OkOrNotFound(order);
        }
    }
}
