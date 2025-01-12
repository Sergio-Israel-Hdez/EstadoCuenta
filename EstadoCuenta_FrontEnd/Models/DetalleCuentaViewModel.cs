using EstadoCuenta_FrontEnd.Models;

namespace EstadoCuenta_FrontEnd.Models
{
    public class DetalleCuentaViewModel
    {
        public EstadoCuentaResponseDTO estadoCuenta { get; set; }
        public List<TransaccionesMensualesResponseDTO>? transaccionesMensuales { get; set; } 
        public List<ComprasResponseDTO>? compras { get; set; }
    }
}
