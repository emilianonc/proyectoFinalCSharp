namespace Emiliano_Chiapponi
{
    // Hago la clase publica para que se accesible desde el resto de programa. // Nombre de la clase con mayuscula para declarar el objeto con minuscula
    public class Producto
    {
        // Las propiedades privadas para que solo se pueda acceder y modificar las mismas vís los métodos SET y GET qye yo defino. // Nombre del atributo con mayuscula para definir nombre del método SET y GET en minuscula
        private long Id; 
        private string Descripcion;
        private double Costo;
        private double PrecioVenta;
        private int Stock;
        private long IdUsuario;


        // Constructor, sin parametros. Debe tener el mismo nombre que la clase. Inicializa, por defecto, los atributos del objeto al ser creado.
        public Producto()
        {
            this.Id = 0;
            this.Descripcion = String.Empty;
            this.Costo = 0.0;
            this.PrecioVenta = 0.0;
            this.Stock = 0;
            this.IdUsuario = 0;
        }


        // Consutructor con parámetros. Debe tener el mismo nombre que la clase. Inciaciliza, según defina el usuario, los atributos del objeto.
        public Producto(long id, string descripcion, double costo, double precioVenta, int stock, long idUsuario)
        {
            this.Id = id;
            this.Descripcion = descripcion;
            this.Costo = costo;
            this.PrecioVenta = precioVenta;
            this.Stock = stock;
            this.IdUsuario = idUsuario;
        }


        // Métodos SET y GET para los atributos del objeto. Si no tuviese estos métodos, al ser los atributos private, no tendría forma de modificar los mismos, por fuera de la clase.
        public long id { get { return Id; } set { Id = value; } }
        public string descripcion { get { return Descripcion; } set { Descripcion = value; } }
        public double costo { get { return Costo; } set { Costo = value; } }
        public double precioVenta { get { return PrecioVenta; } set { PrecioVenta = value; } }
        public int stock { get { return Stock; } set { Stock = value; } }
        public long idUsuario { get { return IdUsuario; } set { IdUsuario = value; } }


        // Método para visualizar el valor de los atributos de un objeto de tipo "Producto"
        public void verAtributos()
        {
            Console.WriteLine(  "\nProducto:" +
                                "\n   id = " + Id.ToString() +
                                "\n   descripcion = " + Descripcion +
                                "\n   costo = " + Costo.ToString() +
                                "\n   precioVenta = " + PrecioVenta.ToString() +
                                "\n   stock = " + Stock.ToString() +
                                "\n   idUsuario = " + IdUsuario.ToString() +
                                "\n");
        }
    }
}

