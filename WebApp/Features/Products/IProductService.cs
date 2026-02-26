using WebApp.Common;

namespace WebApp.Features.Products;

public interface IProductService
{
    Result<List<ProductDto>> GetAll();
    Result<ProductDto> GetById(int id);
    Task<Result<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<Result> UpdateAsync(int id, UpdateProductDto dto);
    Task<Result> DeleteAsync(int id);
}
