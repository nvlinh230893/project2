using Microsoft.AspNetCore.Mvc;
using WebApp.Extensions;
using WebApp.Filters;

namespace WebApp.Features.Products;

/// <summary>
/// Manage products
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppAuthorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public ActionResult<List<ProductDto>> GetProducts()
    {
        var result = _productService.GetAll();
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a product by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<ProductDto> GetProduct(int id)
    {
        var result = _productService.GetById(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var result = await _productService.CreateAsync(dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
    {
        var result = await _productService.UpdateAsync(id, dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a product (soft delete)
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteAsync(id);
        return result.ToActionResult();
    }
}
