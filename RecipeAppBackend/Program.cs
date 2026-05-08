using Microsoft.EntityFrameworkCore;
using RecipeAppBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJS",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// 4. Configure the HTTP request pipeline
// Move Swagger to the TOP of the pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}
else 
{
    // Ensure Swagger works in "Production" mode too if you need it for your thesis
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe API V1");
        c.RoutePrefix = "swagger"; 
    });
}

// CRITICAL: Ensure these are in this specific order
app.UseHttpsRedirection(); // Add this!
app.UseCors("AllowNextJS");
app.UseAuthorization();

app.MapControllers();

app.Run();