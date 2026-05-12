namespace RecipeAppBackend.Models
{
    public class User
    {
        // Changed 'Id' to 'UserId' to match your SQL and Controller logic
        public int UserId { get; set; }

        // Changed 'Name' to 'FullName' to match your SQL table
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // Changed to 'PasswordHash' for professional security naming
        public string PasswordHash { get; set; } = string.Empty;

        // Added this to track when the user joined your cloud platform
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}