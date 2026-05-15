using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema; // Needed for [ForeignKey]

namespace RecipeAppBackend.Models
{
    public class Recipe 
    {
        public int RecipeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int BaseServings { get; set; }
        public string? Instructions { get; set; }
        public string? ImageUrl { get; set; }
        
        // This is your actual column name in the DB
        public int CreatedBy { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new();

        [JsonIgnore]
        [ForeignKey("CreatedBy")] // Add this! Tells EF to use CreatedBy instead of UserId
        public User? User { get; set; } 
    }
}