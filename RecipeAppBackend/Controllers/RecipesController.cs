using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAppBackend.Data;
using RecipeAppBackend.Models;
using RecipeAppBackend.Dtos;

namespace RecipeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/recipes (With Chef/Publisher Details)
        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.User) 
                .ToListAsync();

            // Transform data to include the Chef's Name for the frontend
            var result = recipes.Select(r => new {
                r.RecipeId,
                r.Title,
                r.BaseServings,
                r.Instructions,
                r.ImageUrl,
                r.CreatedBy,
                PublisherName = r.User != null ? r.User.FullName : "Unknown Chef",
                Ingredients = r.Ingredients
            });

            return Ok(result);
        }

        // 2. GET: api/recipes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe == null) return NotFound("Recipe not found.");
            
            var result = new {
                recipe.RecipeId,
                recipe.Title,
                recipe.BaseServings,
                recipe.Instructions,
                recipe.ImageUrl,
                recipe.CreatedBy,
                PublisherName = recipe.User != null ? recipe.User.FullName : "Unknown Chef",
                Ingredients = recipe.Ingredients
            };

            return Ok(result);
        }

        // 3. POST: api/recipes (Saves Recipe & Ingredients)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecipeDto recipeDto)
        {
            // Defensive check to ensure data is present
            if (recipeDto == null) 
            {
                return BadRequest("Recipe data is null");
            }

            var recipe = new Recipe
            {
                Title = recipeDto.Title,
                BaseServings = recipeDto.BaseServings,
                Instructions = recipeDto.Instructions,
                ImageUrl = recipeDto.ImageUrl,
                CreatedBy = recipeDto.CreatedBy
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            // Link the ingredients to the newly created RecipeId
            var ingredients = recipeDto.Ingredients.Select(i => new Ingredient
            {
                RecipeId = recipe.RecipeId,
                Name = i.Name,
                Quantity = i.Quantity,
                Unit = i.Unit
            });

            _context.Ingredients.AddRange(ingredients);
            await _context.SaveChangesAsync();

            return Ok(new { 
                message = "Recipe and ingredients saved successfully!", 
                recipeId = recipe.RecipeId 
            });
        }
    }
}