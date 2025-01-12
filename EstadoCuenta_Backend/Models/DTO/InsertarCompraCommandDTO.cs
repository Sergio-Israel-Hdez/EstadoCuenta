namespace EstadoCuenta_Backend.Models.DTO
{
    public class InsertarCompraCommandDTO
    {
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public int TarjetaID { get; set; }
    }
}
