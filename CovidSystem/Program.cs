using CovidSystem.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CovidDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CovidDbContext>();

    try
    {
        // Create the database and its tables if they don't exist
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        // Handle any exceptions
        Console.WriteLine($"An error occurred while ensuring the database is created: {ex.Message}");
    }
}

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
