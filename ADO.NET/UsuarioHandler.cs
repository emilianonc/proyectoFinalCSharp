
// Debo agregar el paquete SqlClient al proyecto: Click derecho en el proyecto > Administrar paquetes Nuget > Sarch : SqlClient > System.Data.SqlClient > Install

using System.Data.SqlClient; // Para tener acceso a los objetos: SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter.
using System.Data;
using System;

namespace Emiliano_Chiapponi
{
    
    public static class UsuarioHandler// Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "Usuario".
    {
        
        public const string connectionString = "Server=DESKTOP-2QV2INM;Database=SistemaGestion;Trusted_Connection=True";

        public static Usuario TraerUsuario(string nombreUsuario) // Traer Usuario: Método que debe recibir un NombreUsuario, buscarlo en la base de datos y devolver todos sus datos.
        {
            Usuario usuario = new Usuario(); // Creo una objeto de clase Usuario. Va a ser lo que devuelva el método.

            // Verifico si el argumento pasado es válido
            if (String.IsNullOrEmpty(nombreUsuario))
            {
                return usuario; // Devuelvo un objeto Usuario inicializado por defecto
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario] WHERE NombreUsuario = @nombreUsuario", sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Usuario, cuyo NombreUsuario sea igual a @nombreUsuario.
                {
                    var sqlParameter = new SqlParameter();          // Creo un nuevo objeto SqlParameter, para expecificar "@nombre" en la query utilizada en el objeto SqlCommand.
                    sqlParameter.ParameterName = "nombreUsuario";   // Asigno nombre a sqlParameter.
                    sqlParameter.SqlDbType = SqlDbType.VarChar;     // Asigno el tipo de dato que tiene sqlParameter.
                    sqlParameter.Value = nombreUsuario;             // Asigno valor a sqlParameter.
                    sqlCommand.Parameters.Add(sqlParameter);        // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD.
                    {
                        if (dataReader.HasRows & dataReader.Read()) // Me aseguro que haya filas para leer y leo una fila.
                        {
                            // Actualizo todos los atributos de Usuario con los valores obtenidos de la BD.
                            usuario.id = Convert.ToInt64(dataReader["Id"]);
                            usuario.nombre = dataReader["Nombre"].ToString();
                            usuario.apellido = dataReader["Apellido"].ToString();
                            usuario.nombreUsuario = dataReader["NombreUsuario"].ToString();
                            usuario.contraseña = dataReader["Contraseña"].ToString();
                            usuario.mail = dataReader["Mail"].ToString();
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }

            return usuario;
        }



        public static Usuario InicioDeSesion(string nombreUsuario, string contraseña) // Inicio de Sesion: Método al cual se le pasa como parámetro el nombre del Usuario y la
                                                                               // contraseña, busca en la base de datos si el Usuario existe y si coincide con la
                                                                               // contraseña lo devuelve, caso contrario devuelve error.
        {
            Usuario usuario = new Usuario(); // Creo una objeto de clase Usuario. Va a ser lo que devuelva el método.

            // Verifico si los argumentos son validos
            if (String.IsNullOrEmpty(nombreUsuario) || String.IsNullOrEmpty(contraseña)) // Si nombreUsuario o contraseña estámn vacíos o son null no ejecuto el método y devuelvo usuario vacío.
            {
                return usuario; // Devuelvo un Usuario inicializado por defecto.
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario] WHERE (NombreUsuario = @nombreUsuario AND Contraseña = @contraseña)", sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las columnas de la tabla Usuario, cuyo NombreUsuario sea igual a @nombreUsuario y Contraseña = @contraseña.
                {
                    var sqlParameter1 = new SqlParameter();          // Creo un nuevo objeto SqlParameter, para expecificar "@nombre" en la query utilizada en el objeto SqlCommand.
                    sqlParameter1.ParameterName = "nombreUsuario";   // Asigno nombre a sqlParameter.
                    sqlParameter1.SqlDbType = SqlDbType.VarChar;     // Asigno el tipo de dato que tiene sqlParameter.
                    sqlParameter1.Value = nombreUsuario;             // Asigno valor a sqlParameter.
                    sqlCommand.Parameters.Add(sqlParameter1);        // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    var sqlParameter2 = new SqlParameter();         // Creo un nuevo objeto SqlParameter, para expecificar "@nombre" en la query utilizada en el objeto SqlCommand.
                    sqlParameter2.ParameterName = "contraseña";     // Asigno nombre a sqlParameter.
                    sqlParameter2.SqlDbType = SqlDbType.VarChar;    // Asigno el tipo de dato que tiene sqlParameter.
                    sqlParameter2.Value = contraseña;               // Asigno valor a sqlParameter.
                    sqlCommand.Parameters.Add(sqlParameter2);       // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD.
                    {
                        if (dataReader.HasRows & dataReader.Read()) // Me aseguro que haya filas para leer y leo una fila.
                        {
                            // Actualizo todos los atributos de usuario con los valores obtenidos de la BD.
                            usuario.id = Convert.ToInt64(dataReader["Id"]);
                            usuario.nombre = dataReader["Nombre"].ToString();
                            usuario.apellido = dataReader["Apellido"].ToString();
                            usuario.nombreUsuario = dataReader["NombreUsuario"].ToString();
                            usuario.contraseña = dataReader["Contraseña"].ToString();
                            usuario.mail = dataReader["Mail"].ToString();
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }

            return usuario;
        }



        public static bool ModificarUsuario(Usuario usuario) // Modificar Usuario: Método que recibe todos los datos del Usuario por un JSON y se deberá modificar el mismo con los datos nuevos(No crear uno nuevo).
        {
            bool resultado = false; // Variable de tipo bool que indica si la modificación fue exitosa o no.
            int rowsAffected = 0;   // Variable que indica la cantidad de filas que fueron afectadas al realizar ExecuteNonQuery().

            // Verifico si los atributos del objeto pasado como argumento son validos
            if (String.IsNullOrEmpty(usuario.nombre) ||
                String.IsNullOrEmpty(usuario.apellido) ||
                String.IsNullOrEmpty(usuario.nombreUsuario) ||
                String.IsNullOrEmpty(usuario.contraseña) ||
                String.IsNullOrEmpty(usuario.mail))
            {
                return resultado; // Si algún atributo está vacío o es null devuelvo false
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                string queryUpdate =    "UPDATE [SistemaGestion].[dbo].[Usuario] " + // Query que me permite actualizar todas las columnas de la tabla usuario.
                                        "SET Nombre = @nombre, " +
                                            "Apellido = @apellido, " +
                                            "NombreUsuario = @nombreUsuario, " +
                                            "Contraseña = @contraseña, " +
                                            "Mail = @mail " +
                                            "WHERE Id = @id";

                var parameterNombre = new SqlParameter("nombre", SqlDbType.VarChar);    // Creo un nuevo objeto SqlParameter, de tipo VarChar, para expecificar "@nombre".
                parameterNombre.Value = usuario.nombre;                                 // Asigno valor a sqlParameter.

                var parameterApellido = new SqlParameter("apellido", SqlDbType.VarChar);
                parameterApellido.Value = usuario.apellido;

                var parameterNombreUsuario = new SqlParameter("nombreUsuario", SqlDbType.VarChar);
                parameterNombreUsuario.Value = usuario.nombreUsuario;

                var parameterContraseña = new SqlParameter("contraseña", SqlDbType.VarChar);
                parameterContraseña.Value = usuario.contraseña;

                var parameterMail = new SqlParameter("mail", SqlDbType.VarChar);
                parameterMail.Value = usuario.mail;

                var parameterId = new SqlParameter("id", SqlDbType.BigInt);
                parameterId.Value = usuario.id;

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection)) // Creo un objeto SqlCommand con una query previamente definida.
                {
                    sqlCommand.Parameters.Add(parameterNombre);                             // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.
                    sqlCommand.Parameters.Add(parameterApellido);
                    sqlCommand.Parameters.Add(parameterNombreUsuario);
                    sqlCommand.Parameters.Add(parameterContraseña);
                    sqlCommand.Parameters.Add(parameterMail);
                    sqlCommand.Parameters.Add(parameterId);
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



        public static List<Usuario> TraerUsuarios() // Traer Usuarios: Método que trae todos los Usuarios de la BD.
        {
            List<Usuario> usuarios = new List<Usuario>(); // Creo una lista de objetos de clase Usuario. Va a ser lo que devuelva el método.

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario]", sqlConnection)) // Creo un objeto SqlCommand con una query que selecciona todas las filas de la tabla Usuario.
                {
                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader para ir explorando la BD.
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read()) // Me aseguro que haya filas para leer y leo una fila.
                            {
                                Usuario usuario = new Usuario();

                                // Actualizo todos los atributos de Usuario con los valores obtenidos de la BD.
                                usuario.id = Convert.ToInt64(dataReader["Id"]);
                                usuario.nombre = dataReader["Nombre"].ToString();
                                usuario.apellido = dataReader["Apellido"].ToString();
                                usuario.nombreUsuario = dataReader["NombreUsuario"].ToString();
                                usuario.contraseña = dataReader["Contraseña"].ToString();
                                usuario.mail = dataReader["Mail"].ToString();

                                usuarios.Add(usuario);
                            }
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return usuarios;
        }



        public static bool CrearUsuario(Usuario usuario) // Crear Usuario: Método que recibe todos los datos del Usuario por un JSON y deberá crear un nuevo usuario con los datos recibidos.
        {
            bool resultado = false; // Variable de tipo bool que indica si se pudo crear el usuario.
            int rowsAffected = 0;   // Variable que indica la cantidad de filas que fueron afectadas al realizar ExecuteNonQuery().

            // Verifico si los atributos del objeto pasado como argumento son validos
            if (String.IsNullOrEmpty(usuario.nombre) ||
                String.IsNullOrEmpty(usuario.apellido) ||
                String.IsNullOrEmpty(usuario.nombreUsuario) ||
                String.IsNullOrEmpty(usuario.contraseña) ||
                String.IsNullOrEmpty(usuario.mail))
            {
                return resultado; // Si algún atributo está vacío o es null devuelvo false
            }

            // Verifico si el nombreUsuario ya existe
            Usuario usuarioEnBD = new Usuario();
            usuarioEnBD = TraerUsuario(usuario.nombreUsuario); // Devuelve un objeto Usuario cuyo NombreUsuario coincida con el parámetro pasado. En caso de no encontrarlo devuelve un objeto Usuario inicializado por defecto.
            if (usuarioEnBD.id != 0) // Si el Id está en cero es porque el método devolvío un Usuario inicializado por defecto, es decir, no hay Usuario con mismo nombreUsuario en BD.
            {
                return resultado; // Salgo del método y devuelvo False.
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD.
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Usuario] (Nombre, Apellido, NombreUsuario, Contraseña, Mail) " + // Query que me permite crear un Usuario.
                                        "VALUES (@nombre, @apellido, @nombreUsuario, @contraseña, @mail) ";

                var parameterNombre = new SqlParameter("nombre", SqlDbType.VarChar);    // Creo un nuevo objeto SqlParameter, de tipo VarChar, para expecificar "@nombre".
                parameterNombre.Value = usuario.nombre;                                 // Asigno valor a sqlParameter.

                var parameterApellido = new SqlParameter("apellido", SqlDbType.VarChar);
                parameterApellido.Value = usuario.apellido;

                var parameterNombreUsuario = new SqlParameter("nombreUsuario", SqlDbType.VarChar);
                parameterNombreUsuario.Value = usuario.nombreUsuario;

                var parameterContraseña = new SqlParameter("contraseña", SqlDbType.VarChar);
                parameterContraseña.Value = usuario.contraseña;

                var parameterMail = new SqlParameter("mail", SqlDbType.VarChar);
                parameterMail.Value = usuario.mail;

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection)) // Creo un objeto SqlCommand con una query previamente definida.
                {
                    sqlCommand.Parameters.Add(parameterNombre);                             // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.
                    sqlCommand.Parameters.Add(parameterApellido);
                    sqlCommand.Parameters.Add(parameterNombreUsuario);
                    sqlCommand.Parameters.Add(parameterContraseña);
                    sqlCommand.Parameters.Add(parameterMail);
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



        public static bool EliminarUsuario(long id) // Eliminar Usuario: Método que recibe un id de usuario eliminar y debe eliminarlo de la base de datos
                                                     // (eliminar antes sus productos vendidos también, sino no lo podrá hacer).
        {
            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no eliminar el usuario.
            int rowsAffected = 0; // Variable que indica la cantidad de filas que fueron afectadas al realizar ExecuteNonQuery()

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString de mi BD
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Usuario] " + // Query que me permite eliminar una fila de la tabla Usuario.
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, de tipo BigInt, para expecificar "@id".
                parameterId.Value = id; // Asigno valor a sqlParameter

                sqlConnection.Open(); // Abro la conexión con la BD

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection)) // Creo un objeto SqlCommand con queryDelete
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
    }
}