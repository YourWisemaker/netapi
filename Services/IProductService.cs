using netapi.DTOs;
using netapi.Models;

namespace netapi.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Gets products with filtering and pagination
        /// </summary>
        Task<ProductListResponseDto> GetProductsAsync(ProductSearchParams searchParams);

        /// <summary>
        /// Gets a product by ID
        /// </summary>
        Task<ProductDto> GetProductByIdAsync(int id);

        /// <summary>
        /// Creates a new product
        /// </summary>
        Task<ProductDto> CreateProductAsync(ProductDto productDto);

        /// <summary>
        /// Deletes a product by ID
        /// </summary>
        Task DeleteProductAsync(int id);
    }
}
