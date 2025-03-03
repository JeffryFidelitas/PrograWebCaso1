using Caso1.Core.Data;
using Caso1.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Caso1.Controllers
{
    public class RutaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RutaController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region CRUDs
        [HttpGet]
        public async Task<IActionResult> AdministracionRuta()
        {
            var rutas = await _context.Rutas
                .Include(r => r.RutasHorarios).ThenInclude(rh => rh.Horario)
                .Select(r => new RutaListViewModel
                {
                    Id = r.Id,
                    Codigo = r.Codigo,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    Estado = r.Estado,
                    Horarios = r.RutasHorarios.Select(rh => rh.Horario.Hora).ToList()
                })
                .ToListAsync();

            return View(rutas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Horarios = GetHorarios();
            ViewBag.Paradas = GetParadas();
            ViewBag.EstadoRuta = GetEstadoRuta();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Descripcion,Estado,HorariosSeleccionados,ParadasSeleccionadas")] RutaCRUDViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Obtener ID del usuario actual
                var usuarioId = await _context.Usuarios
                    .Where(u => u.NombreUsuario == User.Identity!.Name)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();

                if (usuarioId == 0) // Validar que el usuario existe
                {
                    ModelState.AddModelError("", "No se pudo identificar al usuario actual.");
                    ViewBag.Horarios = GetHorarios();
                    ViewBag.Paradas = GetParadas();
                    ViewBag.EstadoRuta = GetEstadoRuta();
                    return View(model);
                }

                var ruta = new Ruta
                {
                    Codigo = model.Codigo,
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Estado = model.Estado,
                    FechaRegistro = DateTime.UtcNow,
                    UsuarioRegistroId = usuarioId,

                    // Asignar horarios seleccionados
                    RutasHorarios = model.HorariosSeleccionados
                        .Select(id => new RutaHorario { HorarioId = id })
                        .ToList(),

                    // Asignar paradas seleccionadas con orden predeterminado
                    RutasParadas = model.ParadasSeleccionadas
                        .Select((id, index) => new RutaParada { ParadaId = id, Orden = index + 1 })
                        .ToList()
                };

                _context.Add(ruta);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Ruta Creada.";
                TempData["TipoMensaje"] = "success";

                return RedirectToAction(nameof(AdministracionRuta));
            }

            ViewBag.Horarios = GetHorarios();
            ViewBag.Paradas = GetParadas();
            ViewBag.EstadoRuta = GetEstadoRuta();

            TempData["Mensaje"] = "No se pudo crear la ruta.";
            TempData["TipoMensaje"] = "danger";

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ruta = await _context.Rutas
                .Include(r => r.RutasParadas).ThenInclude(rp => rp.Parada)
                .Include(r => r.RutasHorarios).ThenInclude(rh => rh.Horario)
                .FirstOrDefaultAsync(r => r.Id == id);


            if (ruta == null) return NotFound();

            var model = new RutaCRUDViewModel
            {
                Id = ruta.Id,
                Codigo = ruta.Codigo,
                Nombre = ruta.Nombre,
                Descripcion = ruta.Descripcion,
                Estado = ruta.Estado,
                ParadasSeleccionadas = ruta.RutasParadas.Select(rp => rp.ParadaId).ToList(),
                HorariosSeleccionados = ruta.RutasHorarios.Select(rh => rh.HorarioId).ToList()
            };

            ViewBag.Paradas = GetParadas(model.ParadasSeleccionadas);
            ViewBag.Horarios = GetHorarios(model.HorariosSeleccionados);
            ViewBag.EstadoRuta = GetEstadoRuta(ruta.Estado.ToString());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Descripcion,Estado,HorariosSeleccionados,ParadasSeleccionadas")] RutaCRUDViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Verificar que la ruta exista
                var rutaExistente = await _context.Rutas
                    .Include(r => r.RutasHorarios)
                    .Include(r => r.RutasParadas)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (rutaExistente == null) return NotFound();

                // Obtener ID del usuario actual
                var usuarioId = await _context.Usuarios
                    .Where(u => u.NombreUsuario == User.Identity!.Name)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();

                if (usuarioId == 0) // Validar que el usuario existe
                {
                    ModelState.AddModelError("", "No se pudo identificar al usuario actual.");
                    ViewBag.Horarios = GetHorarios();
                    ViewBag.Paradas = GetParadas();
                    ViewBag.EstadoRuta = GetEstadoRuta();
                    return View(model);
                }

                // Actualizar valores de la Ruta
                rutaExistente.Codigo = model.Codigo;
                rutaExistente.Nombre = model.Nombre;
                rutaExistente.Descripcion = model.Descripcion;
                rutaExistente.Estado = model.Estado;

                // Actualizar relación con Horarios
                rutaExistente.RutasHorarios.Clear();
                rutaExistente.RutasHorarios.AddRange(model.HorariosSeleccionados.Select(horarioId => new RutaHorario { HorarioId = horarioId, RutaId = id }));

                // Actualizar relación con Paradas
                rutaExistente.RutasParadas.Clear();
                rutaExistente.RutasParadas.AddRange(model.ParadasSeleccionadas.Select((paradaId, index) => new RutaParada { ParadaId = paradaId, RutaId = id, Orden = index + 1 }));

                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Ruta Actualizada.";
                TempData["TipoMensaje"] = "success";

                return RedirectToAction(nameof(AdministracionRuta));
            }

            ViewBag.Horarios = GetHorarios();
            ViewBag.Paradas = GetParadas();
            ViewBag.EstadoRuta = GetEstadoRuta();

            TempData["Mensaje"] = "No se pudo actualizar la ruta.";
            TempData["TipoMensaje"] = "danger";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ruta = _context.Rutas
                .Include(r => r.UsuarioRegistro)
                .Include(r => r.RutasParadas)
                    .ThenInclude(rp => rp.Parada)
                .Include(r => r.RutasHorarios)
                    .ThenInclude(rh => rh.Horario)
                .FirstOrDefault(r => r.Id == id);

            if (ruta == null)
            {
                return NotFound();
            }

            var nombreUsuarioRegistro = await _context.Usuarios
                .Where(u => u.Id == ruta.UsuarioRegistroId)
                .Select(u => u.NombreCompleto)
                .FirstOrDefaultAsync();

            var viewModel = new RutaDetailsViewModel
            {
                Id = ruta.Id,
                Codigo = ruta.Codigo,
                Nombre = ruta.Nombre,
                Descripcion = ruta.Descripcion,
                Estado = ruta.Estado.HasValue ? (ruta.Estado == EstadoRuta.Activo ? "Activo" : "Inactivo") : "Sin definir",
                FechaRegistro = ruta.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                NombreUsuarioRegistro = nombreUsuarioRegistro!,
                Paradas = ruta.RutasParadas
                    .OrderBy(rp => rp.Orden)
                    .Select(rp => $"{rp.Parada.Nombre} (Orden: {rp.Orden})")
                    .ToList(),
                Horarios = ruta.RutasHorarios
                    .Select(rh => rh.Horario.Hora.ToString())
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ruta = await _context.Rutas
                .Include(r => r.RutasHorarios)
                .Include(r => r.RutasParadas)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (ruta == null) return NotFound();

            // Eliminar relaciones antes de eliminar la ruta
            _context.RutasHorarios.RemoveRange(ruta.RutasHorarios);
            _context.RutasParadas.RemoveRange(ruta.RutasParadas);
            _context.Rutas.Remove(ruta);

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Ruta Eliminada.";
            TempData["TipoMensaje"] = "success";

            return RedirectToAction(nameof(AdministracionRuta));
        }
        #endregion




        #region Métodos Privados
        private SelectList GetParadas(List<int>? seleccionadas = null)
        {
            var paradas = _context.Paradas.ToList();
            return new SelectList(paradas, "Id", "Nombre", seleccionadas);
        }

        private SelectList GetHorarios(List<int>? seleccionados = null)
        {
            var horarios = _context.Horarios.ToList();
            return new SelectList(horarios, "Id", "Hora", seleccionados);
        }

        private SelectList GetEstadoRuta(string? estadoSeleccionado = null)
        {
            return new SelectList(Enum.GetValues(typeof(EstadoRuta))
                .Cast<EstadoRuta>()
                .Select(r => new { Value = r.ToString(), Text = r.ToString() }),
                "Value", "Text", estadoSeleccionado);
        }
        #endregion
    }
}
