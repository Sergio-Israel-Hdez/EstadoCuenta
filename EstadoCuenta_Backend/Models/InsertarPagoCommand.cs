namespace EstadoCuenta_Backend.Models
{
    public class InsertarPagoCommand
    {
        public decimal Monto { get; set; }
        public int TarjetaID { get; set; }
    }

}
