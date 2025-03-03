using Caso1.Core.Data;
using Caso1.Core.Models;
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
        var rutas = await _context.Rutas
                .Include(r => r.UsuarioRegistro)
                .Include(r => r.RutasParadas)
                    .ThenInclude(rp => rp.Parada)
                .Include(r => r.RutasHorarios)
                    .ThenInclude(rh => rh.Horario)
                .Where(r => r.Estado == EstadoRuta.Activo).ToListAsync();
        
        var rutasDTO = rutas.Select(r => new
        {
            r.Id,
            r.Codigo,
            r.Nombre,
            r.Descripcion,
            r.Estado,
            r.FechaRegistro,
            UsuarioRegistro = r.UsuarioRegistro.NombreUsuario,
            Paradas = r.RutasParadas.Select(rp => new
            {
                rp.Orden,
                rp.Parada.Nombre
            }),
            Horarios = r.RutasHorarios.Select(rh => new
            {
                rh.Horario.Hora
            })
        });

        return Results.Ok(rutasDTO);
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
        var ruta = await _context.Rutas
                .Include(r => r.UsuarioRegistro)
                .Include(r => r.RutasParadas)
                    .ThenInclude(rp => rp.Parada)
                .Include(r => r.RutasHorarios)
                    .ThenInclude(rh => rh.Horario)
                .FirstOrDefaultAsync(r => r.Id == id);

        var rutaDTO = new
        {
            ruta.Id,
            ruta.Codigo,
            ruta.Nombre,
            ruta.Descripcion,
            ruta.Estado,
            ruta.FechaRegistro,
            UsuarioRegistro = ruta.UsuarioRegistro.NombreUsuario,
            Paradas = ruta.RutasParadas.Select(rp => new
            {
                rp.Orden,
                rp.Parada.Nombre
            }),
            Horarios = ruta.RutasHorarios.Select(rh => new
            {
                rh.Horario.Hora
            })
        };

        return ruta is not null ? Results.Ok(rutaDTO) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al obtener la ruta con ID {id}: {ex.Message}");
    }
});
#endregion

app.Run();
