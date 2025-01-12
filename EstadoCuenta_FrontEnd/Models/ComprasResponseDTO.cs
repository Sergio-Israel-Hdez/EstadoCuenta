namespace EstadoCuenta_FrontEnd.Models
{
    public class ComprasResponseDTO
    {
        public int CompraId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
