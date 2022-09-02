namespace Emiliano_Chiapponi.Controllers.DTOS
{
    public class PutProducto
    {
        public long Id { get; set; }
        public string Descripcion { get; set; }
        public double Costo { get; set; }
        public double PrecioVenta { get; set; }
        public int Stock { get; set; }
        public long IdUsuario { get; set; }
    }
}
