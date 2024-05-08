using FoodShareApi.DTO.Beneficiary;
using FoodShareApi.DTO.Courier;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CourierController : ControllerBase
{
    private readonly ICourierService _courierService;

    public CourierController(ICourierService courierService)
    {
        _courierService = courierService;
    }

    [ProducesResponseType(typeof(IList<CourierDTO>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<CourierDTO>>> GetAllAsync()
    {
       
            var couriers = await _courierService.GetAllCouriersAsync();

            var courierDTOs = new List<CourierDTO>();
            foreach (var courier in couriers)
            {
                courierDTOs.Add(new CourierDTO
                {
                    Id = courier.Id,
                    Name = courier.Name,
                    Price = courier.Price
                });
            }

            return Ok(courierDTOs);
        
    }

    [ProducesResponseType(typeof(CourierDTO),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<CourierDTO>> GetAsync(int id)
    {
        var courier = await _courierService.GetCourierAsync(id);

        if (courier == null)
        {
            return NotFound();
        }

        var courierDTO = new CourierDTO
        {
            Id = courier.Id,
            Name = courier.Name,
            Price = courier.Price
        };

        return Ok(courierDTO);
    }


    [ProducesResponseType(typeof(CourierDetailDTO),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<CourierDetailDTO>> CreateAsync(CreateCourierDTO createCourierDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var courier = new Courier
        {
            Name = createCourierDto.Name,
            Price = createCourierDto.Price
        };

        var createdCourier = await _courierService.CreateCourierAsync(courier);

        var courierEntityDTO = new CourierDetailDTO
        {
            Id = createdCourier.Id,
            Name = createdCourier.Name,
            Price = createdCourier.Price
        };

        return Ok(courierEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> EditAsync(int id, EditCourierDTO editCourierDto)
    {
        if (id != editCourierDto.Id)
        {
            return BadRequest();
        }

        var updated = await _courierService.UpdateCourierAsync(id, new Courier
        {
            Name = editCourierDto.Name,
            Price = editCourierDto.Price
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
        var deleted = await _courierService.DeleteCourierAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}