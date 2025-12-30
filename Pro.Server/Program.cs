using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;          // keep your existing namespace
using PRO_.Data.Seeder;          // if your seeder is still in PRO_.Data.Seeder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core
builder.Services.AddDbContext<ToolLendingContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql => npgsql.MigrationsAssembly(typeof(Program).Assembly.FullName) // migrations live in Pro.Server assembly now
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Auto apply migrations + seed (DEV only)
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ToolLendingContext>();

    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.MapControllers();

app.Run();