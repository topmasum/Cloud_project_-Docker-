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

        // 1. GET: api/recipes
        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Ingredients)
                .ToListAsync();
            return Ok(recipes);
        }

        // 2. GET: api/recipes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe == null) return NotFound("Recipe not found.");
            
            return Ok(recipe);
        }

        // 3. POST: api/recipes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecipeDto dto)
        {
            var recipe = new Recipe
            {
                Title = dto.Title,
                BaseServings = dto.BaseServings,
                Instructions = dto.Instructions,
                ImageUrl = dto.ImageUrl,
                CreatedBy = dto.CreatedBy
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            var ingredients = dto.Ingredients.Select(i => new Ingredient
            {
                RecipeId = recipe.RecipeId,
                Name = i.Name,
                Quantity = i.Quantity,
                Unit = i.Unit
            });

            _context.Ingredients.AddRange(ingredients);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Recipe and ingredients saved successfully!" });
        }
    }
}