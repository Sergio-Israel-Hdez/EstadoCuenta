using EstadoCuenta_FrontEnd.Models;

namespace EstadoCuenta_FrontEnd.Services
{
    public class TransaccionesService
    {
        private HttpClient httpClient;
        private ApiClient apiClient;
        private readonly IConfiguration _configuration;
        public TransaccionesService(IConfiguration configuration)
        {
            httpClient = new HttpClient();
            _configuration = configuration;
        }
        public async Task<DetalleCuentaViewModel> detalleCuentaAsync(int idUsuario)
        {
            string baseUrl = Convert.ToBoolean(_configuration["IsCompose"]) ? _configuration["ApiBaseUrlCompose"] : _configuration["ApiBaseUrl"];
            EstadoCuentaResponseDTO estadoCuentaResponse = await new ApiClient(httpClient).WithBaseUrl(baseUrl)
                .GetAsync<EstadoCuentaResponseDTO>("/api/EstadoCuenta/ConsultarEstadoCuenta", new EstadoCuentaQuery { UsuarioID = idUsuario });
            if (estadoCuentaResponse == null)
                return null;
            List<TransaccionesMensualesResponseDTO> transaccionesMensualesResponse = await new ApiClient(httpClient).WithBaseUrl(baseUrl)
                .GetAsync<List<TransaccionesMensualesResponseDTO>>("/api/Transacciones/ConsultarTransaccionesMensuales", new TransaccionesMensualesQuery { TarjetaID = estadoCuentaResponse.TarjetaID });
            List<ComprasResponseDTO> comprasResponses = await new ApiClient(httpClient).WithBaseUrl(baseUrl)
                .GetAsync<List<ComprasResponseDTO>>("/api/Transacciones/ConsultarCompras", new ComprasQuery { TarjetaID = estadoCuentaResponse.TarjetaID });
            return new DetalleCuentaViewModel
            {
                compras = comprasResponses,
                estadoCuenta = estadoCuentaResponse,
                transaccionesMensuales = transaccionesMensualesResponse
            };
        }
    }
}
