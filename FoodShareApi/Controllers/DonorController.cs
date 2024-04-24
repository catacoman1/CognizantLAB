using FoodShareApi.DTO.Beneficiary;
using FoodShareApi.DTO.Donation;
using FoodShareApi.DTO.Donor;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonorController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public DonorController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<DonorDTO>>> GetAllAsync()
    {
        var donors = await _context.Donors
            .Include(d => d.Donations)
            .Include(d => d.City)
            .Select(d => new DonorDTO
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

            }).ToListAsync();
        return Ok(donors);
    }
    
    
    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<DonorDTO>> GetAsync(int id) 
    {
        var donorDTO = await _context.Donors
            .Select(d => new DonorDTO()
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
            })
            .FirstOrDefaultAsync(d => d.Id == id);

        if (donorDTO == null)
        {
            return NotFound();
        }

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

        _context.Add(donor);
        await _context.SaveChangesAsync();

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
            return BadRequest("Mismatched Donor DTO");
            
        }

        var donor = await _context.Donors
            .FirstOrDefaultAsync(d => d.Id == editDonorDto.Id);

        if (donor == null)
        {
            return NotFound();
            
        }

        donor.Name = editDonorDto.Name;
        donor.CityId = editDonorDto.CityId;
        donor.Address = editDonorDto.Address;

        await _context.SaveChangesAsync();

        return NoContent();

    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var donor = await _context.Donors.FindAsync(id);

        if (donor == null)
        {
            return NotFound();
        }

        _context.Donors.Remove(donor);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}