namespace Emiliano_Chiapponi
{
    // Hago la clase publica para que se accesible desde el resto de programa. // Nombre de la clase con mayuscula para declarar el objeto con minuscula
    public class Producto
    {
        // Las propiedades privadas para que solo se pueda acceder y modificar las mismas vís los métodos SET y GET qye yo defino. // Nombre del atributo con mayuscula para definir nombre del método SET y GET en minuscula
        private long _Id; 
        private string _Descripciones;
        private double _Costo;
        private double _PrecioVenta;
        private int _Stock;
        private long _IdUsuario;


        // Constructor, sin parametros. Debe tener el mismo nombre que la clase. Inicializa, por defecto, los atributos del objeto al ser creado.
        public Producto()
        {
            this._Id = 0;
            this._Descripciones = String.Empty;
            this._Costo = 0.0;
            this._PrecioVenta = 0.0;
            this._Stock = 0;
            this._IdUsuario = 0;
        }


        // Consutructor con parámetros. Debe tener el mismo nombre que la clase. Inciaciliza, según defina el usuario, los atributos del objeto.
        public Producto(long id, string descripciones, double costo, double precioVenta, int stock, long idUsuario)
        {
            this._Id = id;
            this._Descripciones = descripciones;
            this._Costo = costo;
            this._PrecioVenta = precioVenta;
            this._Stock = stock;
            this._IdUsuario = idUsuario;
        }


        // Métodos SET y GET para los atributos del objeto. Si no tuviese estos métodos, al ser los atributos private, no tendría forma de modificar los mismos, por fuera de la clase.
        public long Id { get { return _Id; } set { _Id = value; } }
        public string Descripciones { get { return _Descripciones; } set { _Descripciones = value; } }
        public double Costo { get { return _Costo; } set { _Costo = value; } }
        public double PrecioVenta { get { return _PrecioVenta; } set { _PrecioVenta = value; } }
        public int Stock { get { return _Stock; } set { _Stock = value; } }
        public long IdUsuario { get { return _IdUsuario; } set { _IdUsuario = value; } }


        // Método para visualizar el valor de los atributos de un objeto de tipo "Producto"
        public void verAtributos()
        {
            Console.WriteLine(  "\nProducto:" +
                                "\n   id = " + _Id.ToString() +
                                "\n   descripcion = " + _Descripciones +
                                "\n   costo = " + _Costo.ToString() +
                                "\n   precioVenta = " + _PrecioVenta.ToString() +
                                "\n   stock = " + _Stock.ToString() +
                                "\n   idUsuario = " + _IdUsuario.ToString() +
                                "\n");
        }
    }
}

