
// Debo agregar el paquete SqlClient al proyecto: Click derecho en el proyecto > Administrar paquetes Nuget > Sarch : SqlClient > System.Data.SqlClient > Install

using System.Data.SqlClient; // Para tener acceso a los objetos: SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter
using System.Data;

namespace Emiliano_Chiapponi
{
    
    public static class ProductoVendidoHandler// Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "ProductoVendido".
    {

        public const string connectionString = "Server=DESKTOP-2QV2INM;Database=SistemaGestion;Trusted_Connection=True";

        public static List<ProductoVendido> TraerProductosVendidos(long idUsuario) // Método que trae todos los ProductoVendido de un determinado Usuario.
        {
            List<ProductoVendido> productosVendidosPorUsuario = new List<ProductoVendido>(); // Creo una lista de objetos de clase ProductoVendido. Va a ser lo que devuelva el método.

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                const string query = "SELECT pv.Id, pv.Stock, pv.IdProducto, pv.IdVenta " +     // Query que devuelve las columnas: 
                                        "FROM[SistemaGestion].[dbo].[ProductoVendido] AS pv " + //    p.Id, p.Descripciones, p.Costo, p.PrecioVenta, p.Stock y p.IdUsuario            
                                        "INNER JOIN Producto p ON p.Id = pv.IdProducto " +      // De la intersección de Producto.Id y ProductoVendido.IdProducto 
                                        "WHERE p.IdUsuario = @idUsuario";                       // Para los elementos de Producto que tienen IdUsuario = idUsuario

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) // Creo un objeto SqlCommand con una query definida previamente.
                {
                    var sqlParameter = new SqlParameter();      // Creo un nuevo objeto SqlParameter, para expecificar "@nombre" en la query utilizada en el objeto SqlCommand.
                    sqlParameter.ParameterName = "idUsuario";   // Asigno nombre a sqlParameter.
                    sqlParameter.SqlDbType = SqlDbType.BigInt;  // Asigno el tipo de dato que tiene sqlParameter.
                    sqlParameter.Value = idUsuario;             // Asigno valor a sqlParameter.
                    sqlCommand.Parameters.Add(sqlParameter);    // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD.
                    {
                        if (dataReader.HasRows) // Me aseguro que haya filas para leer .
                        {
                            while (dataReader.Read()) // Leo una fila.
                            {
                                ProductoVendido productoVendido = new ProductoVendido(); // Creo un nuevo objeto de clase ProductoVendido.

                                // Actualizo todos los atributos de producto con los valores obtenidos de la BD.
                                productoVendido.id = Convert.ToInt64(dataReader["Id"]);
                                productoVendido.stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.idProducto = Convert.ToInt64(dataReader["IdProducto"]);
                                productoVendido.idVenta = Convert.ToInt64(dataReader["Idventa"]);

                                productosVendidosPorUsuario.Add(productoVendido); // Agrego el objeto "productoVendido" a la lista "productosVendidos".
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return productosVendidosPorUsuario;
        }



        public static bool CargarProductosVendidos(List<ProductoVendido> productosVendidos) // Cargar Productos Vendidos: se utiliza para poder ejecutar el segundo paso de CargarVentaProyecto().
        {
            bool resultado = false;     // Creo una variable tipo bool que va a indicar si se pudieron o no cargar los productos vendidos.
            long idProductoVendido = 0; // Variable que va a contener la cantidad de lineas afectadas por el query
            int elementosEnLaLista = 0;
            int idValidoEncontrado = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido] (IdProducto, Stock, IdVenta) " + // Query que me permite agregar un ProductoVendido.
                                        "VALUES (@idProducto, @stock, @idventa) " +
                                        "SELECT @@IDENTITY";

                var parameterIdProducto = new SqlParameter("idProducto", SqlDbType.BigInt) { Value = 0 };
                var parameterStock = new SqlParameter("stock", SqlDbType.Int) { Value = 0 };
                var parameterIdUsuario = new SqlParameter("idVenta", SqlDbType.BigInt) { Value = 0 };

                sqlConnection.Open(); // Abro la conexión con la BD.

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla ProductoVendido.
                {
                    sqlCommand.Parameters.Add(parameterIdProducto);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);

                    foreach (ProductoVendido item in productosVendidos)
                    {
                        parameterIdProducto.Value = item.idProducto;
                        parameterStock.Value = item.stock;
                        parameterIdUsuario.Value = item.idVenta;
                        elementosEnLaLista++;
                        idProductoVendido = Convert.ToInt64(sqlCommand.ExecuteScalar());
                        if(idProductoVendido > 0)
                        {
                            idValidoEncontrado++;
                        }
                    }
                }
                sqlConnection.Close(); // Cierro la conexión con la BD.
            }

            if (idValidoEncontrado == elementosEnLaLista)
            {
                resultado = true;
            }

            return resultado;
        }



