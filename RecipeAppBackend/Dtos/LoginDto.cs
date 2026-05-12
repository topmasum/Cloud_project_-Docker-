namespace RecipeAppBackend.Dtos
{
    /// <summary>
    /// Data Transfer Object for User Login
    /// This matches the JSON body sent from the Next.js login page.
    /// </summary>
    public class LoginDto
    {
        // Must match the "email" key in your JSON body
        public string Email { get; set; } = string.Empty;

        // Using PasswordHash to stay consistent with your User model
        public string PasswordHash { get; set; } = string.Empty;
    }
}