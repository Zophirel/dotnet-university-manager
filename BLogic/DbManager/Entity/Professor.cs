using System.Configuration;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using University.DataModel;

namespace University.BLogic.DbManager.Entity {
    internal class ProfessorLogic {
        private readonly static string ConnectionString = ConfigurationManager.AppSettings["DbConnectionString"]!;
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
       
        private readonly static List<Professor> Professors = DbUtils.GetAll<Professor>();
        
        #region Create

        /// <summary>
        /// Inserts a new Professor object into the DB by collecting user input and saving it to its DB table.
        /// </summary>
        public static void CreateProfessor()
        {
            try
            {
                string? fullName = Utils.GetValidInput("Enter Professor Full Name: ", input => !string.IsNullOrEmpty(input));

                string? gender = Utils.GetGender();

                string? address = Utils.GetValidInput("Enter Address: ", input => !string.IsNullOrEmpty(input));

                string? email = Utils.GetValidInput("Enter Email: ", input => !string.IsNullOrEmpty(input));

                string? phone = Utils.GetValidInput("Enter Phone Number: ", input => !string.IsNullOrEmpty(input));

                string birthYearString = Utils.GetValidInput("Enter Birth Year (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));

                string isFullTimeString = Utils.GetValidInput("Is the Professor full-time? (y / n): ", input => !string.IsNullOrEmpty(input) && (input.ToUpper() == "Y" || input.ToUpper() == "N")); 
                 
                string maritalStatus = Utils.GetMaritialStatus();

                string role = Utils.GetRole();
                
                string faculty = Utils.GetFaculty();
            
                string hiringYearString = Utils.GetValidInput("Enter Hiring Day (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                
                string salaryString = Utils.GetValidInput("Enter Salary: ", input => !string.IsNullOrEmpty(input) && decimal.TryParse(input, out _));  

                int roleint = int.Parse(role);
                int statusint = int.Parse(maritalStatus);

                Professor employee = new()
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

        public static void ReadProfessors()
        {
            try
            {
                foreach (Professor professor in Professors)
                {
                    Console.WriteLine("Professor Details:");
                    Console.WriteLine($"ID: {professor.Id}");
                    Console.WriteLine($"Full Name: {professor.FullName}");
                    Console.WriteLine($"Gender: {professor.Gender}");
                    Console.WriteLine($"Address: {professor.Address}");
                    Console.WriteLine($"Email: {professor.Email}");
                    Console.WriteLine($"Phone Number: {professor.Phone}");
                    Console.WriteLine($"Birth Year: {professor.BirthYear}");
                    Console.WriteLine($"Is Full Time: {professor.IsFullTime}");
                    Console.WriteLine($"Marital Status: {professor.MaritalStatus}");
                    Console.WriteLine($"Role: {professor.Role}");
                    Console.WriteLine($"Faculty: {professor.Faculty}");
                    Console.WriteLine($"Hiring Year: {professor.HiringYear}");
                    Console.WriteLine($"Salary: {professor.Salary}");
                    Console.WriteLine(separator);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }
       
        public static void ReadProfessor(Guid id)
        {
            try
            {
                Professor professor = Professors.First(p => p.Id == id);
                Console.WriteLine("Professor Details:");
                Console.WriteLine($"ID: {professor.Id}");
                Console.WriteLine($"Full Name: {professor.FullName}");
                Console.WriteLine($"Gender: {professor.Gender}");
                Console.WriteLine($"Address: {professor.Address}");
                Console.WriteLine($"Email: {professor.Email}");
                Console.WriteLine($"Phone Number: {professor.Phone}");
                Console.WriteLine($"Birth Year: {professor.BirthYear}");
                Console.WriteLine($"Is Full Time: {professor.IsFullTime}");
                Console.WriteLine($"Marital Status: {professor.MaritalStatus}");
                Console.WriteLine($"Role: {professor.Role}");
                Console.WriteLine($"Faculty: {professor.Faculty}");
                Console.WriteLine($"Hiring Year: {professor.HiringYear}");
                Console.WriteLine($"Salary: {professor.Salary}");
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
        
        /// <summary>
        /// Update an Employee object from the DB by finding it, collecting user input and saving it to its DB record.
        /// </summary>
        public static void UpdateProfessor()
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
                
                //Check if Professor existss
                string query = "SELECT Count(Id) FROM Professor WHERE Id = @Id AND Deleted = 0";
                
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
  
        #endregion

        #region Deleted

        public static void DeleteProfessor()
        {
            try
            {
                string? id = Utils.GetValidId();
                
                using SqlConnection connection = new (ConnectionString);
                connection.Open();
                
                string query = "SELECT Count(Id), FullName FROM Employee WHERE Id = @Id AND Deleted = 0 AND Role = 5 GROUP BY FullName";
                
                using SqlCommand command = new (query, connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier)).Value = Guid.Parse(id);
                
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                if(!reader.HasRows || reader.GetInt32(0) == 0) {
                    Console.WriteLine("Professor not found");
                    return;
                }
                    string FullName = reader.GetString(1);
                    reader.Close();

                    Console.WriteLine($"Are you sure you want to Deleted the Professor: {FullName} ? (y / n)");
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