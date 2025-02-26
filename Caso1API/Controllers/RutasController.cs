using Caso1API.Models.DbConContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caso1API.Controllers
{
    [Route("api/rutas")]
    [ApiController]
    public class RutasController : ControllerBase
    {
        private readonly Caso1APIContext _context;
        public RutasController(Caso1APIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListRutas()
        {
            IEnumerable<Rutas> Rutas = _context.Rutas;
            if (Rutas.Any())
                return Ok(Rutas);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetRuta(int id)
        {
            Rutas? Ruta = _context.Rutas.Find(id);
            if (Ruta != null)
                return Ok(Ruta);
            else
                return NotFound();
        }
    }
}