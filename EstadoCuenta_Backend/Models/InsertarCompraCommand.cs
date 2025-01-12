namespace EstadoCuenta_Backend.Models
{
    public class InsertarCompraCommand
    {
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public int TarjetaID { get; set; }
    }
}
