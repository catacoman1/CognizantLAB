using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNet.Application.Services;

public class DonationService : IDonationService
{
    private readonly IFoodShareDbContext _context;

    public DonationService(IFoodShareDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<List<Donation>> GetAllDonationsAsync()
    {
        return await _context.Donations.ToListAsync();
    }
    public async Task<Donation> GetDonationAsync(int id)
    {
        var donation = await _context.Donations.FirstOrDefaultAsync(d => d.Id == id);
        if (donation == null)
        {
            throw new NotFoundException("Donation", id);
        }
        return donation;
    }
    public async Task<Donation> CreateDonationAsync(Donation donation)
    {
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();
        return donation;
    }
    public async Task<bool> UpdateDonationAsync(int donationId, Donation updatedDonation)
    {
        var donation = await _context.Donations.FirstOrDefaultAsync(d => d.Id == donationId);
        if (donation == null)
        {
            throw new NotFoundException("Donation", donationId);
        }

        donation.Id = updatedDonation.Id;
        donation.Product.Name = updatedDonation.Product.Name;
        donation.Quantity = updatedDonation.Quantity;
        donation.ExpirationDate = updatedDonation.ExpirationDate;
        donation.Status = updatedDonation.Status;

        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteDonationAsync(int donationId)
    {
        var donation = await _context.Donations.FindAsync(donationId);
        if (donation == null)
        {
            throw new NotFoundException("Donation", donationId);
        }

        _context.Donations.Remove(donation);
        await _context.SaveChangesAsync();
        return true;
    }

}