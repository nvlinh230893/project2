using AutoMapper;
using Elect.Data.EF.Interfaces.UnitOfWork;
using WebApp.Common;
using WebApp.Data.Interfaces;
using WebApp.Data.Models;

namespace WebApp.Features.Products;

public class ProductService : IProductService
{
    private readonly IRepository<ProductEntity> _productRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(
        IRepository<ProductEntity> productRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepo = productRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Result<List<ProductDto>> GetAll()
    {
        var entities = _productRepo.Get().ToList();
        return _mapper.Map<List<ProductDto>>(entities);
    }

    public Result<ProductDto> GetById(int id)
    {
        var entity = _productRepo.GetSingle(x => x.Id == id);
        if (entity == null)
            return Result.Failure<ProductDto>(ErrorCodes.Products.NotFound(id));

        return _mapper.Map<ProductDto>(entity);
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        var entity = _mapper.Map<ProductEntity>(dto);
        _productRepo.Add(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(entity);
    }

    public async Task<Result> UpdateAsync(int id, UpdateProductDto dto)
    {
        var entity = _productRepo.GetSingle(x => x.Id == id);
        if (entity == null)
            return Result.Failure(ErrorCodes.Products.NotFound(id));

        _mapper.Map(dto, entity);
        _productRepo.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var entity = _productRepo.GetSingle(x => x.Id == id);
        if (entity == null)
            return Result.Failure(ErrorCodes.Products.NotFound(id));

        _productRepo.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
