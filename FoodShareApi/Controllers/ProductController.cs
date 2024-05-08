using FoodShareApi.DTO.Product;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<List<ProductDTO>>> GetAllAsync()
    {
        
            var products = await _productService.GetAllProductsAsync();
            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image
            }).ToList();
            return Ok(productDTOs);
        
        
    }
    
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<ProductDTO>> GetAsync(int? id)
    {
        
            var product = await _productService.GetProductAsync(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.Image
            };

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

            var product = new Product
            {
                Name = createProductDto.Name,
                Image = createProductDto.Image
            };

            await _productService.CreateProductAsync(product);

            var productEntityDTO = new ProductDetailsDTO
            {
                Name = product.Name,
                Image = product.Image
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
                return BadRequest();
            }

            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = editProductDTO.Name;
            product.Image = editProductDTO.Image;

            await _productService.UpdateProductAsync(id, product);

            return NoContent();
        
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);

            return NoContent();
        
    }

}