using Microsoft.EntityFrameworkCore;
using MiniInventory.Infrastructure.Data;
using MiniInventory.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ─── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Use camelCase for JSON serialization to match React frontend expectations
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Mini Inventory Management System API",
        Version = "v1",
        Description = "ASP.NET Core 8 Web API – Clean Architecture | Repository Pattern | Ceylon Innovation Services",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ceylon Innovation Services (PVT) LTD",
            Url = new Uri("https://ceyloninnovation.com")
        }
    });
});

// ─── Infrastructure + Application layer DI ───────────────────────────────────
builder.Services.AddInfrastructure(builder.Configuration);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

// ─── CORS – allow the frontend ───────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",   // Vite dev server default
                "http://localhost:3000",   // CRA default
                "http://localhost:4173"    // Vite preview
              )
              .SetIsOriginAllowed(origin => true) // Allow any origin for development purposes
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// ─── Auto-apply EF Core Migrations on startup ────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ─── HTTP Pipeline ────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini Inventory API v1");
        c.RoutePrefix = "swagger";   // Available at /swagger
    });
}

app.UseHttpsRedirection();
app.UseCors("ReactFrontend");
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
