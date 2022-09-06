
// Debo agregar el paquete SqlClient al proyecto: Click derecho en el proyecto > Administrar paquetes Nuget > Sarch : SqlClient > System.Data.SqlClient > Install

using System.Data.SqlClient; // Para tener acceso a los objetos: SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter
using System.Data;

namespace Emiliano_Chiapponi
{

    public static class VentaHandler// Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "Venta"
    {

        public const string connectionString = "Server=DESKTOP-2QV2INM;Database=SistemaGestion;Trusted_Connection=True";


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que trae todas las ventas de la BD que contienen productos de un determinado Usuario.
        public static List<Venta> TraerVentas_conIdUsuario(long idUsuario)
        {
            List<Venta> ventas = new List<Venta>();
        
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string query =    "SELECT v.Id, v.Comentarios " + // Query que me devuelve las ventas que contienen productos con IdUsuario = idUsuario.
                                        "FROM Venta AS v " +
                                        "INNER JOIN ProductoVendido AS pv " +
                                        "ON v.Id = pv.IdVenta " +
                                        "INNER JOIN Producto AS p " +
                                        "ON pv.IdProducto = p.Id " +
                                        "WHERE p.IdUsuario = @idUsuario ";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                    parameterIdUsuario.Value = idUsuario;
                    sqlCommand.Parameters.Add(parameterIdUsuario);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();

                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return ventas;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe como parámetro una Venta y debe cargarla en BD.
        public static long CargarVenta(Venta venta)
        {
            bool resultado = false;
            long idVenta = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Venta] (Comentarios) " + // Query que me permite agregar una Venta.
                                        "VALUES (@comentarios) " +
                                        "SELECT @@IDENTITY";

                var parameterComentarios = new SqlParameter("comentarios", SqlDbType.VarChar);
                parameterComentarios.Value = venta.Comentarios;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterComentarios);
                    idVenta = Convert.ToInt64(sqlCommand.ExecuteScalar());
                }
                sqlConnection.Close();
            }
            return idVenta;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que trae todas las ventas de la BD
        /*public static List<Venta> TraerVentas()
        {
            List<Venta> ventas = new List<Venta>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Venta]", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();

                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return ventas;
        }*/


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que elimina de la tabla Venta la fila con Id = id
        public static bool EliminarVenta(long id)
        {
            bool resultado = false;
            int rowsAffected = 0;

            if (id <= 0) // Valido el id recibido
            {
                return false;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Venta] WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt);
                parameterId.Value = id;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterId);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            if (rowsAffected == 1)
            {
                resultado = true;
            }
            return resultado;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que trae todas los ProductoVendido de la BD que estén asociados a una venta.
        public static List<ProductoVendido> TraerVentas()
        {
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string query = "SELECT pv.Id, pv.Stock, pv.IdProducto, pv.IdVenta " + // Query que me devuelve las ventas que contienen productos con IdUsuario = idUsuario.
                                        "FROM Venta AS v " +
                                        "INNER JOIN ProductoVendido AS pv " +
                                        "ON v.Id = pv.IdVenta ";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
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


        // Método que permite ver por consola los atributos de los objetos ProductoVendido contenidos en la lista que se pasa como argumento.
        public static void MostrarVentas(List<Venta> ventas)
        {
            Console.WriteLine("Venta en BD:");
            foreach (Venta item in ventas)
            {
                Console.WriteLine("id: " + item.Id.ToString() +
                                    "\tcomentarios: " + item.Comentarios.ToString());
            }
            Console.WriteLine(" "); // Solo para separar de la próxima linea que se imprima
        }
    }
}