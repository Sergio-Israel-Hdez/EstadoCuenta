using EstadoCuenta_FrontEnd.Models;
using EstadoCuenta_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EstadoCuenta_FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient httpClient;
        private ApiClient apiClient;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            httpClient = new HttpClient();
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(int IdUsuario)
        {

            if (IdUsuario == 0)
                return RedirectToAction("Index", "Home");
            string baseUrl = Convert.ToBoolean(_configuration["IsCompose"]) ? _configuration["ApiBaseUrlCompose"] : _configuration["ApiBaseUrl"];
            EstadoCuentaResponseDTO estadocuentaResponse = await new ApiClient(httpClient).WithBaseUrl(baseUrl)
                .GetAsync<EstadoCuentaResponseDTO>("/api/EstadoCuenta/ConsultarEstadoCuenta", new { UsuarioID = IdUsuario });
            if (estadocuentaResponse == null)
                return RedirectToAction("Index", "Home");
            HttpContext.Session.SetInt32(SessionsNames.TarjetaIDKey, estadocuentaResponse.TarjetaID);
            HttpContext.Session.SetInt32(SessionsNames.UsuarioIDKey, IdUsuario);
            return RedirectToAction("Index", "Tarjeta");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
