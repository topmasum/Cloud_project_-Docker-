namespace RecipeAppBackend.Models
{
    public class Bookmark
    {
        public int BookmarkId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}