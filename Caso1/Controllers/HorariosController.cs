using Caso1.Core.Data;
using Caso1.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Caso1.Controllers
{
    public class HorariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HorariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region CRUDs
        [HttpGet]
        public async Task<IActionResult> AdministracionHorario()
        {
            var horarios = await _context.Horarios
                    .OrderBy(h => h.Hora)
                    .ToListAsync();
            return View(horarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Hora")] Horario horario)
        {
            if (!ModelState.IsValid)
            {
                return View(horario);
            }
            _context.Add(horario);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Horario creado.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction(nameof(AdministracionHorario));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var horario = await _context.Horarios
                .Include(h => h.RutasHorarios)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (horario == null)
            {
                return NotFound();
            }

            try
            {
                // Eliminar referencias en la tabla intermedia (RutasHorarios en este caso)
                if (horario.RutasHorarios.Any())
                {
                    _context.RutasHorarios.RemoveRange(horario.RutasHorarios);
                }

                // Ahora eliminamos el horario
                _context.Horarios.Remove(horario);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Horario eliminado.";
                TempData["TipoMensaje"] = "success";
            }
            catch (DbUpdateException)
            {
                TempData["Mensaje"] = "Error al eliminar el horario.";
                TempData["TipoMensaje"] = "danger";
            }

            return RedirectToAction(nameof(AdministracionHorario));
        }
        #endregion
    }
}
