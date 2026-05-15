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
            
            // 1. Fix the Recipe -> User Relationship
            // This tells EF that 'CreatedBy' is the Foreign Key for the 'User' navigation property
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.User)
                .WithMany() 
                .HasForeignKey(r => r.CreatedBy);

            // 2. Composite Unique constraint for bookmarks
            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.UserId, b.RecipeId })
                .IsUnique();
        }
    }
}