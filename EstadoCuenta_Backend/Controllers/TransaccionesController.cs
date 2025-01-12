using EstadoCuenta_Backend.Handlers;
using EstadoCuenta_Backend.Models;
using EstadoCuenta_Backend.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EstadoCuenta_Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly ILogger<TransaccionesController> logger;
        private readonly InsertarCompraCommandHandler _insertarCompraCommandHandler;
        private readonly InsertarPagoCommandHandler _insertarPagoCommandHandler;
        private readonly TransaccionesMensualesQueryHandler _transaccionesMensualesQueryHandler;
        private readonly ComprasQueryHandler _comprasQueryHandler;
        public TransaccionesController(ILogger<TransaccionesController> logger,
            InsertarCompraCommandHandler insertarCompraCommandHandler,
            InsertarPagoCommandHandler insertarPagoCommandHandler,
            TransaccionesMensualesQueryHandler transaccionesMensualesQueryHandler,
            ComprasQueryHandler comprasQueryHandler)
        {
            this.logger = logger;
            _insertarCompraCommandHandler = insertarCompraCommandHandler;
            _insertarPagoCommandHandler = insertarPagoCommandHandler;
            _transaccionesMensualesQueryHandler = transaccionesMensualesQueryHandler;
            _comprasQueryHandler = comprasQueryHandler;
        }
        [HttpPost]
        public async Task<IActionResult> InsertarCompra([FromBody] InsertarCompraCommandDTO command)
        {
            try
            {
                await _insertarCompraCommandHandler.HandleAsync(command);
                return Ok();
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error de base de datos: {ex.Message}");
                return StatusCode(500, new { Message = "Error al insertar compra" });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inesperado: {ex.Message}");
                return StatusCode(500, new { Message = "Ocurrió un error inesperado. Por favor, intente más tarde." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertarPago([FromBody] InsertarPagoCommandDTO command)
        {
            try
            {
                await _insertarPagoCommandHandler.HandleAsync(command);
                return Ok();
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error de base de datos: {ex.Message}");
                return StatusCode(500, new { Message = "Error al insertar pago" });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inesperado: {ex.Message}");
                return StatusCode(500, new { Message = "Ocurrió un error inesperado. Por favor, intente más tarde." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ConsultarTransaccionesMensuales([FromQuery] TransaccionesMensualesQuery query)
        {
            try
            {
                var response = await _transaccionesMensualesQueryHandler.HandleAsync(query);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error de base de datos: {ex.Message}");
                return StatusCode(500, new { Message = "Error al consultar transacciones mensuales" });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inesperado: {ex.Message}");
                return StatusCode(500, new { Message = "Ocurrió un error inesperado. Por favor, intente más tarde." });
            }
        }
        [HttpGet]   
        public async Task<IActionResult> ConsultarCompras([FromQuery] ComprasQuery query)
        {
            try
            {
                var response = await _comprasQueryHandler.HandleAsync(query);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error de base de datos: {ex.Message}");
                return StatusCode(500, new { Message = "Error al consultar compras" });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inesperado: {ex.Message}");
                return StatusCode(500, new { Message = "Ocurrió un error inesperado. Por favor, intente más tarde." });
            }
        }
    }
}
