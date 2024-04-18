using FoodShareApi.DTO.Order;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public OrderController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(type: typeof(OrderDTO) , StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderDTO orderDTO)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var donation = _context.Donations.FirstOrDefault(d => d.Id == orderDTO.DonationId);
        if (donation == null)
        {
            return BadRequest("donation not found ");
            
        }
        if (orderDTO.Quantity > donation.Quantity)
        {
            return BadRequest("Quantity too high");
        }

        var order = new Order()
        {
            BeneficiaryId = orderDTO.BeneficiaryId,
            DonationId = orderDTO.DonationId,
            CourierId = orderDTO.CourierId,
            CreationDate = orderDTO.CreationDate,
            OrderStatusId = orderDTO.OrderStatusId

        };
        donation.Quantity -= orderDTO.Quantity;
        _context.Add(order);
        await _context.SaveChangesAsync();

        var orderEntityDTO = new OrderDetailsDTO()
        {
            BeneficiaryId = order.BeneficiaryId,
            DonationId = order.DonationId,
            CourierId = order.CourierId,
            CreationDate = order.CreationDate,
            OrderStatusId = order.OrderStatusId

        };
        return Ok(orderEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        return Ok();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch()] 
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        return Ok();
    }
}
