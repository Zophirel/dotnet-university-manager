using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic.DbManager.Relationship {
    internal class ProfessorCourseLogic {
        private readonly static string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
       
        private readonly static List<Professor> Professors = DbUtils.GetAll<Professor>();
        
        #region Create
        private static void CreateProfessorCourse(Guid professorId, Guid courseId, SqlConnection connection)
        {
            string insertQuery = @"
            INSERT INTO ProfessorCourse (ProfessorId, CourseId)
            VALUES (@ProfessorId, @CourseId)";

            using SqlCommand insertCommand = new(insertQuery, connection);
            insertCommand.Parameters.Add(new SqlParameter("@ProfessorId", SqlDbType.UniqueIdentifier)).Value = professorId;
            insertCommand.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.UniqueIdentifier)).Value = courseId;

            int rowsAffected = insertCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Association between Professor and Course created successfully.");
            }
            else
            {
                Console.WriteLine("Failed to create the association.");
            }
        }

        #endregion
        
        #region Read

        public static void ReadAllProfessorCourse(){
        try
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            string query = @"
            SELECT Professor.Id AS ProfessorId, Professor.FullName, Course.Id AS CourseId, Course.Name
            FROM ProfessorCourse
            INNER JOIN Professor ON Professor.Id = ProfessorCourse.ProfessorId
            INNER JOIN Course ON Course.Id = ProfessorCourse.CourseId
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
                Guid courseId = reader.GetGuid(2);
                string courseName = reader.GetString(3);
                Professor professor = Professors.First(p => p.Id == professorId);
                professor.Courses.Add(new Courses() { Id = courseId, Name = courseName });
            }


            foreach (Professor professor in Professors)
            {

                Console.WriteLine($"Professor: {professor.FullName} Id: {professor.Id}");
                foreach (Courses course in professor.Courses)  
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
    
        public static void ReadProfessorCourse(Guid id){
            try
            {
                using SqlConnection connection = new(ConnectionString);
                connection.Open();

                string query = @"
                SELECT Professor.Id AS ProfessorId, Professor.FullName, Course.Id AS CourseId, Course.Name
                FROM ProfessorCourse
                INNER JOIN Professor ON Professor.Id = @Id
                INNER JOIN Course ON Course.Id = ProfessorCourse.CourseId
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
                    Guid courseId = reader.GetGuid(2);
                    string courseName = reader.GetString(3);
                    Professor professor = Professors.First(p => p.Id == professorId);
                    professor.Courses.Add(new Courses() { Id = courseId, Name = courseName });
                }


                foreach (Professor professor in Professors)
                {

                    Console.WriteLine($"Professor: {professor.FullName} Id: {professor.Id}");
                    foreach (Courses course in professor.Courses)  
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

        public static void UpdateProfessorCourse(char answer)
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
                    SELECT Course.Id, Course.Name
                    FROM Course
                    INNER JOIN Professor ON Professor.Faculty = Course.Faculty
                    WHERE Professor.Id = @ProfessorId
                    AND Professor.Deleted = 0
                    
                    EXCEPT

                    SELECT Course.Id, Course.Name 
                    FROM Course
                    INNER JOIN ProfessorCourse ON ProfessorCourse.CourseId = Course.Id
                    WHERE ProfessorCourse.ProfessorId = @ProfessorId";
                } else {
                    // Query per trovare gli esami che il professore ha gia'
                    checkQuery = @"
                    SELECT Course.Id, Course.Name
                    FROM Course
                    INNER JOIN ProfessorCourse ON ProfessorCourse.CourseId = Course.Id
                    WHERE ProfessorCourse.ProfessorId = @ProfessorId";
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
                Console.WriteLine($"Available Courses for Professor: {professorName}");
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
                        CreateProfessorCourse(professorId, courseId, connection);
                        break;

                    case '2':
                        DeleteProfessorCourse(professorId, courseId, connection);
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

        private static void DeleteProfessorCourse(Guid professorId, Guid courseId, SqlConnection connection)
        {
            string deleteQuery = @"
            DELETE FROM ProfessorCourse
            WHERE ProfessorId = @ProfessorId AND CourseId = @CourseId";

            using SqlCommand deleteCommand = new(deleteQuery, connection);
            deleteCommand.Parameters.Add(new SqlParameter("@ProfessorId", SqlDbType.UniqueIdentifier)).Value = professorId;
            deleteCommand.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.UniqueIdentifier)).Value = courseId;

            int rowsAffected = deleteCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Association between Professor and Course deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete the association.");
            }
        }
        
        #endregion
    }
}