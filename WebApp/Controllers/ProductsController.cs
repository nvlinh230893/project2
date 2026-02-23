using AutoMapper;
using Elect.Data.EF.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTOs;
using WebApp.Data.Interfaces;
using WebApp.Data.Models;

namespace WebApp.Controllers;

/// <summary>
/// Manage products
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IRepository<ProductEntity> _productRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductsController(
        IRepository<ProductEntity> productRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepo = productRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetProducts()
    {
        var entities = _productRepo.Get().ToList();
        return Ok(_mapper.Map<List<ProductDto>>(entities));
    }

    /// <summary>
    /// Get a product by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<ProductDto> GetProduct(int id)
    {
        var entity = _productRepo.GetSingle(x => x.Id == id);
        if (entity == null)
            return NotFound();

        return Ok(_mapper.Map<ProductDto>(entity));
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var entity = _mapper.Map<ProductEntity>(dto);
        _productRepo.Add(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<ProductDto>(entity);
        return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
    {
        var entity = _productRepo.GetSingle(x => x.Id == id);
        if (entity == null)
            return NotFound();

        _mapper.Map(dto, entity);
        _productRepo.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a product (soft delete)
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var entity = _productRepo.GetSingle(x => x.Id == id);
        if (entity == null)
            return NotFound();

        _productRepo.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
