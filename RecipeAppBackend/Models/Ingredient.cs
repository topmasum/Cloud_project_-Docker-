namespace RecipeAppBackend.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string? Unit { get; set; }
    }
}