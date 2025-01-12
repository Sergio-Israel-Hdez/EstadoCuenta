namespace EstadoCuenta_Backend.Models
{
    public class ComprasResponse
    {
        public int CompraId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public int TarjetaID { get; set; }
    }
}
