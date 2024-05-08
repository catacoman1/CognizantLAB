using FoodShareNet.Domain.Entities;

namespace FoodShareNet.Application.Interfaces;

public interface ICourierService
{
    Task<List<Courier>> GetAllCouriersAsync();
    Task<Courier> GetCourierAsync(int id);
    Task<Courier> CreateCourierAsync(Courier courier);
    Task<bool> UpdateCourierAsync(int courierId, Courier updatedCourier);
    Task<bool> DeleteCourierAsync(int courierId);

}