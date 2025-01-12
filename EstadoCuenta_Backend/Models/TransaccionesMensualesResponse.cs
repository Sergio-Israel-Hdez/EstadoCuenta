namespace EstadoCuenta_Backend.Models
{
    public class TransaccionesMensualesResponse
    {
        public string TipoTransaccion { get; set; }
        public int TransaccionID { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        
    }
}
