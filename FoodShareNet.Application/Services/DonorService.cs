using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNet.Application.Services;

public class DonorService : IDonorService
{
    private readonly IFoodShareDbContext _context;

    public DonorService(IFoodShareDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<List<Donor>> GetAllDonorsAsync()
    {
        return await _context.Donors
            .Include(d => d.Donations)
            .Include(d => d.City)
            .ToListAsync();
    }

    public async Task<Donor> GetDonorAsync(int id)
    {
        var donor = await _context.Donors
            .Include(d => d.Donations)
            .Include(d => d.City)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (donor == null)
        {
            throw new NotFoundException("Donor", id);
        }

        return donor;
    }

    public async Task<Donor> CreateDonorAsync(Donor donor)
    {
        _context.Donors.Add(donor);
        await _context.SaveChangesAsync();
        return donor;
    }

    public async Task<bool> UpdateDonorAsync(int donorId, Donor updatedDonor)
    {
        var donor = await _context.Donors.FirstOrDefaultAsync(d => d.Id == donorId);
        if (donor == null)
        {
            throw new NotFoundException("Donor", donorId);
        }

        donor.Name = updatedDonor.Name;
        donor.CityId = updatedDonor.CityId;
        donor.Address = updatedDonor.Address;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteDonorAsync(int donorId)
    {
        var donor = await _context.Donors.FindAsync(donorId);
        if (donor == null)
        {
            throw new NotFoundException("Donor", donorId);
        }

        _context.Donors.Remove(donor);
        await _context.SaveChangesAsync();
        return true;
    }
}
    
