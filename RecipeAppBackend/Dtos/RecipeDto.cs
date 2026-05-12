namespace RecipeAppBackend.Dtos
{
    public class RecipeDto {
        public string Title { get; set; } = string.Empty;
        public int BaseServings { get; set; }
        public string? Instructions { get; set; }
        public string? ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public List<IngredientDto> Ingredients { get; set; } = new();
    }

    public class IngredientDto {
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string? Unit { get; set; }
    }

    public class BookmarkRequest {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}