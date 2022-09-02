
// Debo agregar el paquete SqlClient al proyecto: Click derecho en el proyecto > Administrar paquetes Nuget > Sarch : SqlClient > System.Data.SqlClient > Install

using System.Data.SqlClient; // Para tener acceso a los objetos: SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter
using System.Data;

namespace Emiliano_Chiapponi
{

    public static class ProductoHandler // Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "Producto"
    {
        
        public const string connectionString = "Server=DESKTOP-2QV2INM;Database=SistemaGestion;Trusted_Connection=True";

        private static Producto InicializarProductoDesdeBD(SqlDataReader dataReader) // Método que crea e inicializa un objeto de clase Producto con los valores provistos por la BD.
        {
            Producto nuevoProducto = new Producto(
                                            Convert.ToInt64(dataReader["Id"]),
                                            dataReader["Descripciones"].ToString(),
                                            Convert.ToDouble(dataReader["Costo"]),
                                            Convert.ToDouble(dataReader["PrecioVenta"]),
                                            Convert.ToInt32(dataReader["Stock"]),
                                            Convert.ToInt64(dataReader["IdUsuario"]));
            return nuevoProducto;
        }



        public static List<Producto> TraerProductos() // Traer Productos: Método que debe traer todos los productos cargados en la base.
        {
            List<Producto> listaProductos = new List<Producto>(); // Creo una lista de productos. Va a ser lo que devuelva el método.
            
            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Producto]", sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Producto.
                {
                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD.
                    {
                        if (sqlDataReader.HasRows) // Me aseguro que haya filas para leer.
                        {
                            while (sqlDataReader.Read()) // En cada fila leida.
                            {
                                Producto producto = InicializarProductoDesdeBD(sqlDataReader); // Creo objeto producto para alamcenar los atributos leidos en la fila actual de la BD.

                                listaProductos.Add(producto); // Agrego el objeto producto a la lista "listaProductos".
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return listaProductos;
        }



        public static List<Producto> TraerProductosConIdUsuario(long idUsuario) // Traer Productos: Método que debe traer todos los productos cargados en la basecuyo IdUsuario = idUsuario
        {
            List<Producto> listaProductos = new List<Producto>(); // Creo una lista de productos. Va a ser lo que devuelva el método.

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE IdUsuario = @idUsuario", sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Producto cuyo valor de IdUsuario = idUsuario.
                {
                    var sqlParameter = new SqlParameter();          // Creo un nuevo objeto SqlParameter, para expecificar "@idUsuario" en la query utilizada en el objeto SqlCommand.
                    sqlParameter.ParameterName = "idUsuario";       // Asigno nombre a sqlParameter.
                    sqlParameter.SqlDbType = SqlDbType.BigInt;     // Asigno el tipo de dato que tiene sqlParameter.
                    sqlParameter.Value = idUsuario;                 // Asigno valor a sqlParameter.
                    sqlCommand.Parameters.Add(sqlParameter);        // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD.
                    {
                        if (sqlDataReader.HasRows) // Me aseguro que haya filas para leer.
                        {
                            while (sqlDataReader.Read()) // En cada fila leida.
                            {
                                Producto producto = InicializarProductoDesdeBD(sqlDataReader); // Creo objeto producto para alamcenar los atributos leidos en la fila actual de la BD.

                                listaProductos.Add(producto); // Agrego el objeto producto a la lista "listaProductos".
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return listaProductos;
        }



        public static Producto ConsultarStock(Producto producto) 
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @id", sqlConnection)) //Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Producto, que tengan Id = @id.
                {
                    var sqlParameter = new SqlParameter();      // Creo un nuevo objeto SqlParameter, para expecificar "@id" en la query utilizada en el objeto SqlCommand.
                    sqlParameter.ParameterName = "id";          // Asigno nombre a sqlParameter.
                    sqlParameter.SqlDbType = SqlDbType.BigInt;  // Asigno el tipo de dato que tiene sqlParameter.
                    sqlParameter.Value = producto.id;           // Asigno valor a sqlParameter.
                    sqlCommand.Parameters.Add(sqlParameter);    // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD.
                    {
                        if (sqlDataReader.HasRows & sqlDataReader.Read()) // Me aseguro que haya filas para leer y leo una fila.
                        {
                            producto.stock = Convert.ToInt32(sqlDataReader["Stock"]); ;
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return producto;
        }



        public static bool ActualizarStock(Producto producto)
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no actualizar el Producto.
            int rowsAffected = 0;   // Variable que indica la cantidad de filas que fueron afectadas al realizar ExecuteNonQuery().

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] " + // Query que me permite actualizar la columna el Stock de la tabla Producto.
                                        "SET Stock = @stock " +
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, de tipo BigInt, para expecificar "@id".
                parameterId.Value = producto.id;                            // Asigno valor a sqlParameter.

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.stock;

                sqlConnection.Open(); // Abro la conexión con la BD.

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Producto.
                {
                    sqlCommand.Parameters.Add(parameterId);
                    sqlCommand.Parameters.Add(parameterStock);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close(); // Cierro la conexión con la BD.
            }

            if (rowsAffected == 1)
            {
                resultado = true;
            }

            return resultado;
        }



        public static long CrearProducto(Producto producto) // Traer Productos: Método que recibe un producto como parámetro, deberá crearlo, puede ser void, pero validar los datos obligatorios.
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no crear el producto.
            long idProducto = 0;    // Variable que indica el el Id del producto creado. Es lo que va a devolver el método.
            
            //Producto producto = (Producto)objeto; // Convierto objeto de clase Objeto a Clase Producto

            // FALTA LA VALIDACIÓN DE LOS CAMPOS. COMO DEBERÍA HACERLA? COMO CREO UN OBJETO VOID PARA PASARLE?

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Producto] (Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " + // Query que me permite agregar un Producto.
                                        "VALUES (@descripciones, @costo, @precioVenta, @stock, @idUsuario) " +
                                        "SELECT @@IDENTITY";

                //var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, de tipo BigInt, para expecificar "@id".
                //parameterId.Value = producto.id;                            // Asigno valor a sqlParameter.

                var parameterDescripciones = new SqlParameter("descripciones", SqlDbType.VarChar);
                parameterDescripciones.Value = producto.descripcion;

                var parameterCosto = new SqlParameter("costo", SqlDbType.Money);
                parameterCosto.Value = producto.costo;

                var parameterPrecioVenta = new SqlParameter("precioVenta", SqlDbType.Money);
                parameterPrecioVenta.Value = producto.precioVenta;

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.stock;

                var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                parameterIdUsuario.Value = producto.idUsuario;

                sqlConnection.Open(); // Abro la conexión con la BD.

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Producto.
                {
                    //sqlCommand.Parameters.Add(parameterId);
                    sqlCommand.Parameters.Add(parameterDescripciones);
                    sqlCommand.Parameters.Add(parameterCosto);
                    sqlCommand.Parameters.Add(parameterPrecioVenta);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);
                    idProducto = Convert.ToInt64(sqlCommand.ExecuteScalar());
                }

                sqlConnection.Close(); // Cierro la conexión con la BD.
            }
            return idProducto;
        }



        public static bool ModificarProducto(Producto producto) // Modificar Producto: Método que recibe un producto con su número de Id, debe modificarlo con la nueva información.
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no crear el Producto.
            int rowsAffected = 0; // Variable que indica la cantidad de filas que fueron afectadas al realizar ExecuteNonQuery().

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {                
                string queryUpdate =    "UPDATE [SistemaGestion].[dbo].[Producto] " + // Query que me permite actualizar todas las columnas de la tabla Producto.
                                        "SET Descripciones = @descripciones, " +
                                        "Costo = @costo, " +
                                        "PrecioVenta = @precioVenta, " +
                                        "stock = @Stock, " +
                                        "IdUsuario = @idUsuario " +
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, de tipo BigInt, para expecificar "@id".
                parameterId.Value = producto.id;                            // Asigno valor a sqlParameter.

                var parameterDescripciones = new SqlParameter("descripciones", SqlDbType.VarChar);
                parameterDescripciones.Value = producto.descripcion;

                var parameterCosto = new SqlParameter("costo", SqlDbType.Money);
                parameterCosto.Value = producto.costo;

                var parameterPrecioVenta = new SqlParameter("precioVenta", SqlDbType.Money);
                parameterPrecioVenta.Value = producto.precioVenta;

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.stock;

                var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                parameterIdUsuario.Value = producto.idUsuario;

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Producto.
                {
                    sqlCommand.Parameters.Add(parameterId);
                    sqlCommand.Parameters.Add(parameterDescripciones);
                    sqlCommand.Parameters.Add(parameterCosto);
                    sqlCommand.Parameters.Add(parameterPrecioVenta);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close(); // Cierro la conexión con la BD
            }

            if (rowsAffected == 1)
            {
                resultado = true;
            }

            return resultado;
        }



        public static bool EliminarProducto(long id) // Eliminar Producto: Método que recibe un id de producto a eliminar y debe eliminarlo de la base de datos
                                                        // (eliminar antes sus productos vendidos también, sino no lo podrá hacer).
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no crear el producto.
            int rowsAffected = 0; // Variable que indica la cantidad de filas que fueron afectadas al realizar ExecuteNonQuery()

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD
            {
                string queryDelete =    "DELETE FROM [SistemaGestion].[dbo].[Producto] " + // Query que me permite eliminar una fila de la tabla Producto.
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, de tipo BigInt, para expecificar "@id".
                parameterId.Value = id;                            // Asigno valor a sqlParameter

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection)) // Creo un objeto SqlCommand con una queryDelete
                {
                    sqlCommand.Parameters.Add(parameterId);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close(); // Cierro la conexión con la BD
            }

            if (rowsAffected == 1)
            {
                resultado = true;
            }

            return resultado;
        }



        // Permite ver por consola los atributos de los objetos Productos contenidos en la lista que se pasa como argumento
        public static void MostrarProductos(List<Producto> productosEnBd)
        {
            Console.WriteLine("\nProducto en BD: ");
            foreach (Producto item in productosEnBd)
            {
                Console.WriteLine("id: " + item.id.ToString() +
                                    "\tDescripcion: " + item.descripcion +
                                    "\tCosto: " + item.costo.ToString() +
                                    "\tPrecioVenta: " + item.precioVenta.ToString() +
                                    "\tStock: " + item.stock.ToString() +
                                    "\tIdUsuario: " + item.idUsuario.ToString());
            }
            Console.WriteLine(" "); // Solo para separar de la próxima linea que se imprima
        }
    }
}
