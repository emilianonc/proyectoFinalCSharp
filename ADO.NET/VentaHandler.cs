
// Debo agregar el paquete SqlClient al proyecto: Click derecho en el proyecto > Administrar paquetes Nuget > Sarch : SqlClient > System.Data.SqlClient > Install

using System.Data.SqlClient; // Para tener acceso a los objetos: SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter
using System.Data;

namespace Emiliano_Chiapponi
{

    public static class VentaHandler// Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "Venta"
    {

        public const string connectionString = "Server=DESKTOP-2QV2INM;Database=SistemaGestion;Trusted_Connection=True";

        public static List<Venta> TraerVentas(long idUsuario) // Traer Ventas: método que debe traer todas las ventas de la base que contienen productos de un determinado Usuario.
        {
            List<Venta> ventas = new List<Venta>(); // Creo una lista de objetos de clase "Venta". Va a ser lo que devuelva el método.
        
            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                const string query =    "SELECT v.Id, v.Comentarios " + // Query que me devuelve las ventas que contienen productos con IdUsuario = idUsuario.
                                        "FROM Venta AS v " +
                                        "INNER JOIN ProductoVendido AS pv " +
                                        "ON v.Id = pv.IdVenta " +
                                        "INNER JOIN Producto AS p " +
                                        "ON pv.IdProducto = p.Id " +
                                        "WHERE p.IdUsuario = @idUsuario ";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) // Creo un objeto SqlCommand con una query definida previamente.
                {
                    var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                    parameterIdUsuario.Value = idUsuario;
                    sqlCommand.Parameters.Add(parameterIdUsuario);

                    sqlConnection.Open(); // Abro la conexión con la BD

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD.
                    {
                        if (dataReader.HasRows) // Me aseguro que haya filas para leer .
                        {
                            while (dataReader.Read()) // Leo una fila.
                            {
                                Venta venta = new Venta(); // Creo un nuevo objeto de clase Venta.

                                // Actualizo todos los atributos de venta con los valores obtenidos de la BD.
                                venta.id = Convert.ToInt32(dataReader["Id"]);
                                venta.comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta); // Agrego el objeto "venta" a la lista "ventas".
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return ventas;
        }


        
        public static long CargarVenta(Venta venta) // Cargar venta: se utiliza para poder hacer la primera parte de CargarVentaProyecto();
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no cargar la venta.
            long idVenta = 0; // Creo una variable a la cual le asigno el valor de Id de la venta cargada.

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Venta] (Comentarios) " + // Query que me permite agregar una Venta.
                                        "VALUES (@comentarios) " +
                                        "SELECT @@IDENTITY";

                var parameterComentarios = new SqlParameter("comentarios", SqlDbType.VarChar);
                parameterComentarios.Value = venta.comentarios;

                sqlConnection.Open(); // Abro la conexión con la BD.

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Producto.
                {
                    sqlCommand.Parameters.Add(parameterComentarios);
                    idVenta = Convert.ToInt64(sqlCommand.ExecuteScalar());
                }

                sqlConnection.Close(); // Cierro la conexión con la BD.
            }
            return idVenta;
        }



        /*public List<Venta> TraerVentas() // Método que trae todas las ventas de la BD
        {
            List<Venta> ventas = new List<Venta>(); // Creo una lista de objetos de clase Venta. Va a ser lo que devuelva el método

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Venta]", sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Venta
                {
                    sqlConnection.Open(); // Abro la conexión con la BD

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD
                    {
                        if (dataReader.HasRows) // Me aseguro que haya filas para leer 
                        {
                            while (dataReader.Read()) // Leo una fila
                            {
                                Venta venta = new Venta(); // Creo un nuevo objeto de clase Venta

                                // Actualizo todos los atributos de venta con los valores obtenidos de la BD
                                venta.id = Convert.ToInt32(dataReader["Id"]);
                                venta.comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta); // Agrego el objeto "venta" a la lista "ventas"
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD
                }
            }
            return ventas;
        }*/



        // Permite ver por consola los atributos de los objetos ProductoVendido contenidos en la lista que se pasa como argumento.
        public static void MostrarVentas(List<Venta> ventas)
        {
            Console.WriteLine("Venta en BD:");
            foreach (Venta item in ventas)
            {
                Console.WriteLine("id: " + item.id.ToString() +
                                    "\tcomentarios: " + item.comentarios.ToString());
            }
            Console.WriteLine(" "); // Solo para separar de la próxima linea que se imprima
        }
    }
}