using System.ComponentModel.DataAnnotations;

namespace FoodShareApi.DTO.Beneficiary;

public class CreateBeneficiaryDTO
{
   [Required]
    public string Name { get; set; }
    public int CityId { get; set; }
    public string Address { get; set; }
    public int Capacity { get; set; }
    
}