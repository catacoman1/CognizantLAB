using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNet.Application.Services;

public class BeneficiaryService : IBeneficiaryService
{
    private readonly IFoodShareDbContext _context;

    public BeneficiaryService(IFoodShareDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary)
    {
        _context.Beneficiaries.Add(beneficiary);
        await _context.SaveChangesAsync();

        return beneficiary;
    }

    public async Task<Beneficiary> GetBeneficiaryAsync(int id)
    {
        var beneficiary = await _context.Beneficiaries
            .FirstOrDefaultAsync(b => b.Id == id);

        if (beneficiary == null)
        {
            throw new NotFoundException("beneficiary", id);
        }

        await _context.SaveChangesAsync();

        return beneficiary;
    }

    public async Task<bool> UpdateBeneficiaryAsync(int beneficiaryId, Beneficiary updatedBeneficiary)
    {
        var editBeneficiary = await _context.Beneficiaries
            .FirstOrDefaultAsync(b => b.Id == beneficiaryId);
        if (editBeneficiary ==  null)
        {
            throw new NotFoundException("Beneficiary", beneficiaryId);
        }

        editBeneficiary.Name = updatedBeneficiary.Name;
        editBeneficiary.CityId = updatedBeneficiary.CityId;
        editBeneficiary.City = updatedBeneficiary.City;
        editBeneficiary.Adress = updatedBeneficiary.Adress;
        editBeneficiary.Capacity = updatedBeneficiary.Capacity;

        _context.Beneficiaries.Update(editBeneficiary);

        await _context.SaveChangesAsync();
        return true;

    }
    
    public async Task<List<Beneficiary>> GetAllBeneficiariesAsync()
    {
        return await _context.Beneficiaries.ToListAsync();
    }
    public async Task<bool> DeleteBeneficiaryAsync(int beneficiaryId)
    {
        var beneficiary = await _context.Beneficiaries.FindAsync(beneficiaryId);
        if (beneficiary == null)
        {
            throw new NotFoundException("Beneficiary", beneficiaryId);
        }

        _context.Beneficiaries.Remove(beneficiary);
        await _context.SaveChangesAsync();
        return true;
    }
}