using FoodShareApi.DTO.Beneficiary;
using FoodShareApi.DTO.Donation;
using FoodShareApi.DTO.Donor;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonorController : ControllerBase
{
    private readonly IDonorService _donorService;
    public DonorController(IDonorService donorService)
    {
        _donorService = donorService;
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<List<DonorDTO>>> GetAllAsync()
    {
        
            var donors = await _donorService.GetAllDonorsAsync();
            var donorDTOs = donors.Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                CityName = d.City.Name,
                Address = d.Address,
                Donations = d.Donations.Select(dn => new DonationDTO
                {
                    Id = dn.Id,
                    Product = dn.Product.Name,
                    Quantity = dn.Quantity,
                    ExpirationDate = dn.ExpirationDate,
                    Status = dn.Status.Name
                }).ToList()
            }).ToList();
            return Ok(donorDTOs);
        }

    
    
    
    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<DonorDTO>> GetAsync(int id)
    {
       
            var donor = await _donorService.GetDonorAsync(id);
            if (donor == null)
            {
                return NotFound();
            }

            var donorDTO = new DonorDTO()
            {
                Id = donor.Id,
                Name = donor.Name,
                CityName = donor.City.Name,
                Address = donor.Address,
                Donations = donor.Donations.Select(dn => new DonationDTO
                {
                    Id = dn.Id,
                    Product = dn.Product.Name,
                    Quantity = dn.Quantity,
                    ExpirationDate = dn.ExpirationDate,
                    Status = dn.Status.Name
                }).ToList()
            };
            
            return Ok(donorDTO);
        
        
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<DonorDetailDTO>> CreateAsync(CreateDonorDTO createDonorDto)
    {
        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var donor = new Donor
            {
                Name = createDonorDto.Name,
                CityId = createDonorDto.CityId,
                Address = createDonorDto.Address
            };

            await _donorService.CreateDonorAsync(donor);

            var donorEntityDTO = new DonorDetailDTO()
            {
                Id = donor.Id,
                Name = donor.Name,
                CityId = donor.CityId,
                Address = donor.Address
            };

            return Ok(donorEntityDTO);
        
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut()] 
    public async Task<IActionResult> EditAsync(int id, EditDonorDTO editDonorDto)
    {
        
            if (id != editDonorDto.Id)
            {
                return BadRequest();
            }

            var donor = await _donorService.GetDonorAsync(id);
            if (donor == null)
            {
                return NotFound();
            }

            donor.Name = editDonorDto.Name;
            donor.CityId = editDonorDto.CityId;
            donor.Address = editDonorDto.Address;

            await _donorService.UpdateDonorAsync(id, donor);

            return NoContent();
        
        
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        
            var donor = await _donorService.GetDonorAsync(id);
            if (donor == null)
            {
                return NotFound();
            }

            await _donorService.DeleteDonorAsync(id);

            return NoContent();
        }
       
    
}