using System.Data;
using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static SqlConnection connection;
        private static string stringConnection;

        static DataBaseManager()
        {
            connection = new SqlConnection();
            stringConnection = "Server = DESKTOP-NP3CB0L\\MSSQLSERVER777; Database = 20230622SP; Trusted_Connection = True";
        }

        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = $"SELECT * FROM comidas WHERE tipo_comida = @tipo";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        throw new ComidaInvalidaExeption("No se encontro el tipo en la base de datos");
                    }
                    return reader.GetString(2);
                }
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
                throw;
            }
        }

        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
        {
            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = $"INSERT INTO tickes (empleado, ticket) VALUES (@empleado, @ticket)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@empleado", nombreEmpleado);
                    cmd.Parameters.AddWithValue("@ticket", comida.Ticket);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch(Exception ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
                throw new DataBaseManagerException("Error al escribir");
            }
        }
    }
}
