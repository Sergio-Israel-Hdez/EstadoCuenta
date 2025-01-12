namespace EstadoCuenta_Backend.Models.DTO
{
    public class EstadoCuentaResponseDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int TarjetaID { get; set; }
        public string NumeroTarjeta { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal SaldoDisponible { get; set; }
        public decimal ComprasMesActual { get; set; }
        public decimal ComprasMesAnterior { get; set; }
        public decimal InteresBonificable { get; set; }
        public decimal CuotaMinima { get; set; }
        public decimal MontoTotalPagar { get; set; }
        public decimal PagoContadoConIntereses { get; set; }
    }
}
