using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caso1.Core.Data;
using Caso1.Core.Models;

namespace Caso1.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehiculos
        public async Task<IActionResult> AdministracionVehiculo()
        {
            var applicationDbContext = _context.Vehiculos.Include(v => v.UsuarioRegistro);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vehiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.UsuarioRegistro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // GET: Vehiculos/Create
        public IActionResult Create()
        {
            CargarEstadosEnViewBag();
            ViewData["UsuarioRegistroId"] = new SelectList(_context.Usuarios, "Id", "NombreUsuario");
            return View();
        }

        // POST: Vehiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Placa,Modelo,Capacidad,Estado")] Vehiculo vehiculo)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.NombreUsuario == User.Identity!.Name)
                .FirstOrDefaultAsync();

            if (usuario == null) // Validar que el usuario existe
            {
                ModelState.AddModelError("", "No se pudo identificar al usuario actual.");
                CargarEstadosEnViewBag();
                return View(vehiculo);
            }
            vehiculo.UsuarioRegistro = usuario;
            vehiculo.UsuarioRegistroId = usuario.Id;

            if (!ModelState.IsValid)
            {
                _context.Add(vehiculo);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Vehículo registrado correctamente.";
                TempData["TipoMensaje"] = "success";

                return RedirectToAction(nameof(AdministracionVehiculo));
            }

            // Si el modelo es inválido, recargamos el ViewBag y devolvemos la vista con errores.
            CargarEstadosEnViewBag();

            TempData["Mensaje"] = "No se pudo registrar el vehículo. Por favor, revise el formulario.";
            TempData["TipoMensaje"] = "danger";

            return View(vehiculo);
        }

        // GET: Vehiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            CargarEstadosEnViewBag();
            ViewData["UsuarioRegistroId"] = new SelectList(_context.Usuarios, "Id", "NombreUsuario", vehiculo.UsuarioRegistroId);
            return View(vehiculo);
        }

        // POST: Vehiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Placa,Modelo,Capacidad,Estado")] Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
            {

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(vehiculo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            CargarEstadosEnViewBag();
            ViewData["UsuarioRegistroId"] = new SelectList(_context.Usuarios, "Id", "NombreUsuario", vehiculo.UsuarioRegistroId);
            return View(vehiculo);
        }

        // GET: Vehiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.UsuarioRegistro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // POST: Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdministracionVehiculo));
        }
        private void CargarEstadosEnViewBag()
        {
            ViewBag.EstadosVehiculo = Enum.GetValues(typeof(EstadoVehiculo))
                .Cast<EstadoVehiculo>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = e.ToString()
                })
                .ToList();
        }
        private bool VehiculoExists(int id)
        {
            return _context.Vehiculos.Any(e => e.Id == id);
        }
    }
}
