namespace Emiliano_Chiapponi
{
    // Hago la clase publica para que se accesible desde el resto de programa. // Nombre de la clase con mayuscula para declarar el objeto con minuscula
    public class Usuario 
    {
        // Las propiedades privadas para que solo se pueda acceder y modificar las mismas vís los métodos SET y GET qye yo defino. // Nombre del atributo con mayuscula para definir nombre del método SET y GET en minuscula
        private long _Id; 
        private string _Nombre;
        private string _Apellido;
        private string _NombreUsuario;
        private string _Contraseña;
        private string _Mail;


        // Constructor, sin parametros. Debe tener el mismo nombre que la clase. Inicializa, por defecto, los atributos del objeto al ser creado.
        public Usuario() 
        {
            this._Id = 0;
            this._Nombre = String.Empty;
            this._Apellido = String.Empty;
            this._NombreUsuario = String.Empty;
            this._Contraseña = String.Empty;
            this._Mail = String.Empty;
        }


        // Consutructor con parámetros. Debe tener el mismo nombre que la clase. Inciaciliza, según defina el usuario, los atributos del objeto.
        public Usuario(long id, string nombre, string apellido, string nombreUsuario, string contraseña, string mail) 
        {
            this._Id = id;
            this._Nombre = nombre;
            this._Apellido = apellido;
            this._NombreUsuario = nombreUsuario;
            this._Contraseña = contraseña;
            this._Mail = mail;
        }


        // Métodos SET y GET para los atributos del objeto. Si no tuviese estos métodos, al ser los atributos private, no tendría forma de modificar los mismos, por fuera de la clase.
        public long Id { get{ return _Id; } set{ _Id = value; } }
        public string Nombre { get{ return _Nombre;} set { _Nombre = value;} }
        public string Apellido { get{ return _Apellido;} set { _Apellido = value; } }
        public string NombreUsuario { get { return _NombreUsuario; } set { _NombreUsuario = value; } }
        public string Contraseña { get { return _Contraseña; } set { _Contraseña = value; } }
        public string Mail { get { return _Mail; } set { _Mail = value; } }


        // Método para visualizar el valor de los atributos de un objeto de tipo "Usuario"
        public void verAtributos()
        {
            Console.WriteLine("\nUsuario" + 
                                "\n   id = " + _Id.ToString() +
                                "\n   nombre = " + _Nombre +
                                "\n   apellido = " + _Apellido +
                                "\n   nombreUsuario = " + _NombreUsuario +
                                "\n   constraseña = " + _Contraseña +
                                "\n   mail = " + _Mail +
                                "\n");
        }
    }
}
