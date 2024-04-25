using FoodShareApi.DTO.Order;
using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Services;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Domain.Enums;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    //private readonly FoodShareNetDbContext _context;

    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // [ProducesResponseType(type: typeof(OrderDTO) , StatusCodes.Status201Created)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // [HttpPost]
    // public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderDTO orderDTO)
    // {
    //     
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest();
    //     }
    //
    //     var donation = _context.Donations.FirstOrDefault(d => d.Id == orderDTO.DonationId);
    //     if (donation == null)
    //     {
    //         return BadRequest("donation not found ");
    //         
    //     }
    //     if (orderDTO.Quantity > donation.Quantity)
    //     {
    //         return BadRequest("Quantity too high");
    //     }
    //
    //     var order = new Order()
    //     {
    //         BeneficiaryId = orderDTO.BeneficiaryId,
    //         DonationId = orderDTO.DonationId,
    //         CourierId = orderDTO.CourierId,
    //         CreationDate = orderDTO.CreationDate,
    //         OrderStatusId = orderDTO.OrderStatusId
    //
    //     };
    //     donation.Quantity -= orderDTO.Quantity;
    //     _context.Add(order);
    //     await _context.SaveChangesAsync();
    //
    //     var orderEntityDTO = new OrderDetailsDTO()
    //     {
    //         BeneficiaryId = order.BeneficiaryId,
    //         DonationId = order.DonationId,
    //         CourierId = order.CourierId,
    //         CreationDate = order.CreationDate,
    //         OrderStatusId = order.OrderStatusId
    //
    //     };
    //     return Ok(orderEntityDTO);
    // }

    
    [ProducesResponseType(type: typeof(OrderDTO) , StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<OrderDTO>>
        CreateOrder([FromBody] CreateOrderDTO createOrderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        
        var order = new Order()
        {
            BeneficiaryId = createOrderDto.BeneficiaryId,
            DonationId = createOrderDto.DonationId,
            CourierId = createOrderDto.CourierId,
            CreationDate = createOrderDto.CreationDate,
            OrderStatusId = createOrderDto.OrderStatusId,
            Quantity = createOrderDto.Quantity

            
        };

        var createOrder = await _orderService.CreateOrderAsync(order);

        var orderDetails = new OrderDetailsDTO
        {
            Id = createOrder.Id,
            BeneficiaryId = createOrder.BeneficiaryId,
            DonationId = createOrder.DonationId,
            CourierId = createOrder.CourierId,
            CreationDate = createOrder.CreationDate,
            DeliveryDate = createOrder.DeliveryDate,
            OrderStatusId = createOrder.OrderStatusId,
            Quantity = createOrder.Quantity
        };
        return Ok(orderDetails);
    }
    
    
    
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        Order order = null; 
        try
        {
            order = await _orderService.GetOrderAsync(id);
        }
        catch (NotFoundException nfe)
        {
            return NotFound();
        }

        return Ok(order);

    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch()] 
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        try
        {
            await _orderService.UpdateOrderStatusAsync(orderId, (OrderStatusEnum)updateStatusDTO.NewStatusId);
        }
        catch (NotFoundException nfe)
        {
            return NotFound();
        }

        return NoContent();
    }
}
