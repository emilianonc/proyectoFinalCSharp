namespace Emiliano_Chiapponi
{
    // Hago la clase publica para que se accesible desde el resto de programa. // Nombre de la clase con mayuscula para declarar el objeto con minuscula
    public class ProductoVendido
    {
        // Las propiedades privadas para que solo se pueda acceder y modificar las mismas vís los métodos SET y GET qye yo defino. // Nombre del atributo con mayuscula para definir nombre del método SET y GET en minuscula
        private long Id;
        private long IdProducto;
        private int Stock;
        private long IdVenta;


        // Constructor, sin parametros. Debe tener el mismo nombre que la clase. Inicializa, por defecto, los atributos del objeto al ser creado.
        public ProductoVendido()
        {
            this.Id = 0;
            this.IdProducto = 0;
            this.Stock = 0;
            this.IdVenta = 0;
        }


        // Consutructor con parámetros. Debe tener el mismo nombre que la clase. Inciaciliza, según defina el usuario, los atributos del objeto.
        public ProductoVendido(long id, long idProducto, int stock, long idVenta)
        {
            this.Id = id;
            this.IdProducto = idProducto;
            this.Stock = stock;
            this.IdVenta = idVenta;
        }


        // Métodos SET y GET para los atributos del objeto. Si no tuviese estos métodos, al ser los atributos private, no tendría forma de modificar los mismos, por fuera de la clase.
        public long id { get { return Id; } set { Id = value; } }
        public long idProducto { get { return IdProducto; } set { IdProducto = value; } }
        public int stock { get { return Stock; } set { Stock = value; } }
        public long idVenta { get { return IdVenta; } set { IdVenta = value; } }


        // Método para visualizar el valor de los atributos de un objeto de tipo "Venta"
        public void verAtributos()
        {
            Console.WriteLine("\nProductoVendido:" + 
                                "\n   id = " + Id.ToString() +
                                "\n   idProducto = " + IdProducto.ToString() +
                                "\n   stock = " + Stock.ToString() +
                                "\n   idVenta = " + IdVenta.ToString() +
                                "\n");
        }
    }
}
