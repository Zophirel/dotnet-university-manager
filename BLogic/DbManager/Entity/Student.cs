using System.Configuration;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic.DbManager.Entity {
    internal class StudentLogic {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
       
        private readonly static List<Student> Students = DbUtils.GetAll<Student>();
    
        #region Create

        /// <summary>
        /// Inserts a new Professor object into the DB by collecting user input and saving it to its DB table.
        /// </summary>
        public static bool CreateStudent()
        {
            bool insertOK = false;
            try
            {
                string? fullName = Utils.GetValidInput("Enter Student Full Name: ", input => !string.IsNullOrEmpty(input));
 
                string gender = Utils.GetGender();
 
                string address = Utils.GetValidInput("Enter Student Address: ", input => !string.IsNullOrEmpty(input));
 
                string email = Utils.GetValidInput("Enter Student Email: ", input => !string.IsNullOrEmpty(input));
 
                string phone = Utils.GetValidInput("Enter Student Phone Number: ", input => !string.IsNullOrEmpty(input));
 
                string birthYearString = Utils.GetValidInput("Insert new birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
 
                string isFullTime = Utils.GetValidInput("Enter Full Time (y / n) ", input => !string.IsNullOrEmpty(input) && (input?.ToUpper() == "Y" || input?.ToUpper() == "N"));
 
                string maritalStatus = Utils.GetMaritialStatus();
 
                string matricola = Utils.GetValidInput("Enter Student Matricola: ", input => !string.IsNullOrEmpty(input) && input.Length == 7);
 
                string registrationYear = Utils.GetValidInput("Insert registration year (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
 
                string degree = Utils.GetDegree();
 
                string isee = Utils.GetValidInput("Insert ISEE ", input => decimal.TryParse(input, out _) && Convert.ToDecimal(input) > 0);

                using SqlConnection connection = new (ConnectionString);
                connection.Open();
                
                using SqlCommand command = new ("INSERT INTO Student (Id, [FullName],[Gender],[Address],[Email],[Phone],[BirthYear],[IsFullTime],[MaritalStatus],[Matricola],[RegistrationYear],[Degree],[ISEE]) VALUES (@Id, @fullName, @gender, @address, " +
                    "@email, @phone, @birthYearString, @isFullTime, @maritalStatus, @matricola, @registrationYear, @degree, @isee)", connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = Guid.NewGuid();
                command.Parameters.Add(new SqlParameter("@fullName", SqlDbType.Text)).Value = fullName;
                command.Parameters.Add(new SqlParameter("@gender", SqlDbType.NChar, 1)).Value = gender;
                command.Parameters.Add(new SqlParameter("@address", SqlDbType.Text)).Value = address;
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.Text)).Value = email;
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NChar, 13)).Value = phone;
                command.Parameters.Add(new SqlParameter("@birthYearString", SqlDbType.Date)).Value = birthYearString;
                command.Parameters.Add(new SqlParameter("@isFullTime", SqlDbType.Bit)).Value = isFullTime.Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                command.Parameters.Add(new SqlParameter("@maritalStatus", SqlDbType.SmallInt)).Value = maritalStatus;
                command.Parameters.Add(new SqlParameter("@matricola", SqlDbType.NChar, 7)).Value = matricola;
                command.Parameters.Add(new SqlParameter("@registrationYear", SqlDbType.Date)).Value = registrationYear;
                command.Parameters.Add(new SqlParameter("@degree", SqlDbType.SmallInt)).Value = degree;
                command.Parameters.Add(new SqlParameter("@isee", SqlDbType.Decimal)).Value = isee;
 
                command.Parameters.Add("@NumberRows", SqlDbType.Int).Direction = ParameterDirection.Output;
                int y = Convert.ToInt16(command.Parameters["@NumberRows"].Value);
 
                command.ExecuteNonQuery();
                insertOK = true;

                Console.WriteLine("Student inserted successfully");
            }
            catch (Exception)
            {
 
                throw;
            }
 
            return insertOK;
        }
    
        #endregion
        
        #region Read

        public static void ReadStudents()
        {
            try
            {
                foreach (Student student in Students)
                {
                    Console.WriteLine("Student Details:");
                    Console.WriteLine($"ID: {student.Id}");
                    Console.WriteLine($"FullName: {student.FullName}");
                    Console.WriteLine($"Gender: {student.Gender}");
                    Console.WriteLine($"Address: {student.Address}");
                    Console.WriteLine($"Email: {student.Email}");
                    Console.WriteLine($"Phone: {student.Phone}");
                    Console.WriteLine($"BirthYear: {student.BirthYear}");
                    Console.WriteLine($"IsFullTime: {student.IsFullTime}");
                    Console.WriteLine($"MaritalStatus: {student.MaritalStatus}");
                    Console.WriteLine($"Matricola: {student.Matricola}");
                    Console.WriteLine($"RegistrationYear: {student.RegistrationYear}");
                    Console.WriteLine($"Degree: {student.Degree}");
                    Console.WriteLine($"ISEE: {student.ISEE}");
                    Console.WriteLine(separator);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }
       
        public static void ReadStudent(string matricola)
        {
            try
            {
                Student? student = Students.Find(s => s.Matricola == matricola);
                if(student == null)
                {
                    Console.WriteLine("Student not found");
                    return;
                } else { 
                    Console.WriteLine("Student Details:");
                    Console.WriteLine($"ID: {student.Id}");
                    Console.WriteLine($"FullName: {student.FullName}");
                    Console.WriteLine($"Gender: {student.Gender}");
                    Console.WriteLine($"Address: {student.Address}");
                    Console.WriteLine($"Email: {student.Email}");
                    Console.WriteLine($"Phone: {student.Phone}");
                    Console.WriteLine($"BirthYear: {student.BirthYear}");
                    Console.WriteLine($"IsFullTime: {student.IsFullTime}");
                    Console.WriteLine($"MaritalStatus: {student.MaritalStatus}");
                    Console.WriteLine($"Matricola: {student.Matricola}");
                    Console.WriteLine($"RegistrationYear: {student.RegistrationYear}");
                    Console.WriteLine($"Degree: {student.Degree}");
                    Console.WriteLine($"ISEE: {student.ISEE}");
                    Console.WriteLine(separator);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }

        #endregion

        #region Update
    
        public static bool UpdateStudent()
        {
            bool insertOK = false;
            bool loop = true;
 
            try
            {
                using SqlConnection connection = new (ConnectionString);
                connection.Open();
                string? matricola = Utils.GetValidInput("Insert the Matricola of the student ", input => !string.IsNullOrEmpty(input) && input.Length == 7);
                Student? stud = Students.Find(s => s.Matricola == matricola);
                
                
                if(stud == null)
                {
                    Console.WriteLine("Student not found");
                    return false;
                }
            
                Student student = stud;
                Faculties studFaculty =  stud.Faculty;
 
                Console.WriteLine($"Changing student {stud.FullName} infos");
                while (loop)
                {
                    string prompt =
                    """
                    1.  Change Full Name
                    2.  Change Gender
                    3.  Change Address
                    4.  Change Email
                    5.  Change Phone
                    6.  Change Birth Year
                    7.  Change Status
                    8.  Change Full Time
                    9.  Change Registration Year
                    10. Change Faculty
                    11. Change Degree
                    12. Change ISEE
                    """;
 
                    Console.WriteLine(prompt);
                    string n = Utils.GetValidInput("Which info do you want to change? (press a number)", input => int.Parse(input!) > 0 && int.Parse(input!) < 13);
                    switch (n)
                    {
                        case "1":
                            stud!.FullName = Utils.GetValidInput("Insert new name ", input => !string.IsNullOrEmpty(input));
                            break;
 
                        case "2":
                            stud!.Gender = Utils.GetGender();
                            break;
 
                        case "3":
                            stud!.Address = Utils.GetValidInput("Insert new address ", input => !string.IsNullOrEmpty(input));
                            break;
 
                        case "4":
                            stud!.Email = Utils.GetValidInput("Insert new email ", input => !string.IsNullOrEmpty(input));
                            break;
 
                        case "5":
                            stud!.Phone = Utils.GetValidInput("Insert new phone ", input => !string.IsNullOrEmpty(input));
                            break;
 
                        case "6":
                            string birthYearString = Utils.GetValidInput("Insert new birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                            stud!.BirthYear = DateTime.Parse(birthYearString);
                            break;
 
                        case "7":
                            string maritalStatus = Utils.GetMaritialStatus();
                            stud!.MaritalStatus = (Status)int.Parse(maritalStatus);
                            break;
 
                        case "8":
                            Console.WriteLine($"Change Full Time: from {stud!.IsFullTime} to {!stud.IsFullTime} ? (y / n)");
                            char answer = Console.ReadKey().KeyChar;
                            if (answer == 'y')
                            {
                                stud.IsFullTime = !stud.IsFullTime;
                            }
                            break;
 
                        case "9":
                            string registrationYear = Utils.GetValidInput("Insert new registration year (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                            stud!.RegistrationYear = DateTime.Parse(registrationYear);
                            break;
 
                        case "10":
                            string faculty = Utils.GetFaculty();
                            stud!.Faculty = (Faculties)int.Parse(faculty);
                            break;
 
                        case "11":
                            string degree = Utils.GetDegree();
                            stud!.Degree = (Degrees)int.Parse(degree);
                            break;
 
                        case "12":
                            string isee = Utils.GetValidInput("Insert new ISEE ", input => decimal.TryParse(input, out _) && Convert.ToDecimal(input) > 0);
                            stud!.ISEE = Convert.ToDecimal(isee);
                            break;
 
                        default:
                            Console.WriteLine("Invalid input.");
                            break;
                    }
 
                    Console.WriteLine("Do you want to change something else about this student? (y / n)");
                    char answerFinal = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    
                    if (answerFinal == 'n')
                    {
                        
                        loop = false;
                        using SqlCommand command = new ("UPDATE Student SET FullName = @fullName, Gender = @gender, Address = @address, Email = @email, Phone = @phone, BirthYear = @birthYear, IsFullTime = @isFullTime," +
                            "MaritalStatus = @maritalStatus, RegistrationYear = @registrationYear, Degree = @degree, ISEE = @isee " +
                            "FROM Student WHERE Student.Matricola = @Matricola AND Deleted = 0", connection);
                        
                        command.Parameters.AddWithValue("@Matricola", matricola);
                        command.Parameters.Add(new SqlParameter("@fullName", SqlDbType.NVarChar, 50)).Value = stud.FullName;
                        command.Parameters.Add(new SqlParameter("@gender", SqlDbType.NChar, 1)).Value = stud.Gender;
                        command.Parameters.Add(new SqlParameter("@address", SqlDbType.NVarChar, 50)).Value = stud.Address;
                        command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 50)).Value = stud.Email;
                        command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NChar, 13)).Value = stud.Phone;
                        command.Parameters.Add(new SqlParameter("@birthYear", SqlDbType.Date)).Value = stud.BirthYear;
                        command.Parameters.Add(new SqlParameter("@isFullTime", SqlDbType.Bit)).Value = stud.IsFullTime;
                        command.Parameters.Add(new SqlParameter("@maritalStatus", SqlDbType.SmallInt)).Value = stud.MaritalStatus;
                        command.Parameters.Add(new SqlParameter("@registrationYear", SqlDbType.Date)).Value = stud.RegistrationYear;
                        command.Parameters.Add(new SqlParameter("@degree", SqlDbType.SmallInt)).Value = stud.Degree;
                        command.Parameters.Add(new SqlParameter("@isee", SqlDbType.Decimal)).Value = stud.ISEE;
 
                        command.Parameters.Add("@NumberRows", SqlDbType.Int).Direction = ParameterDirection.Output;
                        int y = Convert.ToInt16(command.Parameters["@NumberRows"].Value);
                        command.ExecuteNonQuery();

                        Students.Remove(student);
                        Students.Add(stud);

                        Console.WriteLine("Update saved");
                    }
                }
                insertOK = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
 
            return insertOK;
        }
    
        #endregion

        #region Deleted

        public static void DeleteStudent()
        {
            try
            {
                string? matricola = Utils.GetValidInput("Insert the Matricola of the student you want to delete", input => !string.IsNullOrEmpty(input));
                
                using SqlConnection connection = new (ConnectionString);
                connection.Open();

                Student? student = Students.Find(s => s.Matricola == matricola);
                if(student == null){
                    Console.WriteLine("Student not found");
                    return;
                } 
                
                string fullName = student.FullName;
        
                Console.WriteLine($"Are you sure you want to Deleted the Student: {student.FullName} ? (y / n)");
                char answer = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (answer == 'y')
                {
                    string query = "UPDATE Student SET Deleted = 1 WHERE Matricola = @Matricola";
                    using SqlCommand command = new (query, connection);
                    command.Parameters.Add(new SqlParameter("@Matricola", SqlDbType.NChar, 7)).Value = matricola;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Student removed successfully");
                    Students.Remove(student);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.SaveExeptionInDb(ex);
            }
        }
        
        #endregion
    }
}