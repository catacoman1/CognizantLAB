using FoodShareApi.DTO.Product;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public ProductController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<ProductDTO>>> GetAllAsync()
    {
        var products = await _context.Products
            .Select(p => new ProductDTO()
            {
              Id = p.Id,
              Name = p.Name,
              Image = p.Image

            }).ToListAsync();
        return Ok(products);
    }
    
     [ProducesResponseType(typeof(ProductDTO),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<ProductDTO>> GetAsync(int? id)
    {
        var productDTO = await _context.Products
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image
               
            })
            .FirstOrDefaultAsync(p => p.Id == id);

        if (productDTO == null)
        {
            return NotFound();
        }

        return Ok(productDTO);
    }
    [ProducesResponseType(typeof(ProductDetailsDTO),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<ProductDetailsDTO>> CreateAsync(CreateProductDTO createProductDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var products = new Product 
        {
            Name = createProductDto.Name,
            Image = createProductDto.Image
        };

        _context.Add(products);
        await _context.SaveChangesAsync();

        var productEntityDTO = new ProductDetailsDTO()
        {
            //Id = products.Id,
            Name = products.Name,
            Image = products.Image
            
        };

        return Ok(productEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> EditAsync(int id, EditProductDTO editProductDTO)
    {
        if (id != editProductDTO.Id)
        {
            return BadRequest("Mismatched Product DTO");
            
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == editProductDTO.Id);

        if (product == null)
        {
            return NotFound();
            
        }

        product.Name = editProductDTO.Name;
        product.Image = editProductDTO.Image;

        await _context.SaveChangesAsync();

        return NoContent();



    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}