using System.Configuration;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic.DbManager.Entity {
    internal class EmployeeLogic {
        private readonly static string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
       
        private readonly static List<Employee> Employees = DbUtils.GetAll<Employee>();
        
        #region Create
        /// <summary>
        /// Inserts a new Employee object into the DB by collecting user input and saving it to its DB table.
        /// </summary>
        public static void CreateEmployee()
        {
            try
            {
                string? fullName = Utils.GetValidInput("Enter Employee Full Name: ", input => !string.IsNullOrEmpty(input));

                string? gender = Utils.GetGender();

                string? address = Utils.GetValidInput("Enter Address: ", input => !string.IsNullOrEmpty(input));

                string? email = Utils.GetValidInput("Enter Email: ", input => !string.IsNullOrEmpty(input));

                string? phone = Utils.GetValidInput("Enter Phone Number: ", input => !string.IsNullOrEmpty(input));

                string birthYearString = Utils.GetValidInput("Enter Birth Year (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));

                string isFullTimeString = Utils.GetValidInput("Is the Employee full-time? (y / n): ", input => !string.IsNullOrEmpty(input) && (input.ToUpper() == "Y" || input.ToUpper() == "N")); 
                 
                string maritalStatus = Utils.GetMaritialStatus();

                string role = Utils.GetRole();
                
                string faculty = Utils.GetFaculty();
            
                string hiringYearString = Utils.GetValidInput("Enter Hiring Day (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                
                string salaryString = Utils.GetValidInput("Enter Salary: ", input => !string.IsNullOrEmpty(input) && decimal.TryParse(input, out _));  

                int roleint = int.Parse(role);
                int statusint = int.Parse(maritalStatus);

                Employee employee = new()
                {
                    FullName = fullName,
                    Gender = gender,
                    Address = address,
                    Email = email,
                    Phone = phone,
                    BirthYear = DateTime.Parse(birthYearString),
                    IsFullTime = isFullTimeString.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                    MaritalStatus = (Status)statusint,
                    Role = (Roles)roleint,
                    Faculty = (Faculties) int.Parse(faculty),
                    HiringYear = DateTime.Parse(hiringYearString),
                    Salary = decimal.Parse(salaryString)
                };

                const string query = $"""
                INSERT INTO Employee 
                (FullName, Gender, Address, Email, Phone, BirthYear, IsFullTime, MaritalStatus, Role, Faculty, HiringYear, Salary)
                VALUES (@FullName, @Gender, @Address, @Email, @Phone, @BirthYear, @IsFullTime, @MaritalStatus, @Role, @Faculty, @HiringYear, @Salary)
                """;
                using SqlConnection connection = new (ConnectionString);
                connection.Open();
                
                using SqlCommand command = new (query, connection);

                command.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 50)).Value = employee.FullName;
                command.Parameters.Add(new SqlParameter("@Gender", SqlDbType.NChar, 1)).Value = employee.Gender;
                command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 50)).Value = employee.Address;
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50)).Value = employee.Email;
                command.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NChar, 13)).Value = employee.Phone;
                command.Parameters.Add(new SqlParameter("@BirthYear", SqlDbType.Date)).Value = employee.BirthYear;
                command.Parameters.Add(new SqlParameter("@IsFullTime", SqlDbType.Bit)).Value = employee.IsFullTime;
                command.Parameters.Add(new SqlParameter("@MaritalStatus", SqlDbType.SmallInt)).Value = employee.MaritalStatus;
                command.Parameters.Add(new SqlParameter("@Role", SqlDbType.SmallInt)).Value = employee.Role;
                command.Parameters.Add(new SqlParameter("@Faculty", SqlDbType.SmallInt)).Value =  employee.Faculty;
                command.Parameters.Add(new SqlParameter("@HiringYear", SqlDbType.Date)).Value = employee.HiringYear;
                command.Parameters.Add(new SqlParameter("@Salary", SqlDbType.Decimal)).Value = employee.Salary;
                command.ExecuteNonQuery();
                
            }
            catch (SqlException ex)
            {
                ExceptionLogger.SaveExeptionInDb(ex);
            }
        }

        #endregion
        
        #region Read
        public static void ReadEmployees()
        {
         
            try
            {
                foreach (var employee in Employees)
                {
                    Console.WriteLine("Employee Details:");
                    Console.WriteLine($"ID: {employee.Id}");
                    Console.WriteLine($"Full Name: {employee.FullName}");
                    Console.WriteLine($"Gender: {employee.Gender}");
                    Console.WriteLine($"Address: {employee.Address}");
                    Console.WriteLine($"Email: {employee.Email}");
                    Console.WriteLine($"Phone Number: {employee.Phone}");
                    Console.WriteLine($"Birth Year: {employee.BirthYear}");
                    Console.WriteLine($"Is Full Time: {employee.IsFullTime}");
                    Console.WriteLine($"Marital Status: {employee.MaritalStatus}");
                    Console.WriteLine($"Role: {employee.Role}");
                    Console.WriteLine($"Faculty: {employee.Faculty}");
                    Console.WriteLine($"Hiring Year: {employee.HiringYear}");
                    Console.WriteLine($"Salary: {employee.Salary}");
                    Console.WriteLine(separator);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.SaveExeptionInDb(ex);
            }
        }
    

        public static void ReadEmployee(Guid id)
        {
            try
            {
                Employee employee = Employees.First(e => e.Id == id);
                Console.WriteLine("Employee Details:");
                Console.WriteLine($"ID: {employee.Id}");
                Console.WriteLine($"Full Name: {employee.FullName}");
                Console.WriteLine($"Gender: {employee.Gender}");
                Console.WriteLine($"Address: {employee.Address}");
                Console.WriteLine($"Email: {employee.Email}");
                Console.WriteLine($"Phone Number: {employee.Phone}");
                Console.WriteLine($"Birth Year: {employee.BirthYear}");
                Console.WriteLine($"Is Full Time: {employee.IsFullTime}");
                Console.WriteLine($"Marital Status: {employee.MaritalStatus}");
                Console.WriteLine($"Role: {employee.Role}");
                Console.WriteLine($"Faculty: {employee.Faculty}");
                Console.WriteLine($"Hiring Year: {employee.HiringYear}");
                Console.WriteLine($"Salary: {employee.Salary}");
                Console.WriteLine(separator);
                
            }
            catch (Exception ex)
            {
                ExceptionLogger.SaveExeptionInDb(ex);
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// Update an Employee object from the DB by finding it, collecting user input and saving it to its DB record.
        /// </summary>
        public static void UpdateEmployee()
        {
            try
            {
                bool doWhile = true;
                string n;
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
                    9.  Change Faculty
                    10. Change Role
                    11. Change HiringYear
                    12. Change Salary
                    """;
            
                string? id = Utils.GetValidId();
                
                using SqlConnection connection = new (ConnectionString);
                connection.Open();
                
                string query = "SELECT Count(Id) FROM Employee WHERE Id = @Id AND Deleted = 0";
                
                using SqlCommand command = new (query, connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = Guid.Parse(id);
                
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                if(!reader.HasRows || reader.GetInt32(0) == 0) {
                    Console.WriteLine("Employee not found");
                    return;
               
                } else {
                    
                    reader.Close();

                    query = "SELECT * FROM Employee WHERE Id = @Id";
                    using SqlCommand command2 = new (query, connection);
                    command2.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = Guid.Parse(id);
                          
                    SqlDataReader reader2 = command2.ExecuteReader();
                   
                    reader2.Read();
                    Console.WriteLine($"Employee found: {reader2.GetValue(0)}");
                    Employee emp = new ()
                    {
                        Id = reader2.GetGuid(0),
                        FullName = reader2.GetString(1),
                        Gender = reader2.GetString(2),
                        Address = reader2.GetString(3),
                        Email = reader2.GetString(4),
                        Phone = reader2.GetString(5),
                        BirthYear = reader2.GetDateTime(6),
                        IsFullTime = reader2.GetBoolean(7),
                        MaritalStatus = (Status) reader2.GetInt16(8),
                        Role = (Roles) reader2.GetInt16(9),
                        Faculty = (Faculties) reader2.GetInt16(10),
                        HiringYear = reader2.GetDateTime(11),
                        Salary = reader2.GetDecimal(12),
                    };
                    
                    reader2.Close();

                    do
                    {
                        Console.WriteLine($"Changing employee {emp.FullName} infos");
                        Console.WriteLine(separator);
                        Console.WriteLine(prompt);
                        Console.WriteLine("Which info do you want to change? (press a number)");
                        n = Console.ReadKey().KeyChar.ToString();
                        switch (n)
                        {
                            case "1":
                                emp.FullName = Utils.GetValidInput("Insert new name ", input => !string.IsNullOrEmpty(input));
                                break;

                            case "2":
                                emp.Gender = Utils.GetGender();
                                break;

                            case "3":
                                emp.Address = Utils.GetValidInput("Insert new address ", input => !string.IsNullOrEmpty(input));
                                break;

                            case "4":
                                emp.Email = Utils.GetValidInput("Insert new email", input => !string.IsNullOrEmpty(input));
                                break;

                            case "5":
                                string phone = Utils.GetValidInput("Insert new phone ", input => !string.IsNullOrEmpty(input));
                                break;

                            case "6":
                                string birthYearString = Utils.GetValidInput("Insert new birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                                emp.BirthYear = DateTime.Parse(birthYearString);
                                break;

                            case "7":
                                string maritalStatus = Utils.GetMaritialStatus();
                                emp.MaritalStatus = (Status)int.Parse(maritalStatus);
                                break;

                            case "8":
                                Console.WriteLine($"Change Full Time: from {emp.IsFullTime} to {!emp.IsFullTime} ? (y / n)");
                                char answer = Console.ReadKey().KeyChar;
                                if (answer == 'y')
                                {
                                    emp.IsFullTime = !emp.IsFullTime;
                                }
                                break;

                            case "9":
                                string faculty = Utils.GetFaculty();
                                emp.Faculty = (Faculties)int.Parse(faculty);
                                break;

                            case "10":
                                string role = Utils.GetRole();
                                emp.Role = (Roles)int.Parse(role);
                                break;

                            case "11":
                                string newYear = Utils.GetValidInput("Insert new hiring year ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                                emp.HiringYear = DateTime.Parse(newYear);
                                break;

                            case "12":
                                string salary = Utils.GetValidInput("Insert new salary ", input => !string.IsNullOrEmpty(input) && decimal.TryParse(input, out _) && decimal.Parse(input) > 0);
                                emp.Salary = decimal.Parse(salary);
                                break;

                            default:
                                Console.WriteLine("Invalid input.");
                                break;
                        }

                        Console.WriteLine(separator);
                        Console.WriteLine("Do you want to change something else about this employee? (y / n)");
                        char answerFinal = Console.ReadKey().KeyChar;

                        if (answerFinal == 'n')
                        {  
                            query = 
                            """
                            UPDATE Employee 
                            SET 
                                FullName = @FullName,
                                Gender = @Gender,
                                Address = @Address,
                                Email = @Email,
                                Phone = @Phone,
                                BirthYear = @BirthYear,
                                IsFullTime = @IsFullTime,
                                MaritalStatus = @MaritalStatus,
                                Role = @Role,
                                Faculty = @Faculty,
                                HiringYear = @HiringYear,
                                Salary = @Salary
                                
                            FROM Employee
                            WHERE Employee.Id = @Id
                            """;
                            using SqlCommand command3 = new (query, connection);
                            command3.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = emp.Id;
                            command3.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 50)).Value = emp.FullName;
                            command3.Parameters.Add(new SqlParameter("@Gender", SqlDbType.NChar, 1)).Value = emp.Gender;
                            command3.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 50)).Value = emp.Address;
                            command3.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50)).Value = emp.Email;
                            command3.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NChar, 13)).Value = emp.Phone;
                            command3.Parameters.Add(new SqlParameter("@BirthYear", SqlDbType.Date)).Value = emp.BirthYear;
                            command3.Parameters.Add(new SqlParameter("@IsFullTime", SqlDbType.Bit)).Value = emp.IsFullTime;
                            command3.Parameters.Add(new SqlParameter("@MaritalStatus", SqlDbType.SmallInt)).Value = emp.MaritalStatus;
                            command3.Parameters.Add(new SqlParameter("@Role", SqlDbType.SmallInt)).Value = emp.Role;
                            command3.Parameters.Add(new SqlParameter("@Faculty", SqlDbType.SmallInt)).Value =  emp.Faculty;
                            command3.Parameters.Add(new SqlParameter("@HiringYear", SqlDbType.Date)).Value = emp.HiringYear;
                            command3.Parameters.Add(new SqlParameter("@Salary", SqlDbType.Decimal)).Value = emp.Salary;
                            command3.ExecuteNonQuery();
                           
                            Console.WriteLine("Update saved successfully");
                            return;
                        }
                        

                    } while (doWhile);
          
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.SaveExeptionInDb(ex);
            }
        }       
  
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
        /// <summary>
        /// Deleted an Employee object from the DB by finding it, and removing it from its DB table.
        /// </summary>
        public static void DeleteEmployee()
        {
            try
            {
                string? id = Utils.GetValidId();
                
                using SqlConnection connection = new (ConnectionString);
                connection.Open();
                
                string query = "SELECT Count(Id), FullName FROM Employee WHERE Id = @Id AND Deleted = 0 GROUP BY FullName";
                
                using SqlCommand command = new (query, connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = Guid.Parse(id);
                
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                if(!reader.HasRows || reader.GetInt32(0) == 0) {
                    Console.WriteLine("Employee not found");
                    return;
                }
                    string FullName = reader.GetString(1);
                    reader.Close();

                    Console.WriteLine($"Are you sure you want to Deleted the employee: {FullName} ? (y / n)");
                    char answer = Console.ReadKey().KeyChar;
                    Console.WriteLine();

                    if (answer == 'y')
                    {
                        query = "UPDATE Employee SET Deleted = 1 WHERE Id = @Id";
                        using SqlCommand command2 = new (query, connection);
                        command2.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = Guid.Parse(id);
                        command2.ExecuteNonQuery();
                        Console.WriteLine("Employee removed successfully");
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