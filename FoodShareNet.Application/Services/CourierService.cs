using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNet.Application.Services;

public class CourierService : ICourierService
{
    private readonly IFoodShareDbContext _context;

    public CourierService(IFoodShareDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<List<Courier>> GetAllCouriersAsync()
    {
        return await _context.Couriers.ToListAsync();
    }

    public async Task<Courier> GetCourierAsync(int id)
    {
        var courier = await _context.Couriers.FirstOrDefaultAsync(c => c.Id == id);
        if (courier == null)
        {
            throw new NotFoundException("Courier", id);
        }
        return courier;
    }

    public async Task<Courier> CreateCourierAsync(Courier courier)
    {
        _context.Couriers.Add(courier);
        await _context.SaveChangesAsync();
        return courier;
    }

    public async Task<bool> UpdateCourierAsync(int courierId, Courier updatedCourier)
    {
        var courier = await _context.Couriers.FirstOrDefaultAsync(c => c.Id == courierId);
        if (courier == null)
        {
            throw new NotFoundException("Courier", courierId);
        }

        courier.Name = updatedCourier.Name;
        courier.Price = updatedCourier.Price;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCourierAsync(int courierId)
    {
        var courier = await _context.Couriers.FindAsync(courierId);
        if (courier == null)
        {
            throw new NotFoundException("Courier", courierId);
        }

        _context.Couriers.Remove(courier);
        await _context.SaveChangesAsync();
        return true;
    }
}
