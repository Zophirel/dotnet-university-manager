using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic.DbManager.Relationship {
    internal class StudentCourseLogic {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
       
        private readonly static List<Student> Students = DbUtils.GetAll<Student>();
        
        #region Create
        private static void CreateStudentCourse(string matricola, Guid courseId, SqlConnection connection)
        {
            string insertQuery = @"
            INSERT INTO StudentCourse (matricola, CourseId)
            VALUES (@Matricola, @CourseId)";

            using SqlCommand insertCommand = new(insertQuery, connection);
            insertCommand.Parameters.Add(new SqlParameter("@Matricola", SqlDbType.NChar, 7)).Value = matricola;
            insertCommand.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.UniqueIdentifier)).Value = courseId;

            int rowsAffected = insertCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Association between Student and Course created successfully.");
            }
            else
            {
                Console.WriteLine("Failed to create the association.");
            }
        }

        #endregion
        
        #region Read

        public static void ReadAllStudentCourse(){
        try
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            string query = @"
            SELECT Student.Matricola AS Matricola, Student.FullName, Course.Id AS CourseId, Course.Name
            FROM StudentCourse
            INNER JOIN Student ON Student.Matricola = StudentCourse.Matricola
            INNER JOIN Course ON Course.Id = StudentCourse.CourseId
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
                Guid courseId = reader.GetGuid(2);
                string courseName = reader.GetString(3);
                Student student = Students.First(p => p.Matricola == matricola);
                student.Courses.Add(new Courses() { Id = courseId, Name = courseName });
            }


            foreach (Student student in Students)
            {

                Console.WriteLine($"Student: {student.FullName} Id: {student.Id}");
                foreach (Courses course in student.Courses)  
                {
                    Console.WriteLine($"- {course.Name}");
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
    
        public static void ReadStudentCourse(string matricola){
        try
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            string query = @"
            SELECT Student.Matricola AS Matricola, Student.FullName, Course.Id AS CourseId, Course.Name
            FROM StudentCourse
            INNER JOIN Student ON Student.Matricola = @matricola
            INNER JOIN Course ON Course.Id = StudentCourse.CourseId
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
                Guid courseId = reader.GetGuid(2);
                string courseName = reader.GetString(3);
                Student student = Students.First(p => p.Matricola == matricola);
                student.Courses.Add(new Courses() { Id = courseId, Name = courseName });
            }


            foreach (Student student in Students)
            {

                Console.WriteLine($"Student: {student.FullName} Id: {student.Id}");
                foreach (Courses course in student.Courses)  
                {
                    Console.WriteLine($"- {course.Name}");
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

        public static void UpdateStudentCourse(char answer)
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
                    SELECT Course.Id, Course.Name
                    FROM Course
                    INNER JOIN Student ON Student.Faculty = Course.Faculty
                    WHERE Student.Id = @Matricola
                    AND Student.Deleted = 0
                    
                    EXCEPT

                    SELECT Course.Id, Course.Name 
                    FROM Course
                    INNER JOIN StudentCourse ON StudentCourse.CourseId = Course.Id
                    WHERE StudentCourse.Matricola = @Matricola";
                } else {
                    // Query per trovare gli esami che il studente ha gia'
                    checkQuery = @"
                    SELECT Course.Id, Course.Name
                    FROM Course
                    INNER JOIN StudentCourse ON StudentCourse.CourseId = Course.Id
                    WHERE StudentCourse.Matricola = @Matricola";
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
                Console.WriteLine($"Available Courses for Student: {studentName}");
                while (reader.Read())
                {
                    Console.WriteLine($"Course: {reader["Id"]}, Name: {reader["Name"]}");
                }

                reader.Close();

                Console.WriteLine("Enter the Course Id of intrest:");
                Guid courseId = Guid.Parse(Utils.GetValidId());

                // Esegui l'operazione scelta
                switch (answer)
                {
                    case '1':
                        CreateStudentCourse(matricola, courseId, connection);
                        break;

                    case '2':
                        DeleteStudentCourse(matricola, courseId, connection);
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

        private static void DeleteStudentCourse(string matricola, Guid courseId, SqlConnection connection)
        {
            string deleteQuery = @"
            DELETE FROM StudentCourse
            WHERE Matricola = @Matricola AND CourseId = @CourseId";

            using SqlCommand deleteCommand = new(deleteQuery, connection);
            deleteCommand.Parameters.Add(new SqlParameter("@Matricola", SqlDbType.NChar, 7)).Value = matricola;
            deleteCommand.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.UniqueIdentifier)).Value = courseId;

            int rowsAffected = deleteCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Association between Student and Course deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete the association.");
            }
        }
        
        #endregion
    }
}