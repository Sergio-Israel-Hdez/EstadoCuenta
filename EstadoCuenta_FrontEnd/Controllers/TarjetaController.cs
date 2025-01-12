using ClosedXML.Excel;
using EstadoCuenta_FrontEnd.Models;
using EstadoCuenta_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstadoCuenta_FrontEnd.Controllers
{
    public class TarjetaController : Controller
    {
        IConfiguration _configuration;
        public TarjetaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetInt32(SessionsNames.UsuarioIDKey).ToString()))
                return RedirectToAction("Index", "Home");
            DetalleCuentaViewModel detalleCuentaView = await new TransaccionesService(_configuration)
                .detalleCuentaAsync(HttpContext.Session.GetInt32(SessionsNames.UsuarioIDKey).Value);
            return View(detalleCuentaView);
        }
        public IActionResult InsertarCompra()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> InsertarCompra(InsertarCompraCommandDTO comprasRequest)
        {
            comprasRequest.TarjetaID = HttpContext.Session.GetInt32(SessionsNames.TarjetaIDKey).Value;
            string baseUrl = Convert.ToBoolean(_configuration["IsCompose"]) ? _configuration["ApiBaseUrlCompose"] : _configuration["ApiBaseUrl"];
            try
            {
                var (response, errors) = await new ApiClient(new HttpClient()).WithBaseUrl(baseUrl)
                .PostAsync<InsertarCompraCommandDTO, object>("/api/Transacciones/InsertarCompra", comprasRequest);
                if (errors != null)
                {
                    ViewBag.Error = errors.SelectMany(e => e.Value).ToList();
                    return View(comprasRequest);
                }
                return RedirectToAction("Index", "Tarjeta");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = new List<string> { "Ocurrió un error inesperado." };
                return View(comprasRequest);
            }

        }
        public IActionResult InsertarPago()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> InsertarPago(InsertarPagoCommandDTO pagoRequest)
        {
            pagoRequest.TarjetaID = HttpContext.Session.GetInt32(SessionsNames.TarjetaIDKey).Value;
            string baseUrl = Convert.ToBoolean(_configuration["IsCompose"]) ? _configuration["ApiBaseUrlCompose"] : _configuration["ApiBaseUrl"];
            try
            {
                var (response, errors) = await new ApiClient(new HttpClient()).WithBaseUrl(baseUrl)
                .PostAsync<InsertarPagoCommandDTO, object>("/api/Transacciones/InsertarPago", pagoRequest);
                if (errors != null)
                {
                    ViewBag.Error = errors.SelectMany(e => e.Value).ToList();
                    return View(pagoRequest);
                }
                return RedirectToAction("Index", "Tarjeta");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = new List<string> { "Ocurrió un error inesperado." };
                return View(pagoRequest);
            }

        }
        [HttpGet]
        public async Task<IActionResult> ExportarEstadoCuenta()
        {
            if (HttpContext.Session.GetInt32(SessionsNames.UsuarioIDKey)?.ToString() == null)
                return RedirectToAction("Index", "Home");
            DetalleCuentaViewModel detalleCuentaView = await new TransaccionesService(_configuration)
                .detalleCuentaAsync(HttpContext.Session.GetInt32(SessionsNames.UsuarioIDKey).Value);
            using (var workbook = new XLWorkbook())
            {
                // Crear hojas
                var estadoCuentaSheet = workbook.Worksheets.Add("EstadoCuenta");
                var transaccionesSheet = workbook.Worksheets.Add("TransaccionesMensuales");
                var comprasSheet = workbook.Worksheets.Add("Compras");

                // 1. Llenar la hoja "EstadoCuenta"
                estadoCuentaSheet.Cell(1, 1).Value = "Propiedad";
                estadoCuentaSheet.Cell(1, 2).Value = "Valor";
                estadoCuentaSheet.Cell(2, 1).Value = "Nombre";
                estadoCuentaSheet.Cell(2, 2).Value = detalleCuentaView.estadoCuenta.Nombre;
                estadoCuentaSheet.Cell(3, 1).Value = "Apellido";
                estadoCuentaSheet.Cell(3, 2).Value = detalleCuentaView.estadoCuenta.Apellido;
                estadoCuentaSheet.Cell(4, 1).Value = "Tarjeta ID";
                estadoCuentaSheet.Cell(4, 2).Value = detalleCuentaView.estadoCuenta.TarjetaID;
                estadoCuentaSheet.Cell(5, 1).Value = "Saldo Actual";
                estadoCuentaSheet.Cell(5, 2).Value = detalleCuentaView.estadoCuenta.SaldoActual;

                // Ajustar el ancho de las columnas
                estadoCuentaSheet.Columns().AdjustToContents();

                // 2. Llenar la hoja "TransaccionesMensuales"
                transaccionesSheet.Cell(1, 1).Value = "Tipo Transacción";
                transaccionesSheet.Cell(1, 2).Value = "Fecha";
                transaccionesSheet.Cell(1, 3).Value = "Descripción";
                transaccionesSheet.Cell(1, 4).Value = "Monto";

                if (detalleCuentaView.transaccionesMensuales != null)
                {
                    for (int i = 0; i < detalleCuentaView.transaccionesMensuales.Count; i++)
                    {
                        var transaccion = detalleCuentaView.transaccionesMensuales[i];
                        transaccionesSheet.Cell(i + 2, 1).Value = transaccion.TipoTransaccion;
                        transaccionesSheet.Cell(i + 2, 2).Value = transaccion.Fecha.ToString("yyyy-MM-dd");
                        transaccionesSheet.Cell(i + 2, 3).Value = transaccion.Descripcion;
                        transaccionesSheet.Cell(i + 2, 4).Value = transaccion.Monto;
                    }
                }

                transaccionesSheet.Columns().AdjustToContents();

                // 3. Llenar la hoja "Compras"
                comprasSheet.Cell(1, 1).Value = "Descripción";
                comprasSheet.Cell(1, 2).Value = "Fecha";
                comprasSheet.Cell(1, 3).Value = "Monto";

                if (detalleCuentaView.compras != null)
                {
                    for (int i = 0; i < detalleCuentaView.compras.Count; i++)
                    {
                        var compra = detalleCuentaView.compras[i];
                        comprasSheet.Cell(i + 2, 1).Value = compra.Descripcion;
                        comprasSheet.Cell(i + 2, 2).Value = compra.Fecha.ToString("yyyy-MM-dd");
                        comprasSheet.Cell(i + 2, 3).Value = compra.Monto;
                    }
                }

                comprasSheet.Columns().AdjustToContents();

                // Guardar en memoria y devolver como archivo
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DetalleCuenta.xlsx");
                }
            }
        }
    }
}