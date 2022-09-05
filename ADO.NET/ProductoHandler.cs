
// Debo agregar el paquete SqlClient al proyecto: Click derecho en el proyecto > Administrar paquetes Nuget > Sarch : SqlClient > System.Data.SqlClient > Install

using System.Data.SqlClient; // Me permite tener acceso a los objetos: SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter
using System.Data; // Me permite tener acceso a los tipos de variables de la base de datos, entre otros.

namespace Emiliano_Chiapponi
{

    public static class ProductoHandler // Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "Producto"
    {

        // String que me permite conectarme a mi Base de Datos (BD)
        public const string connectionString = "Server=DESKTOP-2QV2INM;Database=SistemaGestion;Trusted_Connection=True"; 


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que crea e inicializa un objeto de clase Producto con los valores provistos por un objeto SqlDataReader que previamente accedío a la BD.
        private static Producto InicializarProductoDesdeBD(SqlDataReader dataReader) 
        {
            Producto nuevoProducto = new Producto(
                                            Convert.ToInt64(dataReader["Id"]),
                                            dataReader["Descripciones"].ToString(),
                                            Convert.ToDouble(dataReader["Costo"]),
                                            Convert.ToDouble(dataReader["PrecioVenta"]),
                                            Convert.ToInt32(dataReader["Stock"]),
                                            Convert.ToInt64(dataReader["IdUsuario"])); // Utilizo el constructor de Producto con los atributos provistos por el objeto SqlDataReader para crear e inicializar un nuevo Producto.
            return nuevoProducto;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que debe traer todos los productos cargados en la base.
        public static List<Producto> TraerProductos() 
        {
            List<Producto> productosEnBD = new List<Producto>(); // Creo una lista de objetos de clase Producto. Va a ser lo que devuelva el método.
            
            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                const string querySelect = "SELECT * FROM [SistemaGestion].[dbo].[Producto]"; // Query que selecciona todas las columnas de la tabla Producto.

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "querySelect" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader. Permite leer un flujo de filas de solo avance desde una base de datos de SQL Server.
                    {
                        if (sqlDataReader.HasRows) // Si SqlDataReader contiene una o varias filas devuelve TRUE, caso contrario devuelve FALSE // Me aseguro que haya filas para leer.
                        {
                            while (sqlDataReader.Read()) // Desplaza SqlDataReader al siguiente registro. Se debe llamar antes de acceder a los datos. Mientras haya datos devuelve TRUE.
                            {
                                Producto producto = InicializarProductoDesdeBD(sqlDataReader); // Creo objeto producto que es inicializado por el método "InicializarProductoDesdeBD" con los atributos obtenidos por "sqlDataReader".

                                productosEnBD.Add(producto); // Agrego el objeto producto a la lista.
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return productosEnBD; // Devuelvo una lista de objetos de clase Producto, que contiene todos los productos que se encuentran en la BD.
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que trae todos los productos cargados en la BD cuyo IdUsuario = idUsuario.
        public static List<Producto> TraerProductos_conIdUsuario(long idUsuario) 
        {
            List<Producto> productosConIdUsuario = new List<Producto>(); // Creo una lista de objetos de clase Producto. Va a ser lo que devuelva el método.

            // Valido que el argumento pasado al método sea válido
            if (idUsuario <= 0)
            {
                return productosConIdUsuario;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                const string querySelect = "SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE IdUsuario = @idUsuario"; // Query que selecciona todas las columnas de la tabla Producto de las filas con IdUsuario = idUsuario.

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "querySelect" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    var sqlParameter = new SqlParameter();          // Creo un nuevo objeto SqlParameter, para especificar "@idUsuario", declarado en querySelect.
                    sqlParameter.ParameterName = "idUsuario";       // Asigno nombre al sqlParameter. Debe ser el mismo nombre utilizado en la query.
                    sqlParameter.SqlDbType = SqlDbType.BigInt;      // Asigno el tipo de dato que tiene la columna correspondiente al parámetro (IdUsuario).
                    sqlParameter.Value = idUsuario;                 // Asigno el valor a sqlParameter. 
                    sqlCommand.Parameters.Add(sqlParameter);        // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader. Permite leer un flujo de filas de solo avance desde una base de datos de SQL Server.
                    {
                        if (sqlDataReader.HasRows) // Si SqlDataReader contiene una o varias filas devuelve TRUE, caso contrario devuelve FALSE // Me aseguro que haya filas para leer.
                        {
                            while (sqlDataReader.Read()) // Desplaza SqlDataReader al siguiente registro. Se debe llamar antes de acceder a los datos. Mientras haya datos devuelve TRUE.
                            {
                                Producto producto = InicializarProductoDesdeBD(sqlDataReader); // Creo objeto producto que es inicializado por el método "InicializarProductoDesdeBD" con los atributos obtenidos por "sqlDataReader".

                                productosConIdUsuario.Add(producto); // Agrego el objeto producto a la lista.
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return productosConIdUsuario; // Devuelvo una lista de objetos de clase Producto, que contiene todos los productos que se encuentran en la BD cuyo IdUsuario = idUsuario.
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que devuelve el stock de un determinado producto en BD.
        public static Producto ConsultarStock(Producto producto) 
        {
            // Valido que el argumento pasado al método sea válido
            if (producto.Id <= 0)
            {
                return producto;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                const string querySelect = "SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @id"; // Query que selecciona todas las columnas de la tabla Producto de las filas con Id = id.

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "querySelect" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    var sqlParameter = new SqlParameter();      // Creo un nuevo objeto SqlParameter, para especificar "@id", declarado en querySelect.
                    sqlParameter.ParameterName = "id";          // Asigno nombre al sqlParameter. Debe ser el mismo nombre utilizado en la query.
                    sqlParameter.SqlDbType = SqlDbType.BigInt;  // Asigno el tipo de dato que tiene la columna correspondiente al parámetro (Id).
                    sqlParameter.Value = producto.Id;           // Asigno el valor a sqlParameter. 
                    sqlCommand.Parameters.Add(sqlParameter);    // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader. Permite leer un flujo de filas de solo avance desde una base de datos de SQL Server.
                    {
                        if (sqlDataReader.HasRows & sqlDataReader.Read()) // .HasRows: Me aseguro que haya filas para leer // .Read(): desplazo SqlDataReader al siguiente registro. Se debe llamar antes de acceder a los datos.
                        {
                            producto.Stock = Convert.ToInt32(sqlDataReader["Stock"]); // Actualizo el atributo Stock del objeto producto
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return producto; // Devuelvo el objeto de clase Producto con el atributo Stock actualizado.
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que devuelve que actualiza, en BD, el stock de un determinado producto.
        public static bool ActualizarStock(Producto producto)
        {
            // Valido que el argumento pasado al método sea válido
            if (producto.Id <= 0)
            {
                return false;
            }

            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no actualizar el stock del Producto.
            int rowsAffected = 0;   // Variable que va a indicar la cantidad de filas que fueron afectadas al ejecutar la query en la BD. Se utiliza para validación del método.

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] " + // Query que me permite actualizar la columna el Stock de la tabla Producto.
                                        "SET Stock = @stock " +
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, para especificar "@id", declarado en queryUpdate. Defino el nombre del parámetro (que debe coincidir con el nombre utilizado en la query) y defino el tipo de dato de la columna del parámetro.
                parameterId.Value = producto.Id;                            // Asigno valor al sqlParameter.

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.Stock;

                sqlConnection.Open(); // Abro la conexión con la BD.

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "queryUpdate" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    sqlCommand.Parameters.Add(parameterId); // Agrego sqlParameter "parameterId" a la lista de parametros del objeto SqlCommand creado.
                    sqlCommand.Parameters.Add(parameterStock); 
                    rowsAffected = sqlCommand.ExecuteNonQuery(); // Ejecuta una instrucción de Transact-SQL en la conexión y devuelve el número de filas afectadas, que son almacenadas en "rowsAffected"
                }
                sqlConnection.Close(); // Cierro la conexión con la BD.
            }
            if (rowsAffected == 1) // Si se afectó una fila de la BD quiere decir que el método tuvo exito.
            {
                resultado = true; // Devuelvo TRUE.
            }
            return resultado;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe un producto como parámetro y deberá crearlo en la BD.
        public static bool CrearProducto(Producto producto)
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no crear el producto.
            long idProducto = 0;    // Variable que indica el el Id del producto creado. Se utiliza para validación.

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Producto] (Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " + // Query que me permite agregar un producto a la BD.
                                        "VALUES (@descripciones, @costo, @precioVenta, @stock, @idUsuario) " +
                                        "SELECT @@IDENTITY";

                // Creo y defino todos los objetos SqlParameter necesarios para definir queryInsert.
                var parameterDescripciones = new SqlParameter("descripciones", SqlDbType.VarChar);  // Creo un nuevo objeto SqlParameter, para especificar "@descripciones", declarado en queryInsert. Defino el nombre del parámetro (que debe coincidir con el nombre utilizado en la query) y defino el tipo de dato de la columna del parámetro.
                parameterDescripciones.Value = producto.Descripcion;                                // Asigno valor al sqlParameter.

                var parameterCosto = new SqlParameter("costo", SqlDbType.Money);
                parameterCosto.Value = producto.Costo;

                var parameterPrecioVenta = new SqlParameter("precioVenta", SqlDbType.Money);
                parameterPrecioVenta.Value = producto.PrecioVenta;

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.Stock;

                var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                parameterIdUsuario.Value = producto.IdUsuario;

                sqlConnection.Open(); // Abro la conexión con la BD.

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "queryInsert" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    // Agrego todos los objetos SqlParameter al objeto SqlCommand que va a ejecutar queryInsert.
                    sqlCommand.Parameters.Add(parameterDescripciones); // Agrego sqlParameter "parameterId" a la lista de parametros del objeto SqlCommand creado.
                    sqlCommand.Parameters.Add(parameterCosto);
                    sqlCommand.Parameters.Add(parameterPrecioVenta);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);
                    idProducto = Convert.ToInt64(sqlCommand.ExecuteScalar()); // Ejecuta la consulta y devuelve la primera columna de la primera fila del conjunto de resultados devueltos por la consulta. 
                }
                sqlConnection.Close(); // Cierro la conexión con la BD.
            }
            if (idProducto != 0) // Verifico que el Id devuelvo por "ExecuteScalar()" sea distinto de cero. Esto indica que el producto se creo con exito.
            {
                resultado = true; // Devuelto TRUE.
            }
            return resultado;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe un producto con su número de Id y debe modificarlo en la BD con la nueva información.
        public static bool ModificarProducto(Producto producto)
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no modificar el Producto.
            int rowsAffected = 0;   // Variable que va a indicar la cantidad de filas que fueron afectadas al ejecutar la query en la BD. Se utiliza para validación del método.

            // Valido que el argumento pasado al método sea válido
            if (producto.Id <= 0) // El Id debe ser mayor a 0 para que apunte a un producto valido
            {
                return false; 
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {                
                string queryUpdate =    "UPDATE [SistemaGestion].[dbo].[Producto] " + // Query que me permite actualizar todas las columnas de la tabla Producto.
                                        "SET Descripciones = @descripciones, " +
                                        "Costo = @costo, " +
                                        "PrecioVenta = @precioVenta, " +
                                        "stock = @Stock, " +
                                        "IdUsuario = @idUsuario " +
                                        "WHERE Id = @id";

                // Creo y defino todos los objetos SqlParameter necesarios para definir queryUpdate.
                var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, para especificar "@id", declarado en queryUpdate. Defino el nombre del parámetro (que debe coincidir con el nombre utilizado en la query) y defino el tipo de dato de la columna del parámetro.
                parameterId.Value = producto.Id;                            // Asigno valor a sqlParameter.

                var parameterDescripciones = new SqlParameter("descripciones", SqlDbType.VarChar);
                parameterDescripciones.Value = producto.Descripcion;

                var parameterCosto = new SqlParameter("costo", SqlDbType.Money);
                parameterCosto.Value = producto.Costo;

                var parameterPrecioVenta = new SqlParameter("precioVenta", SqlDbType.Money);
                parameterPrecioVenta.Value = producto.PrecioVenta;

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.Stock;

                var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                parameterIdUsuario.Value = producto.IdUsuario;

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))  // Creo un objeto SqlCommand que va a ejecutar "queryUpdate" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    // Agrego todos los objetos SqlParameter al objeto SqlCommand que va a ejecutar queryUpdate.
                    sqlCommand.Parameters.Add(parameterId); // Agrego sqlParameter "parameterId" a la lista de parametros del objeto SqlCommand creado.
                    sqlCommand.Parameters.Add(parameterDescripciones);
                    sqlCommand.Parameters.Add(parameterCosto);
                    sqlCommand.Parameters.Add(parameterPrecioVenta);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);
                    rowsAffected = sqlCommand.ExecuteNonQuery(); // Ejecuta una instrucción de Transact-SQL en la conexión y devuelve el número de filas afectadas, que son almacenadas en "rowsAffected"
                }
                sqlConnection.Close(); // Cierro la conexión con la BD
            }
            if (rowsAffected == 1) // Verifico que se haya afectado una fila, lo cual indica que el método tuvo exito actualizando el producto
            {
                resultado = true; // Devuelvo TRUE
            }
            return resultado;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe un Id de Producto a eliminar y debe eliminarlo de la base de datos. Primero debe eliminar de ProdcutoVendido todos los productos con mismo Id, sino no se podrá actualizar la BD (por restricción de la BD).
        public static bool EliminarProducto(long id) 
        {
            // Valido que el argumento pasado al método sea válido
            if (id <= 0) // El Id debe ser mayor a 0 para que apunte a un producto valido.
            {
                return false; 
            }

            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no ejecutar correctamente el método.
            int rowsAffected = 0;   // Variable que va a indicar la cantidad de filas que fueron afectadas al ejecutar la query en la BD. Se utiliza para validación del método.

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                string queryDelete =    "DELETE FROM [SistemaGestion].[dbo].[Producto] " + // Query que me permite eliminar la fila de la tabla Producto cuyo Id sea igual a id.
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, para especificar "@id", declarado en queryDelete. Defino el nombre del parámetro (que debe coincidir con el nombre utilizado en la query) y defino el tipo de dato de la columna del parámetro.
                parameterId.Value = id;                                     // Asigno valor a sqlParameter.

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "queryDelete" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    sqlCommand.Parameters.Add(parameterId); // Agrego sqlParameter "parameterId" a la lista de parametros del objeto SqlCommand creado.
                    rowsAffected = sqlCommand.ExecuteNonQuery(); // Ejecuta una instrucción de Transact-SQL en la conexión y devuelve el número de filas afectadas, que son almacenadas en "rowsAffected"
                }
                sqlConnection.Close(); // Cierro la conexión con la BD
            }
            if (rowsAffected == 1) // Verifico que se haya afectado una fila, lo cual indica que el método tuvo exito eliminando el producto
            {
                resultado = true; // Devuelvo TRUE
            }
            return resultado;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que debe traer el producto cargado en la base cuyo Id = id
        public static Producto TraerProducto_conId(long id)
        {
            Producto producto = new Producto(); // Creo un objeto de clase Producto. Va a ser lo que devuelva el método.

            // Valido que el argumento pasado al método sea válido
            if (id <= 0) // El Id debe ser mayor a 0 para que apunte a un producto valido.
            {
                return producto;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                const string queryGet = "SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @id"; // Query que me permite seleccionar todas las columnas de la tabla Prodcuto de la fila cuyo Id = id

                using (SqlCommand sqlCommand = new SqlCommand(queryGet, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "queryGet" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    var sqlParameter = new SqlParameter();          // Creo un nuevo objeto SqlParameter, para especificar "@id", declarado en querySelect.
                    sqlParameter.ParameterName = "id";              // Asigno nombre al sqlParameter. Debe ser el mismo nombre utilizado en la query.
                    sqlParameter.SqlDbType = SqlDbType.BigInt;      // Asigno el tipo de dato que tiene la columna correspondiente al parámetro (Id).
                    sqlParameter.Value = id;                        // Asigno el valor a sqlParameter. 
                    sqlCommand.Parameters.Add(sqlParameter);        // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader. Permite leer un flujo de filas de solo avance desde una base de datos de SQL Server.
                    {
                        if (sqlDataReader.HasRows & sqlDataReader.Read()) // .HasRows: Me aseguro que haya filas para leer // .Read(): desplazo SqlDataReader al siguiente registro. Se debe llamar antes de acceder a los datos.
                        {
                            producto = InicializarProductoDesdeBD(sqlDataReader); // Creo objeto producto que es inicializado por el método "InicializarProductoDesdeBD" con los atributos obtenidos por "sqlDataReader".
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return producto; // Devuelvo el objeto producto cuya Id = id encontrado en la BD.
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Permite ver por consola los atributos de los objetos Productos contenidos en la lista que se pasa como argumento
        public static void MostrarProductos(List<Producto> productosEnBd)
        {
            Console.WriteLine("\nProducto en BD: ");
            foreach (Producto item in productosEnBd)
            {
                Console.WriteLine("id: " + item.Id.ToString() +
                                    "\tDescripcion: " + item.Descripcion +
                                    "\tCosto: " + item.Costo.ToString() +
                                    "\tPrecioVenta: " + item.PrecioVenta.ToString() +
                                    "\tStock: " + item.Stock.ToString() +
                                    "\tIdUsuario: " + item.IdUsuario.ToString());
            }
            Console.WriteLine(" "); // Solo para separar de la próxima linea que se imprima
        }
    }
}
