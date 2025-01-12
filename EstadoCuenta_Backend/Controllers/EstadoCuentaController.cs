using EstadoCuenta_Backend.Handlers;
using EstadoCuenta_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EstadoCuenta_Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EstadoCuentaController : ControllerBase
    {
        private readonly ILogger<EstadoCuentaController> _logger;
        private readonly EstadoCuentaQueryHandler _consultarEstadoCuentaQueryHandler;

        public EstadoCuentaController(ILogger<EstadoCuentaController> logger,EstadoCuentaQueryHandler estadoCuentaQueryHandler)
        {
            _logger = logger;
            _consultarEstadoCuentaQueryHandler = estadoCuentaQueryHandler;  
        }
        [HttpGet]
        public async Task<IActionResult> ConsultarEstadoCuenta([FromQuery] EstadoCuentaQuery query)
        {
            try
            {
                var response = await _consultarEstadoCuentaQueryHandler.HandleAsync(query);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Error de base de datos: {ex.Message}");
                return StatusCode(500,new {Mensaje = "Error al consultar estado de cuenta"});
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inesperado: {ex.Message}");
                return StatusCode(500, new { Message = "Ocurrió un error inesperado. Por favor, intente más tarde." });
            }
        }
    }
}
