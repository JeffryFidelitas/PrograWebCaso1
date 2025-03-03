using System.Collections.Immutable;
using Caso1.Core.Data;
using Caso1.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Caso1.Controllers
{
    public class BoletosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoletosController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region CRUDs
        [HttpGet]
        public IActionResult AdministracionBoleto()
        {

            var boletos = _context.Boletos
                .Select(b => new BoletoListViewModel{
                    Id = b.Id,
                    Usuario = _context.Usuarios.Where(u => u.Id == b.UsuarioId).FirstOrDefault(),
                    Ruta = _context.Rutas.Where(r => r.Id == b.RutaId).FirstOrDefault(),
                    Horario = _context.Horarios.Where(h => h.Id == b.Horario.Id).FirstOrDefault()
                })
                .ToList();
            return View(boletos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Rutas = _context.Rutas
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
                .ToList();
            ViewBag.Vehiculos = new SelectList(_context.Vehiculos.ToList(), "Id", "Modelo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int Ruta, int Vehiculo, String Horario, Boleto model)
        {
            ViewBag.Rutas = _context.Rutas
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
                .ToList();

            var usuario = _context.Usuarios
                .Where(u => u.NombreUsuario == User.Identity!.Name)
                .FirstOrDefault();

            if (usuario == null)
                return View(model);

            model.Usuario = usuario;
            model.UsuarioId = usuario.Id;

            var RutaEnc = _context.Rutas
                .Where(r => r.Id == Ruta)
                .FirstOrDefault();

            if (RutaEnc == null)
                return View(model);

            model.Ruta = RutaEnc;
            model.RutaId = RutaEnc.Id;

            if (Horario == null)
                return View(model);

            var HorarioEnc = _context.Horarios
                .Where(h => h.Hora == TimeSpan.Parse(Horario))
                .FirstOrDefault();

            if (HorarioEnc == null)
                return View(model);

            model.Horario = HorarioEnc;

            var VehiculoEnc = _context.Vehiculos
                .Where(v => v.Id == Vehiculo)
                .FirstOrDefault();

            if (VehiculoEnc == null)
                return View(model);

            if (VehiculoEnc.Capacidad < 1)
                return View(model);

            model.Vehiculo = VehiculoEnc;
            model.VehiculoId = VehiculoEnc.Id;
            model.Vehiculo.Capacidad -= 1;

            _context.Update(model.Vehiculo);
            _context.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(AdministracionBoleto));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var boleto = _context.Boletos
                .Where(b => b.Id == id)
                .Select(b => new BoletoDetailsViewModel{
                    Id = b.Id,
                    Usuario = _context.Usuarios.Where(u => u.Id == b.UsuarioId).FirstOrDefault(),
                    Ruta = _context.Rutas.Where(r => r.Id == b.RutaId).FirstOrDefault(),
                    Horario = _context.Horarios.Where(h => h.Id == b.Horario.Id).FirstOrDefault(),
                    FechaCompra = b.FechaCompra
                })
                .ToList().FirstOrDefault();

            if (boleto == null)
                return RedirectToAction(nameof(AdministracionBoleto));
        
            return View(boleto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var boleto = _context.Boletos.Find(id);
            if (boleto == null)
                return NotFound();

            _context.Boletos.Remove(boleto);

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Boleto Eliminado.";
            TempData["TipoMensaje"] = "success";

            return RedirectToAction(nameof(AdministracionBoleto));
        }
        #endregion
    }
}
