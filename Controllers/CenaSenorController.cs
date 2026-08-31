using DB_Enlace.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CenaSenorController : ControllerBase
    {
        private readonly EnlaceContext _dbContext;

        public CenaSenorController(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{mes}/{anno}")]
        public IActionResult GetPorMes(int mes, int anno)
        {
            var cena = _dbContext.CenaSenor.FirstOrDefault(c => c.Mes == mes && c.Anno == anno);
            return Ok(cena);
        }

        [HttpPut]
        [Authorize(Roles = "administrador,lidere")]
        public IActionResult Upsert([FromBody] CenaSenor cena)
        {
            if (cena == null || cena.Mes < 1 || cena.Mes > 12 || cena.Dia < 1 || cena.Dia > 31)
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "Fecha inválida" } });
            }

            var existente = _dbContext.CenaSenor.FirstOrDefault(c => c.Mes == cena.Mes && c.Anno == cena.Anno);
            if (existente == null)
            {
                existente = new CenaSenor { Mes = cena.Mes, Anno = cena.Anno, Dia = cena.Dia };
                _dbContext.CenaSenor.Add(existente);
            }
            else
            {
                existente.Dia = cena.Dia;
            }

            _dbContext.SaveChanges();
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Fecha de la Cena del Señor guardada" } });
        }

        [HttpDelete("{mes}/{anno}")]
        [Authorize(Roles = "administrador,lidere")]
        public IActionResult Delete(int mes, int anno)
        {
            var existente = _dbContext.CenaSenor.FirstOrDefault(c => c.Mes == mes && c.Anno == anno);
            if (existente != null)
            {
                _dbContext.CenaSenor.Remove(existente);
                _dbContext.SaveChanges();
            }

            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Fecha de la Cena del Señor eliminada" } });
        }
    }
}
