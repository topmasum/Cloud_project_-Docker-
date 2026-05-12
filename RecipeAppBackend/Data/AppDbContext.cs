using Microsoft.EntityFrameworkCore;
using RecipeAppBackend.Models;

namespace RecipeAppBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // These properties link your C# code to your PostgreSQL tables
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // This ensures your Composite Unique constraint for bookmarks works in the DB
            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.UserId, b.RecipeId })
                .IsUnique();
        }
    }
}