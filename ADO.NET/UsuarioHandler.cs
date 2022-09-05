
// Debo agregar el paquete SqlClient al proyecto: Click derecho en el proyecto > Administrar paquetes Nuget > Sarch : SqlClient > System.Data.SqlClient > Install

using System.Data.SqlClient; // Para tener acceso a los objetos: SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter.
using System.Data;
using System;

namespace Emiliano_Chiapponi
{
    
    public static class UsuarioHandler// Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "Usuario".
    {
        
        public const string connectionString = "Server=DESKTOP-2QV2INM;Database=SistemaGestion;Trusted_Connection=True";


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que debe recibir un NombreUsuario, debe buscarlo en la base de datos y devolver todos sus atributos.
        public static Usuario TraerUsuario_conNombreUsuario(string nombreUsuario) 
        {
            Usuario usuario = new Usuario();

            // Verifico si el argumento pasado es válido
            if (String.IsNullOrEmpty(nombreUsuario))
            {
                return usuario; // Devuelvo un objeto Usuario inicializado por defecto
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario] WHERE NombreUsuario = @nombreUsuario", sqlConnection))
                {
                    var sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "nombreUsuario";
                    sqlParameter.SqlDbType = SqlDbType.VarChar;
                    sqlParameter.Value = nombreUsuario;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows & dataReader.Read())
                        {
                            usuario.Id = Convert.ToInt64(dataReader["Id"]);
                            usuario.Nombre = dataReader["Nombre"].ToString();
                            usuario.Apellido = dataReader["Apellido"].ToString();
                            usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                            usuario.Contraseña = dataReader["Contraseña"].ToString();
                            usuario.Mail = dataReader["Mail"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return usuario;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe como parámetro un NombreUsuario y una Contraseña. Debe buscar en la base de datos si el Usuario existe y si posee la misma Contraseña lo devuelve, caso contrario devuelve error.
        public static Usuario InicioDeSesion(string nombreUsuario, string contraseña)
        {
            Usuario usuario = new Usuario();

            // Verifico si los argumentos son validos
            if (String.IsNullOrEmpty(nombreUsuario) || String.IsNullOrEmpty(contraseña)) // Si nombreUsuario o contraseña están vacíos o son null no ejecuto el método y devuelvo usuario vacío.
            {
                return usuario; // Devuelvo un Usuario inicializado por defecto.
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario] WHERE (NombreUsuario = @nombreUsuario AND Contraseña = @contraseña)", sqlConnection))
                {
                    var sqlParameter1 = new SqlParameter();
                    sqlParameter1.ParameterName = "nombreUsuario";
                    sqlParameter1.SqlDbType = SqlDbType.VarChar;
                    sqlParameter1.Value = nombreUsuario;
                    sqlCommand.Parameters.Add(sqlParameter1);

                    var sqlParameter2 = new SqlParameter();
                    sqlParameter2.ParameterName = "contraseña";
                    sqlParameter2.SqlDbType = SqlDbType.VarChar;
                    sqlParameter2.Value = contraseña;
                    sqlCommand.Parameters.Add(sqlParameter2);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows & dataReader.Read())
                        {
                            usuario.Id = Convert.ToInt64(dataReader["Id"]);
                            usuario.Nombre = dataReader["Nombre"].ToString();
                            usuario.Apellido = dataReader["Apellido"].ToString();
                            usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                            usuario.Contraseña = dataReader["Contraseña"].ToString();
                            usuario.Mail = dataReader["Mail"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return usuario;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe todos los datos de un Usuario y deberá actualizar el mismo (buscando por su Id) con los datos racibidos. No debe crear un nuevo usuario.
        public static bool ModificarUsuario(Usuario usuario)
        {
            bool resultado = false;
            int rowsAffected = 0;

            // Verifico si los atributos del objeto pasado como argumento son validos
            if (usuario.Id <= 0) // El Id debe ser mayor a 0 para que apunte a un Usuario valido.
            {
                return false;
            }

            if (String.IsNullOrEmpty(usuario.Nombre) ||
                String.IsNullOrEmpty(usuario.Apellido) ||
                String.IsNullOrEmpty(usuario.NombreUsuario) ||
                String.IsNullOrEmpty(usuario.Contraseña) ||
                String.IsNullOrEmpty(usuario.Mail))
            {
                return false; // Si algún atributo está vacío o es null devuelvo false
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryUpdate =    "UPDATE [SistemaGestion].[dbo].[Usuario] " + // Query que me permite actualizar todas las columnas de la tabla usuario.
                                        "SET Nombre = @nombre, " +
                                            "Apellido = @apellido, " +
                                            "NombreUsuario = @nombreUsuario, " +
                                            "Contraseña = @contraseña, " +
                                            "Mail = @mail " +
                                            "WHERE Id = @id";

                var parameterNombre = new SqlParameter("nombre", SqlDbType.VarChar);
                parameterNombre.Value = usuario.Nombre;

                var parameterApellido = new SqlParameter("apellido", SqlDbType.VarChar);
                parameterApellido.Value = usuario.Apellido;

                var parameterNombreUsuario = new SqlParameter("nombreUsuario", SqlDbType.VarChar);
                parameterNombreUsuario.Value = usuario.NombreUsuario;

                var parameterContraseña = new SqlParameter("contraseña", SqlDbType.VarChar);
                parameterContraseña.Value = usuario.Contraseña;

                var parameterMail = new SqlParameter("mail", SqlDbType.VarChar);
                parameterMail.Value = usuario.Mail;

                var parameterId = new SqlParameter("id", SqlDbType.BigInt);
                parameterId.Value = usuario.Id;

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterNombre);
                    sqlCommand.Parameters.Add(parameterApellido);
                    sqlCommand.Parameters.Add(parameterNombreUsuario);
                    sqlCommand.Parameters.Add(parameterContraseña);
                    sqlCommand.Parameters.Add(parameterMail);
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


        // Método que trae todos los Usuarios de la BD.
        public static List<Usuario> TraerUsuarios() 
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario]", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Usuario usuario = new Usuario();

                                usuario.Id = Convert.ToInt64(dataReader["Id"]);
                                usuario.Nombre = dataReader["Nombre"].ToString();
                                usuario.Apellido = dataReader["Apellido"].ToString();
                                usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                usuario.Contraseña = dataReader["Contraseña"].ToString();
                                usuario.Mail = dataReader["Mail"].ToString();

                                usuarios.Add(usuario);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return usuarios;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe todos los datos de un Usuario y debe crear un nuevo Usuario en BD con los datos recibidos.
        public static bool CrearUsuario(Usuario usuario)
        {
            bool resultado = false;
            int rowsAffected = 0;

            // Verifico si los atributos del objeto pasado como argumento son validos
            if (String.IsNullOrEmpty(usuario.Nombre) ||
                String.IsNullOrEmpty(usuario.Apellido) ||
                String.IsNullOrEmpty(usuario.NombreUsuario) ||
                String.IsNullOrEmpty(usuario.Contraseña) ||
                String.IsNullOrEmpty(usuario.Mail))
            {
                return false; // Si algún atributo está vacío o es null devuelvo false
            }

            // Verifico si el nombreUsuario ya existe
            Usuario usuarioEnBD = new Usuario();
            usuarioEnBD = TraerUsuario_conNombreUsuario(usuario.NombreUsuario); // Devuelve un objeto Usuario cuyo NombreUsuario coincida con el parámetro pasado. En caso de no encontrarlo devuelve un objeto Usuario inicializado por defecto.
            if (usuarioEnBD.Id != 0) // Si el Id está en cero es porque el método devolvío un Usuario inicializado por defecto, es decir, no hay Usuario con mismo nombreUsuario en BD.
            {
                return resultado; // Salgo del método y devuelvo False.
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Usuario] (Nombre, Apellido, NombreUsuario, Contraseña, Mail) " + // Query que me permite crear un Usuario.
                                        "VALUES (@nombre, @apellido, @nombreUsuario, @contraseña, @mail) ";

                var parameterNombre = new SqlParameter("nombre", SqlDbType.VarChar);
                parameterNombre.Value = usuario.Nombre;

                var parameterApellido = new SqlParameter("apellido", SqlDbType.VarChar);
                parameterApellido.Value = usuario.Apellido;

                var parameterNombreUsuario = new SqlParameter("nombreUsuario", SqlDbType.VarChar);
                parameterNombreUsuario.Value = usuario.NombreUsuario;

                var parameterContraseña = new SqlParameter("contraseña", SqlDbType.VarChar);
                parameterContraseña.Value = usuario.Contraseña;

                var parameterMail = new SqlParameter("mail", SqlDbType.VarChar);
                parameterMail.Value = usuario.Mail;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterNombre);
                    sqlCommand.Parameters.Add(parameterApellido);
                    sqlCommand.Parameters.Add(parameterNombreUsuario);
                    sqlCommand.Parameters.Add(parameterContraseña);
                    sqlCommand.Parameters.Add(parameterMail);
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


        // Método que recibe un Id de Usuario y debe eliminarlo de la base de datos. Se deben eliminar antes sus productos vendidos también.
        public static bool EliminarUsuario(long id)
        {
            bool resultado = false;
            int rowsAffected = 0;

            // Verifico si los atributos del objeto pasado como argumento son validos
            if (id <= 0) // El Id debe ser mayor a 0 para que apunte a un Usuario valido.
            {
                return false;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Usuario] " + // Query que me permite eliminar una fila de la tabla Usuario.
                                        "WHERE Id = @id";

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


        // Método que recibe un Id de Usuario y debe buscarlo en la base de datos para devolver todos sus atributos.
        public static Usuario TraerUsuario_conId(long id)
        {
            Usuario usuario = new Usuario();

            // Verifico si el argumento pasado es válido
            if (id <= 0)
            {
                return usuario; // El id no puede ser cero o negativo. Devuelvo un objeto Usuario inicializado por defecto
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario] WHERE Id = @id", sqlConnection))
                {
                    var sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "id";
                    sqlParameter.SqlDbType = SqlDbType.VarChar;
                    sqlParameter.Value = id;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows & dataReader.Read())
                        {
                            usuario.Id = Convert.ToInt64(dataReader["Id"]);
                            usuario.Nombre = dataReader["Nombre"].ToString();
                            usuario.Apellido = dataReader["Apellido"].ToString();
                            usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                            usuario.Contraseña = dataReader["Contraseña"].ToString();
                            usuario.Mail = dataReader["Mail"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return usuario;
        }
    }
}