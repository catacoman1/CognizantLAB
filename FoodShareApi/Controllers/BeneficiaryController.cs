using System.Collections.Specialized;
using FoodShareApi.DTO.Beneficiary;
using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BeneficiaryController : ControllerBase
{
    private readonly IBeneficiaryService _beneficiaryService;
    public BeneficiaryController(IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }

    
    
    [ProducesResponseType(typeof(IList<BeneficiaryDTO>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<BeneficiaryDTO>>> GetAllAsync()
    {
        var beneficiaries = await _beneficiaryService.GetAllBeneficiariesAsync();

        var beneficiaryDTOs = beneficiaries.Select(b => new BeneficiaryDTO
        {
            Id = b.Id,
            Name = b.Name,
            Adress = b.Adress,
            CityName = b.City?.Name,
            Capacity = b.Capacity
        }).ToList();

        return Ok(beneficiaryDTOs);
    }

    [ProducesResponseType(typeof(BeneficiaryDTO),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<BeneficiaryDTO>> GetBeneficiary(int id)
    {
        Beneficiary beneficiary = null;
        try
        {
            beneficiary = await _beneficiaryService.GetBeneficiaryAsync(id);
        }
        catch (NotFoundException nfe)
        {
            return NotFound();
        }

        return Ok(beneficiary);
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

        var createBeneficiary = await _beneficiaryService.CreateBeneficiaryAsync(beneficiary);

        var beneficiaryEntityDTO = new BeneficiaryDetailDTO()
        {
            Id = createBeneficiary.Id,
            Name = createBeneficiary.Name,
            Adress = createBeneficiary.Adress,
            CityId = createBeneficiary.CityId,
            Capacity = createBeneficiary.Capacity
        };

        return Ok(beneficiaryEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> EditAsync(int id, [FromBody] EditBeneficiaryDTO editBeneficiaryDto)
    {
        if (id != editBeneficiaryDto.Id)
        {
            return BadRequest("Mismatched Beneficiary DTO");
        }
        
        var updated = await _beneficiaryService.UpdateBeneficiaryAsync(id, new Beneficiary
            {
                Name = editBeneficiaryDto.Name,
                Adress = editBeneficiaryDto.Address,
                CityId = editBeneficiaryDto.CityId,
                Capacity = editBeneficiaryDto.Capacity
            });

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();

        
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var deleted = await _beneficiaryService.DeleteBeneficiaryAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
       
    }
    
}