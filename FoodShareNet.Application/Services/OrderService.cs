using System.Net.NetworkInformation;
using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNet.Application.Services;

public class OrderService : IOrderService
{
    private readonly IFoodShareDbContext _context;

    public OrderService(IFoodShareDbContext dbContext)
    {
        _context = dbContext;
        
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        
        var donation = _context.Donations.FirstOrDefault(d => d.Id == order.DonationId);
        if (donation == null)
        {
            throw new OrderException("Order not found");
        }
        if (order.Quantity > donation.Quantity)
        {
            throw new OrderException("Quantity too big");
        }


        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
        
    }

    public async Task<Order> GetOrderAsync(int id)
    {
        
       
            var order = await _context.Orders
                // .Select(d => new Order
                // {
                //     Id = d.Id,
                //     BeneficiaryId = d.BeneficiaryId,
                //     DonationId = d.DonationId,
                //     CourierId = d.CourierId,
                //     CreationDate = d.CreationDate,
                //     DeliveryDate = d.DeliveryDate,
                //     OrderStatusId = d.OrderStatusId,
                //     Quantity = d.Quantity
                // })
                .FirstOrDefaultAsync(d => d.Id == id);

            if (order == null)
            {
                throw new NotFoundException("order", id);
            }

            await _context.SaveChangesAsync();

            return order;
        }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatusEnum orderStatus)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new NotFoundException("Order",orderId);
        }

        order.OrderStatusId = (int)orderStatus;

        await _context.SaveChangesAsync();
        return true;
    }
}

   

