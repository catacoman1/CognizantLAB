using FoodShareApi.DTO.Beneficiary;
using FoodShareApi.DTO.Donation;
using FoodShareNet.Application.Interfaces;
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
    private readonly IDonationService _donationService;
    public DonationController(IDonationService donationService)
    {
        _donationService = donationService;
    }

    
    
    
    [ProducesResponseType(typeof(IList<DonationDTO>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<DonationDTO>>> GetAllAsync()
    {
        
            var donations = await _donationService.GetAllDonationsAsync();
            var donationDTOs = donations.Select(d => new DonationDTO
            {
                Id = d.Id,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                Status = d.Status.Name
            }).ToList();
            return Ok(donationDTOs);
        
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<DonationDTO>> GetAsync(int? id)
    {
        
            var donation = await _donationService.GetDonationAsync(id ?? 0);
            if (donation == null)
            {
                return NotFound();
            }
            var donationDTO = new DonationDTO
            {
                Id = donation.Id,
                Product = donation.Product.Name,
                Quantity = donation.Quantity,
                ExpirationDate = donation.ExpirationDate,
                Status = donation.Status.Name
            };
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

            var createdDonation = await _donationService.CreateDonationAsync(donation);

            var donationEntityDTO = new DonationDetailDTO
            {
                Id = createdDonation.Id,
                DonorId = createdDonation.DonorId,
                Product = createdDonation.Product.Name,
                Quantity = createdDonation.Quantity,
                ExpirationDate = createdDonation.ExpirationDate,
                StatusId = createdDonation.StatusId,
                Status = createdDonation.Status.Name
            };

            return Ok(donationEntityDTO);


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
                return BadRequest();
            }

            var updated = await _donationService.UpdateDonationAsync(id, new Donation
            {
                Product = new Product { Name = editDonationDto.Product },
                Quantity = editDonationDto.Quantity,
                ExpirationDate = editDonationDto.ExpirationDate,
                Status = new DonationStatus { Name = editDonationDto.Status }
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
            var deleted = await _donationService.DeleteDonationAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        
    
}

