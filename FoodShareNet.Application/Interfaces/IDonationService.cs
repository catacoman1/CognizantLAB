using FoodShareNet.Domain.Entities;

namespace FoodShareNet.Application.Interfaces;

public interface IDonationService
{
    Task<List<Donation>> GetAllDonationsAsync();
    Task<Donation> GetDonationAsync(int id);
    Task<Donation> CreateDonationAsync(Donation donation);
    Task<bool> UpdateDonationAsync(int donationId, Donation updatedDonation);
    Task<bool> DeleteDonationAsync(int donationId);
}