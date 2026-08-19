using DB_Enlace.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RolesMesController : ControllerBase
    {
        private readonly IRolesMesService _rolesMesService;

        public RolesMesController(IRolesMesService rolesMesService)
        {
            _rolesMesService = rolesMesService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_rolesMesService.GetAll());
        }

        [HttpGet("{mes}/{anno}")]
        public IActionResult GetPorMes(int mes, int anno)
        {
            return Ok(_rolesMesService.GetPorMes(mes, anno));
        }

        [HttpPost]
        public IActionResult Create([FromBody] RolesMes nuevo)
        {
            _rolesMesService.Create(nuevo);
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación guardada" } });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] RolesMes actualizado)
        {
            _rolesMesService.Update(id, actualizado);
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación actualizada" } });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _rolesMesService.Delete(id);
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación eliminada" } });
        }
    }
}
