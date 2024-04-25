using FoodShareNet.Domain.Entities;
using FoodShareNet.Domain.Enums;

namespace FoodShareNet.Application.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order);

    Task<Order> GetOrderAsync(int id);

    Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatusEnum orderStatus);
    
}