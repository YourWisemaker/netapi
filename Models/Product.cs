using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace netapi.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public ProductData Data { get; set; } = new ProductData();
    }

    public class ProductData
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
}
