namespace EstadoCuenta_Backend.Models.DTO
{
    public class InsertarPagoCommandDTO
    {
        public decimal Monto { get; set; }
        public int TarjetaID { get; set; }
    }
}
