using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace University.DataModel {
    internal class UniDbManager {
        private SqlConnection _connection = new (ConfigurationManager.AppSettings["DbConnectionString"]!);
        public readonly bool isOnline = false;
        public UniDbManager() {
            try {
                _connection.Open();
                isOnline = true;

            } catch (SqlException e) {
                Console.WriteLine(e.Message);
                isOnline = false;
            } finally {
                if(_connection.State == ConnectionState.Open) {
                    _connection.Close();
                }
            }
           
        }
        
    }
}