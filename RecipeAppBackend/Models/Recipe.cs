using System.Collections.Generic;

namespace RecipeAppBackend.Models
{
    public class Recipe 
    {
        public int RecipeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int BaseServings { get; set; }
        public string? Instructions { get; set; }
        public string? ImageUrl { get; set; }
        public int CreatedBy { get; set; }

        // ADD THIS LINE: This allows .Include() to work
        public List<Ingredient> Ingredients { get; set; } = new();
    }
}