using FoodShareApi.DTO.Beneficiary;
using FoodShareApi.DTO.Courier;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CourierController : ControllerBase
{

    private readonly FoodShareNetDbContext _context;
    public CourierController(FoodShareNetDbContext context)
    {
        _context = context;
    }
    
    [ProducesResponseType(typeof(IList<CourierDTO>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<CourierDTO>>> GetAllAsync()
    {
        var couriers = await _context.Couriers
            .Select(c => new CourierDTO()
            {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price

            }).ToListAsync();
        return Ok(couriers);
    }
    
    [ProducesResponseType(typeof(CourierDTO),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<CourierDTO>> GetAsync(int? id)
    {
        var courierDTO = await _context.Couriers
            .Select(c => new CourierDTO()
            {
               Id = c.Id,
               Name = c.Name,
               Price = c.Price
            })
            .FirstOrDefaultAsync(c => c.Id == id);

        if (courierDTO == null)
        {
            return NotFound();
        }

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

        var courier = new Courier()
        {
            Name = createCourierDto.Name,
            Price = createCourierDto.Price
        };

        _context.Add(courier);
        await _context.SaveChangesAsync();

        var courierEntityDTO = new CourierDetailDTO()
        {
            Id = courier.Id,
            Name = courier.Name,
            Price = courier.Price
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
            return BadRequest("Mismatched Courier DTO");
            
        }

        var courier = await _context.Couriers
            .FirstOrDefaultAsync(c => c.Id == editCourierDto.Id);

        if ( courier == null)
        {
            return NotFound();
            
        }

        courier.Name = editCourierDto.Name;
        courier.Price = editCourierDto.Price;

        await _context.SaveChangesAsync();

        return NoContent();



    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var courier = await _context.Couriers.FindAsync(id);

        if (courier == null)
        {
            return NotFound();
        }

        _context.Couriers.Remove(courier);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    

}