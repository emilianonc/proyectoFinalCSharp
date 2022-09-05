namespace Emiliano_Chiapponi
{
    // Hago la clase publica para que se accesible desde el resto de programa. // Nombre de la clase con mayuscula para declarar el objeto con minuscula
    public class ProductoVendido
    {
        // Las propiedades privadas para que solo se pueda acceder y modificar las mismas vís los métodos SET y GET qye yo defino. // Nombre del atributo con mayuscula para definir nombre del método SET y GET en minuscula
        private long _Id;
        private long _IdProducto;
        private int _Stock;
        private long _IdVenta;


        // Constructor, sin parametros. Debe tener el mismo nombre que la clase. Inicializa, por defecto, los atributos del objeto al ser creado.
        public ProductoVendido()
        {
            this._Id = 0;
            this._IdProducto = 0;
            this._Stock = 0;
            this._IdVenta = 0;
        }


        // Consutructor con parámetros. Debe tener el mismo nombre que la clase. Inciaciliza, según defina el usuario, los atributos del objeto.
        public ProductoVendido(long id, long idProducto, int stock, long idVenta)
        {
            this._Id = id;
            this._IdProducto = idProducto;
            this._Stock = stock;
            this._IdVenta = idVenta;
        }


        // Métodos SET y GET para los atributos del objeto. Si no tuviese estos métodos, al ser los atributos private, no tendría forma de modificar los mismos, por fuera de la clase.
        public long Id { get { return _Id; } set { _Id = value; } }
        public long IdProducto { get { return _IdProducto; } set { _IdProducto = value; } }
        public int Stock { get { return _Stock; } set { _Stock = value; } }
        public long IdVenta { get { return _IdVenta; } set { _IdVenta = value; } }


        // Método para visualizar el valor de los atributos de un objeto de tipo "Venta"
        public void verAtributos()
        {
            Console.WriteLine("\nProductoVendido:" + 
                                "\n   id = " + _Id.ToString() +
                                "\n   idProducto = " + _IdProducto.ToString() +
                                "\n   stock = " + _Stock.ToString() +
                                "\n   idVenta = " + _IdVenta.ToString() +
                                "\n");
        }
    }
}
