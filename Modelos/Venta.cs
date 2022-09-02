namespace Emiliano_Chiapponi
{
    // Hago la clase publica para que se accesible desde el resto de programa. // Nombre de la clase con mayuscula para declarar el objeto con minuscula
    public class Venta
    {
        // Las propiedades privadas para que solo se pueda acceder y modificar las mismas vís los métodos SET y GET qye yo defino. // Nombre del atributo con mayuscula para definir nombre del método SET y GET en minuscula
        private long Id;
        private string Comentarios;


        // Constructor, sin parametros. Debe tener el mismo nombre que la clase. Inicializa, por defecto, los atributos del objeto al ser creado.
        public Venta()
        {
            this.Id = 0;
            this.Comentarios = String.Empty;
        }


        // Consutructor con parámetros. Debe tener el mismo nombre que la clase. Inciaciliza, según defina el usuario, los atributos del objeto.
        public Venta(long id, string comentarios)
        {
            this.Id = id;
            this.Comentarios = comentarios;
        }


        // Métodos SET y GET para los atributos del objeto. Si no tuviese estos métodos, al ser los atributos private, no tendría forma de modificar los mismos, por fuera de la clase.
        public long id { get { return Id; } set { Id = value; } }
        public string comentarios { get { return Comentarios; } set { Comentarios = value; } }


        // Método para visualizar el valor de los atributos de un objeto de tipo "Venta"
        public void verAtributos()
        {
            Console.WriteLine("\nVenta:" + 
                                "\n   id = " + Id.ToString() +
                                "\n   comentarios = " + Comentarios +
                                "\n");
        }
    }
}

