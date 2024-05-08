using FoodShareNet.Domain.Entities;

namespace FoodShareNet.Application.Interfaces;

public interface IBeneficiaryService
{
    Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary);

    Task<Beneficiary> GetBeneficiaryAsync(int id);

    Task<bool> UpdateBeneficiaryAsync(int beneficiaryId, Beneficiary beneficiary);

    Task<List<Beneficiary>> GetAllBeneficiariesAsync();

    Task<bool> DeleteBeneficiaryAsync(int beneficiaryId);

}