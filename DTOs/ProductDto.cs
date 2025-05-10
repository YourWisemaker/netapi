using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace netapi.DTOs
{
    public class ProductDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data is required")]
        [JsonPropertyName("data")]
        public ProductDataDto Data { get; set; } = new ProductDataDto();
    }

    public class ProductDataDto
    {
        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("CPU model")]
        public string? CpuModel { get; set; }

        [JsonPropertyName("Hard disk size")]
        public string? HardDiskSize { get; set; }

        [JsonPropertyName("color")]
        public string? Color { get; set; }
    }

    public class ProductListResponseDto
    {
        [JsonPropertyName("products")]
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }

    public class ProductSearchParams
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        public string? NameFilter { get; set; }
    }
}
