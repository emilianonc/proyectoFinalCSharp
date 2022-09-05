
// Debo agregar el paquete SqlClient al proyecto: Click derecho en el proyecto > Administrar paquetes Nuget > Sarch : SqlClient > System.Data.SqlClient > Install

using System.Data.SqlClient; // Para tener acceso a los objetos: SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter
using System.Data;

namespace Emiliano_Chiapponi
{
    
    public static class ProductoVendidoHandler// Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "ProductoVendido".
    {

        public const string connectionString = "Server=DESKTOP-2QV2INM;Database=SistemaGestion;Trusted_Connection=True";


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que trae todos los ProductoVendido de un determinado Usuario.
        public static List<ProductoVendido> TraerProductosVendidos_conIdUsuario(long idUsuario)
        {
            List<ProductoVendido> productosVendidosPorUsuario = new List<ProductoVendido>();

            // Valido el argumento recibído
            Usuario usuarioEnBD = UsuarioHandler.TraerUsuario_conId(idUsuario); // Verifica que el argumento recibido sea >= 0 y busca un usuario que tenga como Id el argumento, si no lo encuentra devuelve un objeto de clase Usuario inicializado por defecto
            if (usuarioEnBD.Id == 0) // Si el objeto Usuario tiene Id == 0 es porque está inicializado por defecto, es decir, no se encontró en BD. Lo que implica que el argumento recibido no apunta a un usuario válido.
            {
                return productosVendidosPorUsuario;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string query = "SELECT pv.Id, pv.Stock, pv.IdProducto, pv.IdVenta " +     // Query que devuelve las columnas: 
                                        "FROM[SistemaGestion].[dbo].[ProductoVendido] AS pv " + //      Id, Descripciones, Costo, PrecioVenta, Stock y IdUsuario de la tabla Producto.           
                                        "INNER JOIN Producto p ON p.Id = pv.IdProducto " +      // De la intersección de Producto.Id y ProductoVendido.IdProducto 
                                        "WHERE p.IdUsuario = @idUsuario";                       // Para los elementos de Producto que tienen IdUsuario = idUsuario

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    var sqlParameter = new SqlParameter();     
                    sqlParameter.ParameterName = "idUsuario";  
                    sqlParameter.SqlDbType = SqlDbType.BigInt; 
                    sqlParameter.Value = idUsuario;            
                    sqlCommand.Parameters.Add(sqlParameter);   

                    sqlConnection.Open(); 

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows) 
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt64(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt64(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt64(dataReader["Idventa"]);

                                productosVendidosPorUsuario.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close(); 
                }
            }
            return productosVendidosPorUsuario;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe un IdVenta y debe devolver todos los ProductoVendido que tengan el mismo IdVenta.
        public static List<ProductoVendido> TraerProductosVendidos_conIdVenta(long idVenta) 
        {
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string query =    "SELECT * FROM[SistemaGestion].[dbo].[ProductoVendido] " +     // Query que devuelve todas las columnas de la tabla ProductoVendido de las filas de  que tienen IdVenta = idVenta.
                                        "WHERE IdVenta = @idVenta";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    var sqlParameter = new SqlParameter();      
                    sqlParameter.ParameterName = "idVenta";     
                    sqlParameter.SqlDbType = SqlDbType.BigInt;  
                    sqlParameter.Value = idVenta;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt64(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt64(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt64(dataReader["Idventa"]);

                                productosVendidos.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosVendidos;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe una lista de objetos de clase ProductoVendido y debe cargar los mismos en la tabla ProductoVendido en BD. Se utiliza para poder ejecutar el segundo paso de CargarVenta().
        public static bool CargarProductosVendidos(List<ProductoVendido> productosVendidos)
        {
            bool resultado = false;
            long idProductoVendido = 0;
            int elementosEnLaLista = 0;
            int idValidoEncontrado = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido] (IdProducto, Stock, IdVenta) " + // Query que me permite agregar un ProductoVendido.
                                        "VALUES (@idProducto, @stock, @idventa) " +
                                        "SELECT @@IDENTITY";

                var parameterIdProducto = new SqlParameter("idProducto", SqlDbType.BigInt) { Value = 0 };
                var parameterStock = new SqlParameter("stock", SqlDbType.Int) { Value = 0 };
                var parameterIdUsuario = new SqlParameter("idVenta", SqlDbType.BigInt) { Value = 0 };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterIdProducto);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);

                    foreach (ProductoVendido item in productosVendidos)
                    {
                        // Por cada elemento en la lista productosVendidos se actualizan los parámetros de la query y se ejecuta la misma
                        parameterIdProducto.Value = item.IdProducto;
                        parameterStock.Value = item.Stock;
                        parameterIdUsuario.Value = item.IdVenta;
                        elementosEnLaLista++;
                        idProductoVendido = Convert.ToInt64(sqlCommand.ExecuteScalar());
                        if(idProductoVendido > 0) // Si el Id del objeto ProductoVendido insertado en la tabla es > 0 quiere decir que se inserto correctamente
                        {
                            idValidoEncontrado++;
                        }
                    }
                }
                sqlConnection.Close();
            }
            if (idValidoEncontrado == elementosEnLaLista) // Se valida que se hayan insertado tantas filas en BD como elementos había en la lista recibida por el método
            {
                resultado = true;
            }
            return resultado;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe un IdProducto y debe eliminar todos los ProductoVendido con dicho Id de la tabla ProductoVendido en la BD.
        public static bool EliminarProductoVendido(long idProducto)
        {
            bool resultado = false;
            int rowsAffected = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryUpdate = "DELETE FROM [SistemaGestion].[dbo].[ProductoVendido] " + // Query que me permite eliminar todas las filas de la tabla ProductoVendido que cumplan con IdProducto = idProducto
                                        "WHERE IdProducto = @idProducto";

                var parameterIdProducto = new SqlParameter("idProducto", SqlDbType.BigInt);
                parameterIdProducto.Value = idProducto;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterIdProducto);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            if (rowsAffected >= 1)
            {
                resultado = true;
            }
            return resultado;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe un IdVenta y debe eliminar todos los ProductoVendido que tengan ese mismo IdVenta.
        public static bool EliminarProductoVendido_conIdVenta(long idVenta) 
        {
            bool resultado = false;
            int rowsAffected = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[ProductoVendido] " + // Query que me permite eliminar todas las filas de la tabla ProductoVendido que cumplan con IdVenta = idVenta.
                                        "WHERE IdVenta = @idventa";

                var parameterIdVenta = new SqlParameter("idVenta", SqlDbType.BigInt);
                parameterIdVenta.Value = idVenta;                                     

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterIdVenta);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            if (rowsAffected >= 1)
            {
                resultado = true;
            }
            return resultado;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        /*public List<ProductoVendido> TraerProductosVendidos_conIdUsuario() // Método que trae todos los productos vendidos de la BD
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


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que permite ver por consola los atributos de los objetos ProductoVendido contenidos en la lista que se pasa como argumento.
        public static void MostrarProductosVendidos(List<ProductoVendido> productosVendidos)
        {
            Console.WriteLine("ProductoVendido en BD:");
            foreach (ProductoVendido item in productosVendidos)
            {
                Console.WriteLine("id: " + item.Id.ToString() +
                                    "\tidProducto: " + item.IdProducto.ToString() +
                                    "\tstock: " + item.Stock.ToString() +
                                    "\tidVenta: " + item.IdVenta.ToString());
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

        public List<Producto> TraerProductosVendidos_conIdUsuario(int idUsuario) // Creo método que trae todos los productos vendidos por un usuario. 
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