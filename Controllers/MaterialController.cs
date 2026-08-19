using DB_Enlace.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok(_materialService.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(Guid id)
        {
            var material = _materialService.GetById(id);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }

        [HttpGet("{id}/descargar")]
        [Authorize]
        public IActionResult Descargar(Guid id)
        {
            var material = _materialService.GetById(id);
            if (material == null || material.Contenido == null || material.Contenido.Length == 0)
            {
                return NotFound();
            }

            return File(material.Contenido, material.ContentType ?? "application/pdf", material.Nombre ?? "material");
        }

        [HttpPost]
        [Authorize(Roles = "administrador,lidere")]
        public async Task<IActionResult> Upload(
            [FromForm] IFormFile archivo,
            [FromForm] string? nombre,
            [FromForm] string? descripcion)
        {
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest(new ApiResponse
                {
                    status = "error",
                    result = new { mensaje = "Debes seleccionar un archivo" }
                });
            }

            using var ms = new MemoryStream();
            await archivo.CopyToAsync(ms);

            var material = new Material
            {
                MaterialId = Guid.NewGuid(),
                Nombre = string.IsNullOrWhiteSpace(nombre) ? archivo.FileName : nombre,
                Descripcion = descripcion,
                Fecha = DateTime.Now,
                Contenido = ms.ToArray(),
                ContentType = archivo.ContentType ?? "application/pdf",
                Tamano = archivo.Length
            };

            _materialService.Create(material);

            return Ok(new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Material subido con éxito", materialId = material.MaterialId }
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrador,lidere")]
        public IActionResult Update(Guid id, [FromBody] Material materialActualizado)
        {
            _materialService.Update(id, materialActualizado);

            return Ok(new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Material actualizado con éxito" }
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador,lidere")]
        public IActionResult Delete(Guid id)
        {
            _materialService.Delete(id);

            return Ok(new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Material eliminado con éxito" }
            });
        }
    }
}
