using System.Collections.Generic;

namespace RecipeAppBackend.Dtos
{
    public class RecipeDto
    {
        public string Title { get; set; } = string.Empty;
        public int BaseServings { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int CreatedBy { get; set; } 
        public List<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
    }

    public class IngredientDto
    {
        public string Name { get; set; } = string.Empty;
        
        // FIXED: Changed double to decimal to match your Ingredient Model/Database
        public decimal Quantity { get; set; } 
        
        public string Unit { get; set; } = string.Empty;
    }

    /// <summary>
    /// This DTO handles the data for adding/removing bookmarks.
    /// Keeping it here allows the BookmarksController to access it via the Dtos namespace.
    /// </summary>
    public class BookmarkRequest
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}