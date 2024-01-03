using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Entities;
using SampleAPI.Requests;
using SampleAPI.Response;
using SampleAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, IMapper mapper, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOrder([FromBody] CreateOrderRequest orderRequest)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(orderRequest, new System.ComponentModel.DataAnnotations.ValidationContext(orderRequest), validationResults, true))
                {
                    return BadRequest(validationResults);
                }

                var orderReq = _mapper.Map<Order>(orderRequest);
                var order = await _orderService.SubmitOrder(orderReq);
                var orderResponse = _mapper.Map<OrderResponse>(order);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while adding an order");

                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpGet("recent/{days}")]
        public async Task<IActionResult> GetRecentOrders(int days)
        {
            try
            {
                var recentOrders =  await _orderService.GetRecentOrders(days);
                var orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(recentOrders);
                return Ok(orderResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while retrieving orders");

                return StatusCode(500, "An error occurred while processing the request");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var isDeleted = await _orderService.DeleteOrder(id);
                if (isDeleted)
                {
                    return Ok("Order deleted successfully");
                }
                return NotFound("Order not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while deleting an order");

                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpGet("recent-working-days/{days}")]
        public async Task<IActionResult> GetOrdersInWorkingDays(int days)
        {
            try
            {
                var orders = await _orderService.GetOrdersInWorkingDays(days);

                if (orders == null || orders.ToList().Count == 0)
                {
                    return NotFound("No orders found within the specified working days");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while retrieving orders");

                return StatusCode(500, "An error occurred while processing the request");
            }
        }
    }
}
