using FoodShareApi.DTO.Beneficiary;
using FoodShareApi.DTO.Donation;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonationController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public DonationController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    
    
    
    [ProducesResponseType(typeof(IList<DonationDTO>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<DonationDTO>>> GetAllAsync()
    {
        var donations = await _context.Donations
            .Include(d=> d.Product)
            .Include(d=> d.Status)
            .Select(d => new DonationDTO
            {
                Id = d.Id,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                Status = d.Status.Name

            }).ToListAsync();
        return Ok(donations);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<DonationDetailDTO>> GetAsync(int? id)
    {
        var donationDTO = await _context.Donations
            .Select(d => new DonationDTO
            {
                Id = d.Id,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                Status = d.Status.Name
            })
            .FirstOrDefaultAsync(d => d.Id == id);

        if (donationDTO == null)
        {
            return NotFound();
        }

        return Ok(donationDTO);
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<DonationDetailDTO>> CreateAsync(CreateDonationDTO createDonationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var donation = new Donation 
        {
            DonorId = createDonationDto.DonorId,
            ProductId = createDonationDto.ProductId,
            Quantity = createDonationDto.Quantity,
            ExpirationDate = createDonationDto.ExpirationDate,
            StatusId = createDonationDto.StatusId
            
        };

        _context.Add(donation);
        await _context.SaveChangesAsync();

        var donationEntityDTO = new DonationDetailDTO()
        {
            Id = donation.Id,
            DonorId = donation.DonorId,
            Product = donation.Product.Name,
            Quantity = donation.Quantity,
            ExpirationDate = donation.ExpirationDate,
            StatusId = donation.StatusId,
            Status = donation.Status.Name
            
        };

        return Ok(donationEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<IList<DonationDetailDTO>>> GetDonationsByCityId(int cityId)
    {
        var donationDTO = await _context.Donations
            .Include(d => d.Donor)
            .ThenInclude(donor => donor.City)
            .Include(d => d.Product)
            .Include(d => d.Status)
            .Where(d => d.Donor.CityId == cityId)
            .Select(d => new DonationDetailDTO
            {
                Id = d.Id,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                Status = d.Status.Name
            })
            .ToListAsync();
    
        if (!donationDTO.Any())
        {
            return NotFound();
        }
    
        return Ok(donationDTO);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> EditAsync(int id, EditDonationDTO editDonationDto)
    {
        if (id != editDonationDto.Id)
        {
            return BadRequest("Mismatched Donation DTO");
            
        }

        var donation = await _context.Donations
            .Include(d => d.Product)
            .Include(d => d.Status)
            .FirstOrDefaultAsync(d => d.Id == editDonationDto.Id);
            
        if (donation == null)
        {
            return NotFound();
            
        }

        donation.Product.Name = editDonationDto.Product;
        donation.Quantity = editDonationDto.Quantity;
        donation.ExpirationDate = editDonationDto.ExpirationDate;
        donation.Status.Name = editDonationDto.Status;

        await _context.SaveChangesAsync();

        return NoContent();



    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var donation = await _context.Donations.FindAsync(id);

        if (donation == null)
        {
            return NotFound();
        }

        _context.Donations.Remove(donation);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}