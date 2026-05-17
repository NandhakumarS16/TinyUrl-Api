using Microsoft.EntityFrameworkCore;
using TinyUrl.Application.Interfaces.Repositories;
using TinyUrl.Application.Interfaces.Services;
using TinyUrl.Infrastructure.Data;
using TinyUrl.Infrastructure.Repositories;


public static class ProgramHelper
{
    // Registers all application services and infrastructure dependencies
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        // ── Swagger / OpenAPI ────────────────────────────────────────────────
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = "Tiny URL API",
                Version = "v1",
                Description = "REST API for TinyURL — shortens, redirects and manages URLs."
            });
        });

        // ── Database (SQL Server + EF Core Migrations) ───────────────────────
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                }
            )
        );

        // ── Dependency Injection ─────────────────────────────────────────────
        builder.Services.AddScoped<ITinyUrlRepository, TinyUrlRepository>();
        builder.Services.AddScoped<ITinyUrlService, TinyUrlService>();

        // ── CORS ──────────────────────────────────────────────────────────────
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });
    }

    // Configures middleware pipeline
    public static void ConfigurePipeline(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tiny URL API v1");
            c.RoutePrefix = "swagger";
        });

        app.UseCors("AllowAngular");

        // Apply migrations at startup
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }
    }
}