using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using netapi.DTOs;
using netapi.Exceptions;
using netapi.Models;

namespace netapi.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;
        private readonly string _baseUrl = "https://api.restful-api.dev/objects";
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductService(IHttpClientFactory httpClientFactory, ILogger<ProductService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Converters = { new NullableIntJsonConverter() }
            };
        }

        public async Task<ProductListResponseDto> GetProductsAsync(ProductSearchParams searchParams)
        {
            try
            {
                // Get all products from the external API
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();

                var allProducts = await response.Content.ReadFromJsonAsync<List<ProductDto>>(_jsonOptions) 
                    ?? new List<ProductDto>();

                // Apply name filtering if provided
                if (!string.IsNullOrWhiteSpace(searchParams.NameFilter))
                {
                    allProducts = allProducts.Where(p => 
                        p.Name.Contains(searchParams.NameFilter, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Calculate total count and paging
                int totalCount = allProducts.Count;
                int skipCount = (searchParams.Page - 1) * searchParams.PageSize;
                
                // Apply paging
                var pagedProducts = allProducts
                    .Skip(skipCount)
                    .Take(searchParams.PageSize)
                    .ToList();

                return new ProductListResponseDto
                {
                    Products = pagedProducts,
                    TotalCount = totalCount,
                    Page = searchParams.Page,
                    PageSize = searchParams.PageSize
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving products from external API");
                throw new ExternalApiException(ex.Message, ex.StatusCode ?? HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing products");
                throw new ApiException($"Error processing products: {ex.Message}");
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();

                var product = await response.Content.ReadFromJsonAsync<ProductDto>(_jsonOptions);
                
                if (product == null)
                {
                    throw new NotFoundException($"Product with ID {id} not found");
                }

                return product;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"Product with ID {id} not found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving product {ProductId} from external API", id);
                throw new ExternalApiException(ex.Message, ex.StatusCode ?? HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(productDto, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                response.EnsureSuccessStatusCode();

                var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>(_jsonOptions);
                
                if (createdProduct == null)
                {
                    throw new ApiException("Failed to parse created product response");
                }

                return createdProduct;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating product in external API");
                throw new ExternalApiException(ex.Message, ex.StatusCode ?? HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"Product with ID {id} not found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId} from external API", id);
                throw new ExternalApiException(ex.Message, ex.StatusCode ?? HttpStatusCode.InternalServerError);
            }
        }
    }
}
