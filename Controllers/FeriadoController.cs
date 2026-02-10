using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoFreeTime.DB;
using NoFreeTime.Entity;

namespace NoFreeTime.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeriadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeriadoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("verificar")]
        public async Task<IActionResult> VerificarDia([FromQuery] string fecha = null)
        {
            try
            {
                DateTime fechaVerificar;

                if (string.IsNullOrEmpty(fecha))
                {
                    fechaVerificar = DateTime.Today;
                }
                else
                {
                    fechaVerificar = DateTime.Parse(fecha);
                }

                if (fechaVerificar.DayOfWeek == DayOfWeek.Saturday ||
                    fechaVerificar.DayOfWeek == DayOfWeek.Sunday)
                {
                    return Ok("Libre");
                }

                var feriado = await _context.Feriados
                    .FirstOrDefaultAsync(f => f.Fecha.Date == fechaVerificar.Date);

                if (feriado != null)
                {
                    return Ok("Libre");
                }

                return Ok("Trabaja");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarFeriado([FromBody] Feriado nuevoFeriado)
        {
            try
            {
                _context.Feriados.Add(nuevoFeriado);
                await _context.SaveChangesAsync();
                return Ok($"Feriado '{nuevoFeriado.Nombre}' agregado para {nuevoFeriado.Fecha:yyyy-MM-dd}");
            }
            catch (Exception)
            {
                return BadRequest("Error al agregar feriado");
            }
        }

        [HttpGet("todos")]
        public async Task<IActionResult> ObtenerFeriados()
        {
            var feriados = await _context.Feriados.ToListAsync();
            return Ok(feriados);
        }
    }
}