        public static bool EliminarProductoVendido(long idProducto) // Eliminar ProductoVendido: Método que recibe un idProducto y debe eliminar todos los ProductoVendido con dicho Id de la base de datos
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no eliminar los ProductoVendido.
            int rowsAffected = 0;   // Variable que indica la cantidad de filas que fueron afectadas al realizar ExecuteNonQuery()

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD
            {
                string queryUpdate = "DELETE FROM [SistemaGestion].[dbo].[ProductoVendido] " + // Query que me permite eliminar todas las filar con IdProducto = idProducto
                                        "WHERE IdProducto = @idProducto";

                var parameterIdProducto = new SqlParameter("idProducto", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, de tipo BigInt, para expecificar "@idProducto".
                parameterIdProducto.Value = idProducto;                                     // Asigno valor a sqlParameter

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Producto.
                {
                    sqlCommand.Parameters.Add(parameterIdProducto);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close(); // Cierro la conexión con la BD
            }

            if (rowsAffected >= 1)
            {
                resultado = true;
            }

            return resultado;
        }



        /*public List<ProductoVendido> TraerProductosVendidos() // Método que trae todos los productos vendidos de la BD
        {
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>(); // Creo una lista de objetos de clase ProductoVendido. Va a ser lo que devuelva el método

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[ProductoVendido]", sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla ProductoVendido
                {
                    sqlConnection.Open(); // Abro la conexión con la BD

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD
                    {
                        if (dataReader.HasRows) // Me aseguro que haya filas para leer 
                        {
                            while (dataReader.Read()) // Leo una fila
                            {
                                ProductoVendido productoVendido = new ProductoVendido(); // Creo un nuevo objeto de clase ProductoVendido
                                
                                // Actualizo todos los atributos de ProductoVendido con los valores obtenidos de la BD
                                productoVendido.id = Convert.ToInt32(dataReader["Id"]);
                                productoVendido.idProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                productoVendido.stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.idVenta = Convert.ToInt32(dataReader["IdVenta"]);

                                productosVendidos.Add(productoVendido); // Agrego el objeto "productoVendido" a la lista "productosVendidos"
                            }   
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD
                }
            }
            return productosVendidos;
        }*/



        // Permite ver por consola los atributos de los objetos ProductoVendido contenidos en la lista que se pasa como argumento.
        public static void MostrarProductosVendidos(List<ProductoVendido> productosVendidos)
        {
            Console.WriteLine("ProductoVendido en BD:");
            foreach (ProductoVendido item in productosVendidos)
            {
                Console.WriteLine("id: " + item.id.ToString() +
                                    "\tidProducto: " + item.idProducto.ToString() +
                                    "\tstock: " + item.stock.ToString() +
                                    "\tidVenta: " + item.idVenta.ToString());
            }
            Console.WriteLine(" "); // Solo para separar de la próxima linea que se imprima
        }
    }
}


















/*
// Traer Productos Vendidos: Traer Todos los productos vendidos de un Usuario,
        // cuya información está en su producto(Utilizar dentro de esta función
        // el "Traer Productos" anteriormente hecho para saber que productosVendidos
        // ir a buscar).

        public List<Producto> TraerProductosVendidos(int idUsuario) // Creo método que trae todos los productos vendidos por un usuario. 
        {
            List<Producto> todosLosProductos = new List<Producto>(); // Creo una lista de objetos de clase Producto que va a contener todos los productos.
            List<Producto> productosVendidosPorUsuario = new List<Producto>(); // Creo una lista de objetos de clase Producto que va a contener los productos vendidos por el Usuario con Id = idUsuario . Va a ser lo que devuelva el método
            List<int> idProductosVendidosPorUsuario = new List<int>(); // Creo una lista de objetos de clase Int32. Para saber el Id de cada producto vendido por el IdUsuario pasado como parámetro a la función
            int idProductoVendido; // Almaceno el IdProduco de la fila que se exta explorando actualmente

            // 1) Busco en la tabla ProductoVendido los IdProducto que tienen IdVenta igual a idUsuario. Almaceno los IdProducto en la lista idProductosVendidos
            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT IdProducto, IdVenta FROM ProductoVendido", sqlConnection)) // Creo un objeto SqlCommand con la query: "SELECT * FROM ProductoVendido" (ya que quiero ver los IdProducto de cada IdVenta) y el objeto sqlConnection
                {
                    sqlConnection.Open(); // Abro la conexión con la BD

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD
                    {
                        if (dataReader.HasRows) // Me aseguro que haya filas para leer
                        {
                            while (dataReader.Read())
                            {
                                if (Convert.ToInt32(dataReader["IdVenta"]) == idUsuario)
                                {
                                    // Si Nombre = nombreUsuario, actualizo todos los atributos de usuario con los valores obtenidos de la BD
                                    //idProductoVendido = Convert.ToInt32(dataReader["IdProducto"]);
                                    //idProductosVendidos.Add(idProductoVendido);
                                    idProductosVendidosPorUsuario.Add(Convert.ToInt32(dataReader["IdProducto"]));
                                }
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD
                }
            }

            // DEBUG Id de cada producto vendido por idUsuario
            //Console.WriteLine("Los IdProducto con IdVenta = " + idUsuario.ToString() + " son:");
            //foreach (var item in idProductosVendidosPorUsuario)
            //{
            //    Console.WriteLine("\t " + item.ToString());
            //}

// 2) Traigo todos los productos
ProductoHandler productoHandler = new ProductoHandler();
todosLosProductos = productoHandler.TraerProductos();

// 3) Agrego a la lista "productosVendidosPorUsuario" todos los Producto que tengan un Id que se encuentre en la lista idProductosVendidosPorUsuario
foreach (var item in todosLosProductos) // Recorro todos los elementos de la lista "todosLosProductos"
{
    if (idProductosVendidosPorUsuario.Contains(item.id)) // Si el Id del Producto actual se encuentra dentro de la lista "idProductosVendidosPorUsuario"
    {
        productosVendidosPorUsuario.Add(item); // Agrego el Producto a la lista "productosVendidosPorUsuario"
    }
}
return productosVendidosPorUsuario;
        }*/