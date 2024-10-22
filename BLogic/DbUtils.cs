
using System.Configuration;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic
{
    public static class DbUtils
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        
        private static Student DeserializeStudent(SqlDataReader reader)
        {
            return new Student(){
                Id = reader.GetGuid(0),
                FullName = reader.GetString(1),
                Gender = reader.GetString(2),
                Address = reader.GetString(3),
                Email = reader.GetString(4),
                Phone = reader.GetString(5),
                BirthYear = reader.GetDateTime(6),
                IsFullTime = reader.GetBoolean(7),
                MaritalStatus = (Status) reader.GetInt16(8),
                Matricola = reader.GetString(9),
                RegistrationYear = reader.GetDateTime(10),
                Degree = (Degrees)reader.GetInt16(11),
                ISEE = reader.GetDecimal(12)
            };
        }

        private static Exam DeserializeExam(SqlDataReader reader)
        {
           return new Exam()
            {
                Id = reader.GetGuid(0),  // Assegna l'Id all'oggetto exam
                Name = reader.GetString(1),
                Faculty = (Faculties)reader.GetInt16(2),
                CFU = reader.GetInt16(3),
                Date = reader.GetDateTime(4),
                IsOnline = reader.GetBoolean(5),
                ExamType = (ExamType) reader.GetInt16(6),
                IsProjectRequired = reader.GetBoolean(7)
            };
        }

        private static Professor DeserializeProfessor(SqlDataReader reader)
        {
            return new Professor(){
                Id = reader.GetGuid(0),
                FullName = reader.GetString(1),
                Gender = reader.GetString(2),
                Address = reader.GetString(3),
                Email = reader.GetString(4),
                Phone = reader.GetString(5),
                BirthYear = reader.GetDateTime(6),
                IsFullTime = reader.GetBoolean(7),
                MaritalStatus = (Status) reader.GetInt16(8),
                Role = (Roles) reader.GetInt16(9),
                Faculty = (Faculties) reader.GetInt16(10),
                HiringYear= reader.GetDateTime(11),
                Salary = reader.GetDecimal(12),
            };
        }

        private static Employee DeserializeEmployee(SqlDataReader reader)
        {
            return new Employee(){
                Id = reader.GetGuid(0),
                FullName = reader.GetString(1),
                Gender = reader.GetString(2),
                Address = reader.GetString(3),
                Email = reader.GetString(4),
                Phone = reader.GetString(5),
                BirthYear = reader.GetDateTime(6),
                IsFullTime = reader.GetBoolean(7),
                MaritalStatus = (Status) reader.GetInt16(8),
                Role = (Roles) reader.GetInt16(9),
                Faculty = (Faculties) reader.GetInt16(10),
                HiringYear= reader.GetDateTime(11),
                Salary = reader.GetDecimal(12),
            };
        }

        private static T? Deserialize<T>(SqlDataReader reader)
        {
            if(typeof(T) == typeof(Employee))
            {
                return (T)(object) DeserializeEmployee(reader);
            }
            else if(typeof(T) == typeof(Professor))
            {
                return (T)(object) DeserializeProfessor(reader);
            }
            else if(typeof(T) == typeof(Student))
            {
                return (T)(object) DeserializeStudent(reader);
            }else if(typeof(T) == typeof(Exam))
            {
                return (T)(object) DeserializeExam(reader);
            }
            else
            {
                throw new Exception("Type not supported");
            }
        }

        public static List<T> GetAll<T>(){
            List<T> data = [];
            using SqlConnection connection = new (ConnectionString);
            
            connection.Open();
            string query = $"SELECT * FROM {typeof(T).Name}";
            using SqlCommand command = new(query, connection);

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                data.Add(Deserialize<T>(reader) ?? throw new Exception("Error deserializing data"));
            }
            return data;
        }
    }
}