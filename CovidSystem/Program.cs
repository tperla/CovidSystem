using CovidSystem.DbContexts;
using CovidSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add EF to connect to DB
builder.Services.AddDbContext<CovidDbContext>(options =>
    {
        options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.EnableSensitiveDataLogging();
    });

// Add your validation service registration
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ISummeryDataService, SummeryDataService>();

//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Ensure that tabels in database exists
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
