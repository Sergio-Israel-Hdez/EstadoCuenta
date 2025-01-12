namespace EstadoCuenta_FrontEnd.Models
{
    public class TransaccionesMensualesResponseDTO
    {
        public string TipoTransaccion { get; set; }
        public int TransaccionID { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
