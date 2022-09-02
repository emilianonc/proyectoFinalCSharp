namespace Emiliano_Chiapponi
{
    // Hago la clase publica para que se accesible desde el resto de programa. // Nombre de la clase con mayuscula para declarar el objeto con minuscula
    public class Usuario 
    {
        // Las propiedades privadas para que solo se pueda acceder y modificar las mismas vís los métodos SET y GET qye yo defino. // Nombre del atributo con mayuscula para definir nombre del método SET y GET en minuscula
        private long Id; 
        private string Nombre;
        private string Apellido;
        private string NombreUsuario;
        private string Contraseña;
        private string Mail;


        // Constructor, sin parametros. Debe tener el mismo nombre que la clase. Inicializa, por defecto, los atributos del objeto al ser creado.
        public Usuario() 
        {
            this.Id = 0;
            this.Nombre = String.Empty;
            this.Apellido = String.Empty;
            this.NombreUsuario = String.Empty;
            this.Contraseña = String.Empty;
            this.Mail = String.Empty;
        }


        // Consutructor con parámetros. Debe tener el mismo nombre que la clase. Inciaciliza, según defina el usuario, los atributos del objeto.
        public Usuario(long id, string nombre, string apellido, string nombreUsuario, string contraseña, string mail) 
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.NombreUsuario = nombreUsuario;
            this.Contraseña = contraseña;
            this.Mail = mail;
        }


        // Métodos SET y GET para los atributos del objeto. Si no tuviese estos métodos, al ser los atributos private, no tendría forma de modificar los mismos, por fuera de la clase.
        public long id { get{ return Id; } set{ Id = value; } }
        public string nombre { get{ return Nombre;} set { Nombre = value;} }
        public string apellido { get{ return Apellido;} set { Apellido = value; } }
        public string nombreUsuario { get { return NombreUsuario; } set { NombreUsuario = value; } }
        public string contraseña { get { return Contraseña; } set { Contraseña = value; } }
        public string mail { get { return Mail; } set { Mail = value; } }


        // Método para visualizar el valor de los atributos de un objeto de tipo "Usuario"
        public void verAtributos()
        {
            Console.WriteLine("\nUsuario" + 
                                "\n   id = " + Id.ToString() +
                                "\n   nombre = " + Nombre +
                                "\n   apellido = " + Apellido +
                                "\n   nombreUsuario = " + NombreUsuario +
                                "\n   constraseña = " + Contraseña +
                                "\n   mail = " + Mail +
                                "\n");
        }
    }
}
