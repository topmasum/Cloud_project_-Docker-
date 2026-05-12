using Microsoft.EntityFrameworkCore;
using RecipeAppBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Database Connection - FIXED: Changed back to UseNpgsql for PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Register HttpClient for the YouTube feature
builder.Services.AddHttpClient();

// 4. CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJS",
        policy => policy.WithOrigins("http://localhost:3000", "https://radiance-anyway-dumpster.ngrok-free.dev")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

var app = builder.Build();

// 5. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}
else 
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe API V1");
        c.RoutePrefix = "swagger"; 
    });
}

// 6. Middleware Order
app.UseHttpsRedirection();
app.UseRouting(); 

app.UseCors("AllowNextJS");

app.UseAuthorization();

app.MapControllers();

app.Run();