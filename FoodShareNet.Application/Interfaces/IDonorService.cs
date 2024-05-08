using FoodShareNet.Domain.Entities;

namespace FoodShareNet.Application.Interfaces;

public interface IDonorService
{
    Task<List<Donor>> GetAllDonorsAsync();
    Task<Donor> GetDonorAsync(int id);
    Task<Donor> CreateDonorAsync(Donor donor);
    Task<bool> UpdateDonorAsync(int donorId, Donor updatedDonor);
    Task<bool> DeleteDonorAsync(int donorId);
}