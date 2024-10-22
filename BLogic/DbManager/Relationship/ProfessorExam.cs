using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic.DbManager.Relationship {
    internal class ProfessorExamLogic {
        private readonly static string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
       
        private readonly static List<Professor> Professors = DbUtils.GetAll<Professor>();
        
        #region Create
        private static void CreateProfessorExam(Guid professorId, Guid examId, SqlConnection connection)
        {
            string insertQuery = @"
            INSERT INTO ProfessorExam (ProfessorId, ExamId)
            VALUES (@ProfessorId, @ExamId)";

            using SqlCommand insertCommand = new(insertQuery, connection);
            insertCommand.Parameters.Add(new SqlParameter("@ProfessorId", SqlDbType.UniqueIdentifier)).Value = professorId;
            insertCommand.Parameters.Add(new SqlParameter("@ExamId", SqlDbType.UniqueIdentifier)).Value = examId;

            int rowsAffected = insertCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Association between Professor and Exam created successfully.");
            }
            else
            {
                Console.WriteLine("Failed to create the association.");
            }
        }

        #endregion
        
        #region Read

        public static void ReadAllProfessorExam(){
        try
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            string query = @"
            SELECT Professor.Id AS ProfessorId, Professor.FullName, Exam.Id AS ExamId, Exam.Name
            FROM ProfessorExam
            INNER JOIN Professor ON Professor.Id = ProfessorExam.ProfessorId
            INNER JOIN Exam ON Exam.Id = ProfessorExam.ExamId
            WHERE Professor.Deleted = 0";

            using SqlCommand command = new(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("No associations found.");
                return;
            }
            
            while (reader.Read())
            {
                Guid professorId = reader.GetGuid(0);
                string professorName = reader.GetString(1);
                Guid examId = reader.GetGuid(2);
                string examName = reader.GetString(3);
                Professor professor = Professors.First(p => p.Id == professorId);
                professor.Exams.Add(new Exam() { Id = examId, Name = examName });
            }


            foreach (Professor professor in Professors)
            {

                Console.WriteLine($"Professor: {professor.FullName} Id: {professor.Id}");
                foreach (Exam exam in professor.Exams)  
                {
                    Console.WriteLine($"- {exam.Name}");
                }
                Console.WriteLine(separator);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"{ex.StackTrace}");
        }
    }
    
        public static void ReadProfessorExam(Guid id){
        try
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            string query = @"
            SELECT Professor.Id AS ProfessorId, Professor.FullName, Exam.Id AS ExamId, Exam.Name
            FROM ProfessorExam
            INNER JOIN Professor ON Professor.Id = @Id
            INNER JOIN Exam ON Exam.Id = ProfessorExam.ExamId
            WHERE Professor.Deleted = 0";

            using SqlCommand command = new(query, connection);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = id;
            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("No associations found.");
                return;
            }
            
            while (reader.Read())
            {
                Guid professorId = reader.GetGuid(0);
                string professorName = reader.GetString(1);
                Guid examId = reader.GetGuid(2);
                string examName = reader.GetString(3);
                Professor professor = Professors.First(p => p.Id == professorId);
                professor.Exams.Add(new Exam() { Id = examId, Name = examName });
            }


            foreach (Professor professor in Professors)
            {

                Console.WriteLine($"Professor: {professor.FullName} Id: {professor.Id}");
                foreach (Exam exam in professor.Exams)  
                {
                    Console.WriteLine($"- {exam.Name}");
                }
                Console.WriteLine(separator);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"{ex.StackTrace}");
        }
    }
    

        #endregion

        #region Update

        public static void UpdateProfessorExam(char answer)
        {
            try
            {

                Console.WriteLine("Choose the professor:");
                foreach (Professor professor in Professors)
                {
                    Console.WriteLine($"ID: {professor.Id}, Name: {professor.FullName}");
                }
               
                Guid professorId = Guid.Parse(Utils.GetValidId());
                string professorName = Professors.First(p => p.Id == professorId).FullName;
                
                using SqlConnection connection = new(ConnectionString);
                connection.Open();
                string checkQuery = "";
                
                if(answer == '1'){
                    // Query per trovare gli esami che il professore può fare, escludendo quelli già assegnati
                    checkQuery = @"
                    SELECT Exam.Id, Exam.Name
                    FROM Exam
                    INNER JOIN Professor ON Professor.Faculty = Exam.Faculty
                    WHERE Professor.Id = @ProfessorId
                    AND Professor.Deleted = 0
                    
                    EXCEPT

                    SELECT Exam.Id, Exam.Name 
                    FROM Exam
                    INNER JOIN ProfessorExam ON ProfessorExam.ExamId = Exam.Id
                    WHERE ProfessorExam.ProfessorId = @ProfessorId";
                } else {
                    // Query per trovare gli esami che il professore ha gia'
                    checkQuery = @"
                    SELECT Exam.Id, Exam.Name
                    FROM Exam
                    INNER JOIN ProfessorExam ON ProfessorExam.ExamId = Exam.Id
                    WHERE ProfessorExam.ProfessorId = @ProfessorId";
                }
                using SqlCommand checkCommand = new(checkQuery, connection);
                checkCommand.Parameters.Add(new SqlParameter("@ProfessorId", SqlDbType.UniqueIdentifier)).Value = professorId;

                using SqlDataReader reader = checkCommand.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("No available exams for the professor or the professor already has these exams.");
                    return;
                }

                // Visualizza gli esami che il professore può fare
                Console.WriteLine($"Available Exams for Professor: {professorName}");
                while (reader.Read())
                {
                    Console.WriteLine($"Exam: {reader["Id"]}, Name: {reader["Name"]}");
                }

                reader.Close();

                Console.WriteLine("Enter the Exam Id of intrest:");
                Guid examId = Guid.Parse(Utils.GetValidId());

                // Esegui l'operazione scelta
                switch (answer)
                {
                    case '1':
                        CreateProfessorExam(professorId, examId, connection);
                        break;

                    case '2':
                        DeleteProfessorExam(professorId, examId, connection);
                        break;

                    default:
                        Console.WriteLine("Operation type not recognized. Use '1' to Create or '2' to Delete.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }

        #endregion

        #region Deleted

        private static void DeleteProfessorExam(Guid professorId, Guid examId, SqlConnection connection)
        {
            string deleteQuery = @"
            DELETE FROM ProfessorExam
            WHERE ProfessorId = @ProfessorId AND ExamId = @ExamId";

            using SqlCommand deleteCommand = new(deleteQuery, connection);
            deleteCommand.Parameters.Add(new SqlParameter("@ProfessorId", SqlDbType.UniqueIdentifier)).Value = professorId;
            deleteCommand.Parameters.Add(new SqlParameter("@ExamId", SqlDbType.UniqueIdentifier)).Value = examId;

            int rowsAffected = deleteCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Association between Professor and Exam deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete the association.");
            }
        }
        
        #endregion
    }
}