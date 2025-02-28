using Caso1.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#region Configuracion
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Caso1DB"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
#endregion

#region Endpoints   
app.MapGet("/api/GetRutas", async (ApplicationDbContext _context) =>
{
    try
    {
        var rutas = await _context.Rutas.Where(r => r.Activo).ToListAsync();
        return Results.Ok(rutas);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al obtener las rutas: {ex.Message}");
    }
});

app.MapGet("/api/GetRuta/{id}", async (int id, ApplicationDbContext _context) =>
{
    try
    {
        var ruta = await _context.Rutas.FindAsync(id);
        return ruta is not null ? Results.Ok(ruta) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al obtener la ruta con ID {id}: {ex.Message}");
    }
});
#endregion

app.Run();
