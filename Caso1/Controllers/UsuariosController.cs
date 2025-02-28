using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Caso1.Core.Data;
using Caso1.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Caso1.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private SelectList GetRoles(string? rolSeleccionado = null)
        {
            return new SelectList(Enum.GetValues(typeof(RolUsuario))
                .Cast<RolUsuario>()
                .Select(r => new { Value = r.ToString(), Text = r.ToString() }),
                "Value", "Text", rolSeleccionado);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.Roles = GetRoles();
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCompleto,NombreUsuario,Correo,Telefono,Contraseña,Rol")] Usuario usuarios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }
            ViewBag.Roles = GetRoles(usuarios.Rol.ToString());
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,NombreCompleto,NombreUsuario,Correo,Telefono,Contraseña,Rol")] Usuario usuario)
        {
            if (!UsuariosExists(usuario.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);
                    if (usuarioExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar solo campos necesarios
                    usuarioExistente.NombreCompleto = usuario.NombreCompleto;
                    usuarioExistente.NombreUsuario = usuario.NombreUsuario;
                    usuarioExistente.Correo = usuario.Correo;
                    usuarioExistente.Telefono = usuario.Telefono;
                    usuarioExistente.Contraseña = usuario.Contraseña;
                    usuarioExistente.Rol = usuario.Rol;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500, "Error de concurrencia en la base de datos.");
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = GetRoles(usuario.Rol.ToString());
            return View(usuario);
        }


        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuarios == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuarios);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
