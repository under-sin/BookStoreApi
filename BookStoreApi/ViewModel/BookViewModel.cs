using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.ViewModel;

public class BookViewModel
{
    public string? Id { get; set; }
    
    [Required]
    [BsonElement("Name")]
    [JsonPropertyName("Name")]
    public string BookName { get; set; } = null!;

    [Required]
    [Range(0, 2000)]
    public decimal Price { get; set; }

    [Required]
    public string Category { get; set; } = null!;

    [Required]
    public string Author { get; set; } = null!;
}
