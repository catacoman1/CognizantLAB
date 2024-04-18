using FoodShareApi.DTO.Beneficiary;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BeneficiaryController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public BeneficiaryController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    
    
    [ProducesResponseType(typeof(IList<BeneficiaryDTO>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<BeneficiaryDTO>>> GetAllAsync()
    {
        var beneficiaries = await _context.Beneficiaries
            .Include(b => b.City)
            .Select(b => new BeneficiaryDTO
            {
                Id = b.Id,
                Name = b.Name,
                Adress = b.Adress,
                CityName = b.City.Name,
                Capacity = b.Capacity

            }).ToListAsync();
        return Ok(beneficiaries);
    }

    [ProducesResponseType(typeof(BeneficiaryDTO),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<BeneficiaryDTO>> GetAsync(int? id)
    {
        var beneficiaryDTO = await _context.Beneficiaries
            .Select(b => new BeneficiaryDTO
            {
                Id = b.Id,
                Name = b.Name,
                Adress = b.Adress,
                CityName = b.City.Name,
                Capacity = b.Capacity
            })
            .FirstOrDefaultAsync(m => m.Id == id);

        if (beneficiaryDTO == null)
        {
            return NotFound();
        }

        return Ok(beneficiaryDTO);
    }
    [ProducesResponseType(typeof(BeneficiaryDetailDTO),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<BeneficiaryDetailDTO>> CreateAsync(CreateBeneficiaryDTO createBeneficiaryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var beneficiary = new Beneficiary
        {
            Name = createBeneficiaryDto.Name,
            Adress = createBeneficiaryDto.Address,
            CityId = createBeneficiaryDto.CityId,
            Capacity = createBeneficiaryDto.Capacity
        };

        _context.Add(beneficiary);
        await _context.SaveChangesAsync();

        var beneficiaryEntityDTO = new BeneficiaryDetailDTO()
        {
            Id = beneficiary.Id,
            Name = beneficiary.Name,
            Adress = beneficiary.Adress,
            CityId = beneficiary.CityId,
            Capacity = beneficiary.Capacity
        };

        return Ok(beneficiaryEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> EditAsync(int id, EditBeneficiaryDTO editBeneficiaryDto)
    {
        if (id != editBeneficiaryDto.Id)
        {
            return BadRequest("Mismatched Beneficiary DTO");
            
        }

        var beneficiary = await _context.Beneficiaries
            .FirstOrDefaultAsync(b => b.Id == editBeneficiaryDto.Id);

        if (beneficiary == null)
        {
            return NotFound();
            
        }

        beneficiary.Name = editBeneficiaryDto.Name;
        beneficiary.Adress = editBeneficiaryDto.Address;
        beneficiary.CityId = editBeneficiaryDto.CityId;
        beneficiary.Capacity = editBeneficiaryDto.Capacity;

        await _context.SaveChangesAsync();

        return NoContent();



    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var beneficiary = await _context.Beneficiaries.FindAsync(id);

        if (beneficiary == null)
        {
            return NotFound();
        }

        _context.Beneficiaries.Remove(beneficiary);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
}