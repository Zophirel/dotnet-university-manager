using System.Configuration;
using Microsoft.Data.SqlClient;

namespace University.BLogic {
    internal static class ExceptionLogger {
        static readonly string  ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        public static void SaveExeptionInDb(Exception ex) {
            Console.WriteLine($"Exception: {ex.Message}");


            using SqlConnection connection = new(ConnectionString);
            using SqlCommand command = new("INSERT INTO LOG (Message, StackTrace) VALUES (@Message, @StackTrace)", connection);
            command.Parameters.AddWithValue("@Message", ex.Message);
            command.Parameters.AddWithValue("@StackTrace", ex.StackTrace); 
            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("Exception saved in DB");
        }
    }
}