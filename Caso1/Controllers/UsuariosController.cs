using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Caso1.Core.Data;
using Caso1.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.Scripting;
using System.Security.Claims;

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

        #region Autenticación
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("NombreUsuario,Contraseña")] UsuarioLogin usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario); // Devuelve la vista con los errores de validación
            }
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == usuario.NombreUsuario);

            if (usuarioExistente == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(usuario);
            }

            // Autenticación con claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuarioExistente.NombreUsuario),
                new Claim(ClaimTypes.Role, usuarioExistente.Rol.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Mantiene la sesión abierta
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("NombreCompleto,NombreUsuario,Correo,Telefono,Contraseña")] Usuario usuario)
        {
            ModelState.Remove(nameof(usuario.Rol));
            
            if (!ModelState.IsValid)
            {
                return View(usuario); // Devuelve la vista con los errores de validación
            }
            // Validamos que el nombre usuario no exista
            if (_context.Usuarios.Any(u => u.NombreUsuario == usuario.NombreUsuario))
            {
                ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya existe.");
                return View(usuario);
            }

            usuario.Rol = RolUsuario.Usuario;

            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
        #endregion

        #region CRUD
        [Authorize(Roles = $"{nameof(RolUsuario.Administrador)}")]
        [HttpGet]
        public async Task<IActionResult> AdministracionUsuario()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        [Authorize(Roles = $"{nameof(RolUsuario.Administrador)}")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = GetRoles();
            return View();
        }

        [Authorize(Roles = $"{nameof(RolUsuario.Administrador)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCompleto,NombreUsuario,Correo,Telefono,Contraseña,Rol")] Usuario usuarios)
        {
            // Validamos que el nombre usuario no exista
            if (_context.Usuarios.Any(u => u.NombreUsuario == usuarios.NombreUsuario))
            {
                ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya existe.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Usuario creado.";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction(nameof(AdministracionUsuario));
            }

            ViewBag.Roles = GetRoles();
            TempData["Mensaje"] = "Error al crear el usuario.";
            TempData["TipoMensaje"] = "danger";
            return View(usuarios);
        }

        [Authorize(Roles = $"{nameof(RolUsuario.Administrador)}")]
        [HttpGet]
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

        [Authorize(Roles = $"{nameof(RolUsuario.Administrador)}")]
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

                    TempData["Mensaje"] = "Usuario actualizado.";
                    TempData["TipoMensaje"] = "success";
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500, "Error de concurrencia en la base de datos.");
                }

                return RedirectToAction(nameof(AdministracionUsuario));
            }
            ViewBag.Roles = GetRoles(usuario.Rol.ToString());
            TempData["Mensaje"] = "Error al actualizar el usuario.";
            TempData["TipoMensaje"] = "danger";
            return View(usuario);
        }

        [Authorize(Roles = $"{nameof(RolUsuario.Administrador)}")]
        [HttpPost]
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

            TempData["Mensaje"] = "Usuario eliminado.";
            TempData["TipoMensaje"] = "success";

            return RedirectToAction(nameof(AdministracionUsuario));
        }
        #endregion

        #region Métodos privados
        private SelectList GetRoles(string? rolSeleccionado = null)
        {
            return new SelectList(Enum.GetValues(typeof(RolUsuario))
                .Cast<RolUsuario>()
                .Select(r => new { Value = r.ToString(), Text = r.ToString() }),
                "Value", "Text", rolSeleccionado);
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
        #endregion
    }
}
