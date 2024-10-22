using System.Configuration;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic.DbManager.Entity {
    internal class ExamLogic {
        private readonly static string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
       
        private readonly static List<Exam> Exams = DbUtils.GetAll<Exam>();
        
        #region Create
        public static void CreateExam()
        {
            try
            {
                string? name = Utils.GetValidInput("Enter Exam Name: ", input => string.IsNullOrEmpty(input));
        
                string faculty = Utils.GetFaculty();
        
                string cfu = Utils.GetValidInput("Enter CFU (Credits): ", input => !string.IsNullOrEmpty(input) && int.TryParse(input, out _));
        
                string date = Utils.GetValidInput("Enter Exam Date (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        
                string isOnline = Utils.GetValidInput("Is the Exam Online? (true / false): ", input => !string.IsNullOrEmpty(input) && bool.TryParse(input, out _));
        
                string participants = Utils.GetValidInput("Enter Number of Participants: ", input => !string.IsNullOrEmpty(input) && int.TryParse(input, out _));
        
                string examType = Utils.GetExamType();
        
                string isProjectRequired = Utils.GetValidInput("Is a Project Required? (true/false): ", input => !string.IsNullOrEmpty(input) && bool.TryParse(input, out _));
        
                Exam exam = new()
                {
                    Name = name!,
                    Faculty = (Faculties)int.Parse(faculty),
                    CFU = int.Parse(cfu),
                    Date = DateTime.Parse(date),
                    IsOnline = bool.Parse(isOnline),
                    Participants = int.Parse(participants),
                    ExamType = (ExamType)int.Parse(examType),
                    IsProjectRequired = bool.Parse(isProjectRequired)
                };
        
                using SqlConnection connection = new(ConnectionString);
                using SqlCommand command = new ("INSERT INTO Exam (Id, Name, Faculty, CFU, ExamDate, IsOnline, ExamType, IsProjectRequired) VALUES (@Id, @Name, @Faculty, @CFU, @ExamDate, @IsOnline, @ExamType, @IsProjectRequired)", connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = exam.Id;
                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = exam.Name;
                command.Parameters.Add(new SqlParameter("@Faculty", SqlDbType.SmallInt)).Value = (int)exam.Faculty;
                command.Parameters.Add(new SqlParameter("@CFU", SqlDbType.SmallInt)).Value = exam.CFU;
                command.Parameters.Add(new SqlParameter("@ExamDate", SqlDbType.Date)).Value = exam.Date;
                command.Parameters.Add(new SqlParameter("@IsOnline", SqlDbType.Bit)).Value = exam.IsOnline;
                command.Parameters.Add(new SqlParameter("@ExamType", SqlDbType.SmallInt)).Value = exam.ExamType;
                command.Parameters.Add(new SqlParameter("@IsProjectRequired", SqlDbType.Bit)).Value = exam.IsProjectRequired;
                command.ExecuteNonQuery();


                // Update the list of exams
                Exams.Add(exam);

                Console.WriteLine("Exam added successfully");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        #endregion
        
        #region Read
        public static void ReadExams()
        {
            try
            {
                foreach (Exam exam in Exams)
                {
                    Console.WriteLine("Exam Details:");
                    Console.WriteLine($"Id: {exam.Id}");
                    Console.WriteLine($"Faculty:  {exam.Faculty}");
                    Console.WriteLine($"Exam Name: {exam.Name}");
                    Console.WriteLine($"CFU: {exam.CFU}\n");
                    Console.WriteLine($"Exam Date: {exam.Date}");
                    Console.WriteLine(exam.IsOnline ? "The exam is online" : "The exam takes place on site");
                    Console.WriteLine("Exam Type:");
                    Console.WriteLine(separator);
                    Console.WriteLine(separator);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }
       
        public static void ReadExam(Guid id)
        {
            try
            {
                Exam exam = Exams.First(p => p.Id == id);
                Console.WriteLine($"Id: {exam.Id}\n");
                Console.WriteLine($"Faculty:  {exam.Faculty}\n");
                Console.WriteLine($"Exam Name: {exam.Name}\n");
                Console.WriteLine($"CFU: {exam.CFU}\n");
                Console.WriteLine($"Exam Date: {exam.Date}\n");
                Console.WriteLine(exam.IsOnline ? "The exam is online" : "The exam takes place on site");
                Console.WriteLine("Exam Type:");
                Console.WriteLine(separator);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }

        #endregion

        #region Update

        public static bool UpdateExam()
        {
            bool updateOk = false;
            try
            {
                using SqlConnection connection = new(ConnectionString);
                using SqlCommand command = new ("SELECT COUNT(*) AS NumeroEsami FROM Exam WHERE Deleted = 0;", connection);
               
                using SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    int numberOfExams = dataReader.GetInt32(0);
                    if (numberOfExams == 0)
                    {
                        Console.WriteLine("There are no exams saved.\n");
                        return false;
                    }
                }

                dataReader.Close();

                Console.WriteLine("Which exam do you want to change?\n Insert ID:\n");
                Guid Id;
                while (!Guid.TryParse(Console.ReadLine(), out Id))
                {
                    Console.WriteLine("Invalid Guid. Please try again.");
                }

                using SqlCommand command2 = new ("SELECT * FROM Exam WHERE Id = @Id", connection);
                command2.Parameters.AddWithValue("@Id", Id);
               
                using SqlDataReader dataReader2 = command.ExecuteReader();
                if (!dataReader2.Read())
                {
                    Console.WriteLine("Error! Exam not found.\n");
                    return false;
                }

                // Create the new object exam based on the data provided
                Exam exam = new()
                {
                    Id = Id,  // Assegna l'Id all'oggetto exam
                    Name = dataReader2.GetString(1),
                    Faculty = (Faculties)dataReader2.GetInt16(2),
                    CFU = dataReader2.GetInt16(3),
                    Date = dataReader2.GetDateTime(4),
                    IsOnline = dataReader2.GetBoolean(5),
                    ExamType = (ExamType)dataReader2.GetInt16(6),
                    IsProjectRequired = dataReader2.GetBoolean(7)
                };

                dataReader2.Close();

                string prompt =
                """
                1 - Faculty
                2 - Name
                3 - CFU
                4 - Date
                5 - Modality (Online/On-site)
                6 - Number of participants
                7 - Exam type (Written, Oral, Written and Oral)
                8 - Project (is required or not)
                """;

                bool doWhile = true;

                do
                {
                    Console.WriteLine(ConfigurationManager.AppSettings["SeparationLine"]);
                    Console.WriteLine("What do you want to change about the exam?\n");
                    Console.WriteLine(prompt);
                    Console.WriteLine(ConfigurationManager.AppSettings["SeparationLine"]);

                    string? preChoice = Utils.GetValidInput("Enter a number ", input => !string.IsNullOrEmpty(input));
                    Console.WriteLine(ConfigurationManager.AppSettings["SeparationLine"]);
                    int choice = int.Parse(preChoice);

                    switch (choice)
                    {
                        case 1:
                            string faculty = Utils.GetFaculty();
                            exam.Faculty = (Faculties)int.Parse(faculty);
                            break;
                        case 2:
                            exam.Name = Utils.GetValidInput("Insert new name: ", input => !string.IsNullOrEmpty(input));
                            break;
                        case 3:
                            string? inputCfu = Utils.GetValidInput("Enter the new number of CFU: ", input => int.TryParse(input, out _));
                            exam.CFU = int.Parse(inputCfu);
                            break;
                        case 4:
                            string? inputExamDate = Utils.GetValidInput("Enter the new date (YYYY-MM-DD): ", input => DateTime.TryParse(input, out _));
                            exam.Date = DateTime.Parse(inputExamDate);
                            break;
                        case 5:
                            exam.IsOnline = Utils.GetValidInput("Enter 1 for Online or 0 for On-site: ", input => input == "1" || input == "0") == "1";
                            break;
                        case 6:
                            string? inputParticipants = Utils.GetValidInput("Enter the new number of participants: ", input => int.TryParse(input, out _));
                            exam.Participants = int.Parse(inputParticipants);
                            break;
                        case 7:
                            exam.ExamType = (ExamType)int.Parse(Utils.GetExamType());
                            break;
                        case 8:
                            exam.IsProjectRequired = Utils.GetValidInput("Enter 1 if the project is required, 0 if not: ", input => input == "1" || input == "0") == "1";
                            break;
                        default:
                            Console.WriteLine("Invalid input.");
                            break;
                    }

                    Console.WriteLine(ConfigurationManager.AppSettings["SeparationLine"]);

                    //Asks if want to change another thing of the exam
                    Console.WriteLine("Do you want to change something else about this exam? (y / n)\n");
                    char answerFinal = Console.ReadKey().KeyChar;

                    if (answerFinal == 'n')
                    {
                        doWhile = false;
                        dataReader.Close();

                        // Prepare the update query
                        using SqlCommand command3 = new (
                        @"UPDATE Exam
                            SET 
                                Name = @Name,
                                Faculty = @Faculty,
                                CFU = @CFU,
                                ExamDate = @ExamDate,
                                IsOnline = @IsOnline,
                                ExamType = @ExamType,
                                IsProjectRequired = @IsProjectRequired
                            WHERE Id = @Id", connection);

                        // Adds parameters
                        command3.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = exam.Name;
                        command3.Parameters.Add(new SqlParameter("@Faculty", SqlDbType.SmallInt)).Value = (int)exam.Faculty;
                        command3.Parameters.Add(new SqlParameter("@CFU", SqlDbType.SmallInt)).Value = exam.CFU;
                        command3.Parameters.Add(new SqlParameter("@ExamDate", SqlDbType.Date)).Value = exam.Date;
                        command3.Parameters.Add(new SqlParameter("@IsOnline", SqlDbType.Bit)).Value = exam.IsOnline;
                        command3.Parameters.Add(new SqlParameter("@ExamType", SqlDbType.SmallInt)).Value = exam.ExamType;
                        command3.Parameters.Add(new SqlParameter("@IsProjectRequired", SqlDbType.Bit)).Value = exam.IsProjectRequired;
                        command3.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = exam.Id;

                        // Execute the query
                        int rowsAffected = command3.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("Update failed, no rows affected.");
                        }
                        else
                        {
                            Console.WriteLine("Update successful.");
                            updateOk = true;
                        }
                    }

                } while (doWhile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n{ex.StackTrace}");
            }

            return updateOk;
        }

        #endregion

        #region Deleted
        public static void DeleteExam()
        {
            try
            {
                string? id = Utils.GetValidId();
    
                using SqlConnection connection = new(ConnectionString);
                connection.Open();
        
                string query = "SELECT Count(Id), Name FROM Exam WHERE Id = @Id GROUP BY Name";
        
                SqlCommand command = new(query, connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = Guid.Parse(id);
        
                SqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
        
                if (!dataReader.HasRows || dataReader.GetInt32(0) == 0)
                {
                    Console.WriteLine("Exam not found");
                    return;
                }
        
                string Name = dataReader.GetString(1);
                dataReader.Close();
        
                Console.WriteLine($"Are you sure you want to delete this exam?: {Name} ? (y / n)");
                char answer = Console.ReadKey().KeyChar;
                Console.WriteLine();
        
                if (answer == 'y')
                {
                    query = "UPDATE Exam SET Deleted = 1 WHERE Id = @Id";
                    SqlCommand command2 = new(query, connection);
                    command2.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = Guid.Parse(id);
                    command2.ExecuteNonQuery();
                    Console.WriteLine("Exam removed successfully");
                }
                else if (answer == 'n')
                {
                    Console.WriteLine("Exam not removed!");
                }
                else
                {
                    return;
                }
        
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        #endregion
    }
}