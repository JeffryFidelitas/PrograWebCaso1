using Caso1.Core.Data;
using Caso1.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Caso1.Controllers
{
    public class ParadasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ParadasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AdministracionParada()
        {
            return View(await _context.Paradas.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Nombre")] Parada parada)
        {
            if (!ModelState.IsValid)
            {
                return View(parada);
            }
            _context.Add(parada);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Parada creada.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction(nameof(AdministracionParada));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var parada = await _context.Paradas
                .Include(p => p.RutasParadas)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (parada == null)
            {
                return NotFound();
            }
            try
            {
                // Eliminar referencias en la tabla intermedia (RutasParadas en este caso)
                if (parada.RutasParadas.Any())
                {
                    _context.RutasParadas.RemoveRange(parada.RutasParadas);
                }

                _context.Remove(parada);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Parada eliminada.";
                TempData["TipoMensaje"] = "success";
            }
            catch
            {
                TempData["Mensaje"] = "No se puede eliminar la parada.";
                TempData["TipoMensaje"] = "danger";
            }
            return RedirectToAction(nameof(AdministracionParada));
        }
    }
}
