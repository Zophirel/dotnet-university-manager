using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic.DbManager.Relationship {
    internal class StudentExamLogic {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
       
        private readonly static List<Student> Students = DbUtils.GetAll<Student>();
        
        #region Create
        private static void CreateStudentExam(string matricola, Guid examId, SqlConnection connection)
        {
            string insertQuery = @"
            INSERT INTO StudentExam (matricola, ExamId)
            VALUES (@Matricola, @ExamId)";

            using SqlCommand insertCommand = new(insertQuery, connection);
            insertCommand.Parameters.Add(new SqlParameter("@Matricola", SqlDbType.NChar, 7)).Value = matricola;
            insertCommand.Parameters.Add(new SqlParameter("@ExamId", SqlDbType.UniqueIdentifier)).Value = examId;

            int rowsAffected = insertCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Association between Student and Exam created successfully.");
            }
            else
            {
                Console.WriteLine("Failed to create the association.");
            }
        }

        #endregion
        
        #region Read

        public static void ReadAllStudentExam(){
        try
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            string query = @"
            SELECT Student.Matricola AS Matricola, Student.FullName, Exam.Id AS ExamId, Exam.Name
            FROM StudentExam
            INNER JOIN Student ON Student.Matricola = StudentExam.Matricola
            INNER JOIN Exam ON Exam.Id = StudentExam.ExamId
            WHERE Student.Deleted = 0";

            using SqlCommand command = new(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("No associations found.");
                return;
            }

            while (reader.Read())
            {
                string matricola = reader.GetString(0);
                string studentName = reader.GetString(1);
                Guid examId = reader.GetGuid(2);
                string examName = reader.GetString(3);
                Student student = Students.First(p => p.Matricola == matricola);
                student.Exams.Add(new Exam() { Id = examId, Name = examName });
            }


            foreach (Student student in Students)
            {

                Console.WriteLine($"Student: {student.FullName} Id: {student.Id}");
                foreach (Exam exam in student.Exams)  
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
    
        public static void ReadStudentExam(string matricola){
        try
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            string query = @"
            SELECT Student.Matricola AS Matricola, Student.FullName, Exam.Id AS ExamId, Exam.Name
            FROM StudentExam
            INNER JOIN Student ON Student.Matricola = @Matricola
            INNER JOIN Exam ON Exam.Id = StudentExam.ExamId
            WHERE Student.Deleted = 0";

            using SqlCommand command = new(query, connection);
            command.Parameters.Add(new SqlParameter("@Matricola", SqlDbType.NChar, 7)).Value = matricola;
            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("No associations found.");
                return;
            }

            while (reader.Read())
            {
                matricola = reader.GetString(0);
                string studentName = reader.GetString(1);
                Guid examId = reader.GetGuid(2);
                string examName = reader.GetString(3);
                Student student = Students.First(p => p.Matricola == matricola);
                student.Exams.Add(new Exam() { Id = examId, Name = examName });
            }


            foreach (Student student in Students)
            {

                Console.WriteLine($"Student: {student.FullName} Id: {student.Id}");
                foreach (Exam exam in student.Exams)  
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

        public static void UpdateStudentExam(char answer)
        {
            try
            {

                Console.WriteLine("Choose the student:");
                foreach (Student student in Students)
                {
                    Console.WriteLine($"ID: {student.Id}, Name: {student.FullName}");
                }
               
                string matricola = Utils.GetValidInput("Enter Student matricola", p => p?.Length == 7);
                string studentName = Students.First(p => p.Matricola == matricola).FullName;
                
                using SqlConnection connection = new(ConnectionString);
                connection.Open();
                string checkQuery = "";
                
                if(answer == '1'){
                    // Query per trovare gli esami che il studente può fare, escludendo quelli già assegnati
                    checkQuery = @"
                    SELECT Exam.Id, Exam.Name
                    FROM Exam
                    INNER JOIN Student ON Student.Faculty = Exam.Faculty
                    WHERE Student.Id = @Matricola
                    AND Student.Deleted = 0
                    
                    EXCEPT

                    SELECT Exam.Id, Exam.Name 
                    FROM Exam
                    INNER JOIN StudentExam ON StudentExam.ExamId = Exam.Id
                    WHERE StudentExam.Matricola = @Matricola";
                } else {
                    // Query per trovare gli esami che il studente ha gia'
                    checkQuery = @"
                    SELECT Exam.Id, Exam.Name
                    FROM Exam
                    INNER JOIN StudentExam ON StudentExam.ExamId = Exam.Id
                    WHERE StudentExam.Matricola = @Matricola";
                }
                using SqlCommand checkCommand = new(checkQuery, connection);
                checkCommand.Parameters.Add(new SqlParameter("@Matricola", SqlDbType.NChar, 7)).Value = matricola;

                using SqlDataReader reader = checkCommand.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("No available exams for the student or the student already has these exams.");
                    return;
                }

                // Visualizza gli esami che il studente può fare
                Console.WriteLine($"Available Exams for Student: {studentName}");
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
                        CreateStudentExam(matricola, examId, connection);
                        break;

                    case '2':
                        DeleteStudentExam(matricola, examId, connection);
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

        private static void DeleteStudentExam(string matricola, Guid examId, SqlConnection connection)
        {
            string deleteQuery = @"
            DELETE FROM StudentExam
            WHERE Matricola = @Matricola AND ExamId = @ExamId";

            using SqlCommand deleteCommand = new(deleteQuery, connection);
            deleteCommand.Parameters.Add(new SqlParameter("@Matricola", SqlDbType.NChar, 7)).Value = matricola;
            deleteCommand.Parameters.Add(new SqlParameter("@ExamId", SqlDbType.UniqueIdentifier)).Value = examId;

            int rowsAffected = deleteCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Association between Student and Exam deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete the association.");
            }
        }
        
        #endregion
    }
}