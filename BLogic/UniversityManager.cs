/*
*    THIS FILE IS COAUTHORED BY:
*    - Francesco Piersanti
        all the functions about import / export and input validation
*    - Delia Ricca
        all the CRUD functions about the Exam object
*    - Andreea Cojocaru
        all the CRUD functions about the Student object
*    - Mattia Andrea Tarantino
        all the CRUD functions about the Professor object
*/

using System.Configuration;
using System.Globalization;
using System.Text.Json;
using University.DataModel;
namespace University.BLogic
{
    public class UniversityManager
    {
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
        readonly string filePath = Convert.ToString(ConfigurationManager.AppSettings["FileCourses"])!;

        #region Create
        /// <summary>
        /// Inserts a new Employee object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        public static void InsertEmployee()
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

                var updatedList = ImportFromJson<Employee>();
                updatedList.Add(employee);
                ExportJson(updatedList);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }

        
        /// <summary>
        /// Inserts a new Student object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        public static void InsertStudent()
        {
            try{
                string? fullName = Utils.GetValidInput("Enter Student Full Name: ", input => !string.IsNullOrEmpty(input));
        
                string gender = Utils.GetGender();
            
                string address = Utils.GetValidInput("Enter Student Address: ", input => !string.IsNullOrEmpty(input));
            
                string email = Utils.GetValidInput("Enter Student Email: ", input => !string.IsNullOrEmpty(input));
            
                string phone = Utils.GetValidInput("Enter Student Phone Number: ", input => !string.IsNullOrEmpty(input));
            
                string birthYearString = Utils.GetValidInput("Insert new birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
            
                string isFullTime = Utils.GetValidInput("Enter Full Time (y / n) ", input => !string.IsNullOrEmpty(input) && (input?.ToUpper() == "Y" || input?.ToUpper() == "N"));
            
                string maritalStatus = Utils.GetMaritialStatus();
            
                string matricola = Utils.GetValidInput("Enter Student Matricola: ", input => !string.IsNullOrEmpty(input) && input.Length == 7);
            
                string registrationYear = Utils.GetValidInput("Insert new registration year (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
            
                string degree = Utils.GetDegree();
            
                string isee = Utils.GetValidInput("Insert new ISEE ", input => decimal.TryParse(input, out _) && Convert.ToDecimal(input) > 0);
            
                Student student = new()
                {
                    FullName = fullName,
                    Gender = gender,
                    Address = address,
                    Email = email,
                    Phone = phone,
                    BirthYear = DateTime.Parse(birthYearString),
                    IsFullTime = isFullTime.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                    MaritalStatus = (Status)Enum.Parse(typeof(Status), maritalStatus!.ToUpper()), 
                    Matricola = matricola,
                    RegistrationYear = DateTime.Parse(registrationYear),
                    Degree = (Degrees)Enum.Parse(typeof(Degrees), degree!.ToUpper()),
                    ISEE = decimal.Parse(isee)
            
                };

                var updatedList = ImportFromJson<Student>();
                updatedList.Add(student);
                ExportJson(updatedList);
            }catch(Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// Inserts a new Professor object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        public static void InsertProfessor()
        {
         
            try{
                string fullName = Utils.GetValidInput("Enter Professor Full Name: ", input => !string.IsNullOrEmpty(input));
        
                string gender = Utils.GetGender();

                string address = Utils.GetValidInput("Enter Address: ", input => !string.IsNullOrEmpty(input));

                string email = Utils.GetValidInput("Enter Email: ", input => !string.IsNullOrEmpty(input));

                string phone = Utils.GetValidInput("Enter Phone Number (10 digits): ", input => !string.IsNullOrEmpty(input) && input.Length == 10);

                string birthYearString = Utils.GetValidInput("Insert birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                DateTime birthYear = DateTime.Parse(birthYearString);

            
                Console.WriteLine($"Is the professor full-time? (y / n)");
                char answer = Console.ReadKey().KeyChar;
                while (answer != 'y' && answer != 'n'){
                    Console.WriteLine("Invalid input - please enter 'y' or 'n'.");
                    answer = Console.ReadKey().KeyChar;
                }
                
                string maritalStatus = Utils.GetMaritialStatus();
                string faculty = Utils.GetFaculty();
                
                string hiringYearString = Utils.GetValidInput("Insert hiring date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                DateTime hiringYear  = DateTime.Parse(hiringYearString);

                decimal salary = decimal.Parse(Utils.GetValidInput("Enter Salary: ", input => !string.IsNullOrEmpty(input) && decimal.TryParse(input, out _)));

                Professor professore = new()
                {
                    FullName = fullName,
                    Gender = gender,
                    Address = address,
                    Email = email,
                    Phone = phone,
                    BirthYear = birthYear,
                    IsFullTime = answer == 'y',
                    MaritalStatus = (Status)Enum.Parse(typeof(Status), maritalStatus!.ToUpper()),
                    Role = Roles.PROFESSOR,
                    Faculty = (Faculties)int.Parse(faculty),
                    HiringYear = hiringYear,
                    Salary = salary
                };

                List<Professor> professors = ImportFromJson<Professor>();
                professors.Add(professore);
                ExportJson(professors);
            } catch(Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// Inserts a new Exam object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        public static void InsertExam()
        {
            try
            {
                string? name = Utils.GetValidInput("Enter Exam Name: ", input => !string.IsNullOrEmpty(input));
        
                string faculty = Utils.GetFaculty();
        
                string cfu = Utils.GetValidInput("Enter CFU (Credits): ", input => !string.IsNullOrEmpty(input) && int.TryParse(input, out _));
        
                string date = Utils.GetValidInput("Enter Exam Date (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        
                string isOnline = Utils.GetValidInput("Is the Exam Online? (y / n): ", input => !string.IsNullOrEmpty(input) && (input.ToUpper() == "Y" || input.ToUpper() == "N"));
        
                string participants = Utils.GetValidInput("Enter Number of Participants: ", input => !string.IsNullOrEmpty(input) && int.TryParse(input, out _));
        
                string examType = Utils.GetExamType();
        
                string isProjectRequired = Utils.GetValidInput("Is a Project Required? (y / n): ", input => !string.IsNullOrEmpty(input) && (input.ToUpper() == "Y" || input.ToUpper() == "N"));
        
                Exam exam = new()
                {
                    Name = name!,
                    Faculty = (Faculties)int.Parse(faculty),
                    CFU = int.Parse(cfu),
                    Date = DateTime.Parse(date),
                    IsOnline = isOnline.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                    Participants = int.Parse(participants),
                    ExamType = (ExamType)int.Parse(examType),
                    IsProjectRequired = isProjectRequired.Equals("Y", StringComparison.CurrentCultureIgnoreCase)
                };
        
                var updatedList = ImportFromJson<Exam>();
                updatedList.Add(exam);
                ExportJson(updatedList);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Inserts a new Course object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        public static void InsertCourse()
        {
            try{
                string faculty = Utils.GetFaculty();
                string name = Utils.GetValidInput("Enter Course Name: ", input => !string.IsNullOrEmpty(input));
                int cfu = int.Parse(Utils.GetValidInput("Enter CFU (Credits): ", input => !string.IsNullOrEmpty(input) && int.TryParse(input, out _)));    
                Console.WriteLine($"Is the course online? (y / n)");
                char answer = Console.ReadKey().KeyChar;
                while (answer != 'y' && answer != 'n'){
                    Console.WriteLine("Invalid input - please enter 'y' or 'n'.");
                    answer = Console.ReadKey().KeyChar;
                }
                
                string classroom = Utils.GetClassroom();

                int classroomint = int.Parse(classroom);

                Courses course = new()
                {
                    Name = name,
                    Faculty = (Faculties) int.Parse(faculty),
                    CFU = cfu,
                    IsOnline = answer == 'y',
                    Classroom = (Classroom) classroomint,
                };

                List<Courses> courses = ImportFromJson<Courses>();
                courses.Add(course);
                ExportJson(courses); 
            }catch(Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
   
        }

        #endregion 

        #region Read

        /// <summary>
        /// Print all the Employee objects collected in the employee JSON file in readable format.
        /// </summary>
        public static void ReadEmployees()
        {
            // Assuming you have a method to Utils.Get the list of employees
            List<Employee> employees = ImportFromJson<Employee>();

            if (employees == null || employees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                return;
            }

            foreach (var employee in employees)
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

        /// <summary>
        /// Print all the Professor objects collected in the professor JSON file in readable format.
        /// </summary>
        public static void ReadProfessors()
        {
            // Deserialize the list of professors from JSON
            List<Professor> professors = ImportFromJson<Professor>();

            // Check if the list is null or empty
            if (professors == null || professors.Count == 0)
            {
                Console.WriteLine("No professors found.");
                return;
            }

            // Iterate through each professor and print their details
            foreach (var professor in professors)
            {
                Console.WriteLine("Professor Details:");
                Console.WriteLine($"ID: {professor.Id}");
                Console.WriteLine($"Full Name: {professor.FullName}");
                Console.WriteLine($"Gender: {professor.Gender}");
                Console.WriteLine($"Address: {professor.Address}");
                Console.WriteLine($"Email: {professor.Email}");
                Console.WriteLine($"Phone Number: {professor.Phone}");
                Console.WriteLine($"Birth Year: {professor.BirthYear.ToShortDateString()}");
                Console.WriteLine($"Is Full Time: {professor.IsFullTime}");
                Console.WriteLine($"Marital Status: {professor.MaritalStatus}");
                Console.WriteLine($"Role: {professor.Role}");
                Console.WriteLine($"Faculty: {professor.Faculty}");
                Console.WriteLine($"Hiring Year: {professor.HiringYear.ToShortDateString()}");
                Console.WriteLine($"Salary: {professor.Salary:C}");

                // Print Exams
                Console.WriteLine("Exams:");
                if (professor.Exams != null && professor.Exams.Count > 0)
                {
                    foreach (var exam in professor.Exams)
                    {
                        Console.WriteLine($" - {exam.Name} on {exam.Date.ToShortDateString()}");
                    }
                }
                else
                {
                    Console.WriteLine(" No exams assigned.");
                }

                // Print Courses
                Console.WriteLine("Courses:");
                if (professor.Courses != null && professor.Courses.Count > 0)
                {
                    foreach (var course in professor.Courses)
                    {
                        Console.WriteLine($" - {course.Name}");
                    }
                }
                else
                {
                    Console.WriteLine(" No courses assigned.");
                }

                // Print separator
                Console.WriteLine(separator);
            }
        }

        /// <summary>
        /// Print all the Student objects collected in the student JSON file in readable format.
        /// </summary>
        public static void ReadStudents()
        {
            try
            {
                List<Student> students = ImportFromJson<Student>();
        
                if (students.Count == 0)
                {
                    Console.WriteLine("There are no students to show.\n");
                }
                else
                {
                    foreach (Student student in students)
                    {
                        Console.WriteLine("Personal Details");
                        Console.WriteLine($"Fullname: {student.FullName}");
                        Console.WriteLine($"Gender: {student.Gender}");
                        Console.WriteLine($"Address: {student.Address}");
                        Console.WriteLine($"Email: {student.Email}");
                        Console.WriteLine($"Phone: {student.Phone}");
                        Console.WriteLine($"Birth Date: {student.BirthYear}");
                        Console.WriteLine($"Status: {student.MaritalStatus}");
                        Console.WriteLine(separator);
        
                        Console.WriteLine();
        
                        Console.WriteLine("University Data");
                        Console.WriteLine($"ID: {student.Matricola}");
                        Console.WriteLine($"Registration Year: {student.RegistrationYear}");
                        Console.WriteLine($"Faculty: {student.Faculty}");
                        Console.WriteLine($"Degree: {student.Degree}");
                        Console.WriteLine($"Full Time: {student.IsFullTime}");
                        Console.WriteLine(separator);
        
                        Console.WriteLine("Financial Data");
                        Console.WriteLine($"ISEE: {student.ISEE}");
                        Console.WriteLine(separator);
                    }
                }
            }
            catch (Exception  ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        
        }

        /// <summary>
        /// Print all the Exam objects collected in the exam JSON file in readable format.
        /// </summary>
        public static void ReadExams()
        {
            try
            {
                List<Exam> exams = ImportFromJson<Exam>();
        
                Console.Clear();
        
                if (exams.Count == 0)
                {
                    Console.WriteLine("No exams found.");
                    return; ;
                }
                else
                {
                    foreach (Exam exam in exams)
                    {
                        Console.WriteLine($"Id: {exam.Id}\n");
                        Console.WriteLine($"Faculty:  {exam.Faculty}\n");
                        Console.WriteLine($"Exam Name: {exam.Name}\n");
                        Console.WriteLine($"CFU: {exam.CFU}\n");
                        Console.WriteLine($"Exam Date: {exam.Date}\n");
        
                        Console.WriteLine(exam.IsOnline ? "The exam is online" : "The exam takes place on site");
        
                        Console.WriteLine($"Participants: {exam.Participants}\n");
        
                        Console.WriteLine("Exam Type:");
                        switch (exam.ExamType)
                        {
                            case ExamType.WRITTEN:
                                Console.WriteLine("The exam is written.\n");
                                break;
                            case ExamType.WRITTEN_AND_ORAL:
                                Console.WriteLine("The exam is written and oral.\n");
                                break;
                            case ExamType.ORAL:
                                Console.WriteLine("The exam is oral.\n");
                                break;
                            default:
                                break;
                        }
        
                        Console.WriteLine(exam.IsProjectRequired ? "The exam requires a project\n" : "The exam doesn't require a project\n");
        
                        Console.WriteLine(ConfigurationManager.AppSettings["SeparationLine"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Update a Professor object from the system by finding it, collecting user input and saving it to its JSON file.
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
                    7.  Change Marital Status
                    8.  Change Full-Time Status
                    9. Change Hiring Year
                    10. Change Faculty
                    11. Change Salary
                    """;
                
                List<Professor> Professors = ImportFromJson<Professor>();
        
                if (Professors?.Count == 0)
                {
                    Console.WriteLine("There are no Professors saved.\n");
                    return;
                }
                else
                {
                    string? id = Utils.GetValidId();
        
                    do
                    {
                        Professor? prof = Professors!.Find(prof => prof.Id == Guid.Parse(id));
                        
                        if(prof != null){
                            Console.WriteLine($"Changing Professor {prof.FullName} infos");
                            Console.WriteLine(separator);
                            Console.WriteLine(prompt);
                            Console.WriteLine("Which info do you want to change? (press a number)");
            
                            n = Console.ReadKey().KeyChar.ToString();
            
                            switch (n){
                                case "1":
                                    Console.WriteLine("Insert new name");
                                    prof.FullName = Utils.GetValidInput("Insert new name ", input => !string.IsNullOrEmpty(input));
                                    break;

                                case "2":
                                    prof.Gender = Utils.GetGender(); 
                                    break;

                                case "3":
                                    prof.Address = Utils.GetValidInput("Insert new address ", input => !string.IsNullOrEmpty(input));
                                    break;

                                case "4":
                                    prof.Email = Utils.GetValidInput("Insert new email ", input => !string.IsNullOrEmpty(input));
                                    break;

                                case "5":
                                    prof.Phone = Utils.GetValidInput("Insert new phone (10 digits)", input => !string.IsNullOrEmpty(input) && input.Length == 10);
                                    break;

                                case "6":
                                    string birthYearString = Utils.GetValidInput("Insert new birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                                    prof.BirthYear = DateTime.Parse(birthYearString);
                                    break;

                                case "7":
                                    Console.WriteLine($"Change Full-Time Status: from {prof.IsFullTime} to {!prof.IsFullTime}? (y / n)");
                                    char answer = Console.ReadKey().KeyChar;
                                    if (answer == 'y')
                                    {
                                        prof.IsFullTime = !prof.IsFullTime;
                                    }
                                    break;

                                case "8":
                                    DateTime HiringYear = DateTime.Parse(Utils.GetValidInput("Insert new hiring year (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)));
                                    prof.HiringYear = HiringYear;
                                    break;

                                case "9":
                                    string faculty = Utils.GetFaculty();
                                    prof.Faculty = (Faculties) int.Parse(faculty); 
                                    
                                    break;

                                case "10":
                                    Console.WriteLine("Insert new salary");
                                    decimal salary = Convert.ToDecimal(Console.ReadLine());
                                    while (salary <= 0)
                                    {
                                        Console.WriteLine("Invalid input - the salary must be a positive number.");
                                        salary = Convert.ToDecimal(Console.ReadLine());
                                    }
                                    prof.Salary = salary;
                                    break;

                                default:
                                    Console.WriteLine("Invalid input.");
                                    break;
                            }
            
                            Console.WriteLine(separator);
                            Console.WriteLine("Do you want to change something else about this Professor? (y / n)");
                            char answerFinal = Console.ReadKey().KeyChar;
            
                            if (answerFinal  == 'n')
                            {
                                ExportJson(Professors!);
                                Console.WriteLine("Update saved successfully");
                                doWhile = false;
                            }                            
                        } else {
                            Console.WriteLine("Professor not found");
                            Console.ReadLine();
                            return;
                        }

                    } while (doWhile);
                }
        
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
     
        /// <summary>
        /// Update a Student object from the system by finding it, collecting user input and saving it to its JSON file.
        /// </summary>
        public static void UpdateStudent()
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
                    9.  Change Registration Year
                    10. Change Faculty
                    11. Change Degree
                    12. Change ISEE
                    """;
                
                List<Student> students = ImportFromJson<Student>();

                if (students?.Count == 0)
                {
                    Console.WriteLine("There are no students saved.\n");
                    return;
                }
                else
                {
                    string? matricola = Utils.GetValidInput("Insert the Matricola of the student ", input => !string.IsNullOrEmpty(input) && input.Length == 7);
                    Student? stud = students!.Find(stud => stud.Matricola == matricola);

                    do
                    {
                        if (stud != null)
                        {
                            Console.WriteLine($"Changing student {stud?.Matricola} infos");
                            Console.WriteLine(separator);
                            Console.WriteLine(prompt);
                            Console.WriteLine("Which info do you want to change? (press a number)");

                            n = Console.ReadKey().KeyChar.ToString();

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

                            Console.WriteLine(separator);
                            Console.WriteLine("Do you want to change something else about this student? (y / n)");
                            char answerFinal = Console.ReadKey().KeyChar;

                            if (answerFinal == 'n')
                            {
                                ExportJson(students!);
                                Console.WriteLine("Update saved successfully");
                                doWhile = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Student not found");
                            Console.ReadLine();
                            return;
                        }

                    } while (doWhile);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an Employee object from the system by finding it, collecting user input and saving it to its JSON file.
        /// </summary>
        public static void UpdateEmployee()
        {
            try
            {
                List<Employee> employees = ImportFromJson<Employee>();
                if (employees.Count == 0)
                {
                    Console.WriteLine("There are no employees saved.\n");
                }
                else
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
                    Employee? emp = employees.Find(emp => emp.Id == Guid.Parse(id!));
                    do
                    {
                        if (emp != null){
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
                                ExportJson(employees);
                                Console.WriteLine("Update saved successfully");
                                return;
                            }
                        } else {
                            Console.WriteLine("Employee not found");
                            Console.ReadLine();
                            return;
                        }

                    } while (doWhile);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n{ex.StackTrace}");
            }
        }       
        
        /// <summary>
        /// Update an Exam object from the system by finding it, collecting user input and saving it to its JSON file.
        /// </summary>
        public static void UpdateExams()
        {
            try
            {
                List<Exam>? exams = ImportFromJson<Exam>();

                if (exams.Count == 0)
                {
                    Console.WriteLine("There are no exams saved.\n");
                    return;
                }
                else
                {
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
                    string? id = Utils.GetValidId();
                    Exam? exam = exams.Find(exam => exam.Id == Guid.Parse(id));

                    do
                    {
                        if (exam != null)
                        {
                            Console.WriteLine(prompt);

                            string? preChoice = Utils.GetValidInput("Enter a number ", input => !string.IsNullOrEmpty(input));
                            int choice = int.Parse(preChoice);

                            switch (choice)
                            {
                                case 1:
                                    exam.Faculty = (Faculties)int.Parse(Utils.GetFaculty());
                                    break;

                                case 2:
                                    exam.Name = Utils.GetValidInput("Enter the new exam name: ", input => !string.IsNullOrEmpty(input));
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

                            Console.WriteLine("Do you want to change something else about this exam? (y / n)");
                            char answerFinal = Console.ReadKey().KeyChar;

                            if (answerFinal == 'n')
                            {
                                ExportJson(exams!);
                                Console.WriteLine("Update saved successfully");
                                doWhile = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Exam not found.");
                            Console.ReadLine();
                            return;;
                        }

                    } while (doWhile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete a Professor object from the system by finding it, and removing it to its JSON file.
        /// </summary>
        public static void DeleteProfessor()
        {
            bool loop = true;
            try
            {   
                List<Professor> professors = ImportFromJson<Professor>(); // Importa i dati esistenti

                Console.WriteLine(professors.Count);

                while (loop)
                {
        
                    if (professors.Count == 0)
                    {
                        Console.WriteLine("There are no professor saved");
                        Console.ReadLine();
                        return;
                        
                    }
                    else
                    {
                        string? id = Utils.GetValidId();
                        
                        Professor? prof = professors.Find(prof => prof.Id == Guid.Parse(id));
                        
                        if(prof != null){

                            Console.WriteLine($"Are you sure you want to delete the professor: {prof!.FullName} ? (y / n)");
                            char answer = Console.ReadKey().KeyChar;
                            Console.WriteLine();

                            if (answer == 'y')
                            {
                                professors.Remove(prof);
                                ExportJson(professors);
                                Console.WriteLine("professor removed successfully");
                                loop = false;
                            }

                            Console.WriteLine("professor not found, try again");
                        }

                    }
                }
            }
            catch ( Exception ex ) 
            { 
                    Console.WriteLine($"Error: {ex.Message}");
            }
        }
       
        /// <summary>
        /// Delete a Student object from the system by finding it, and removing it to its JSON file.
        /// </summary>
        public static void DeleteStudent()
        {
            bool loop = true;
            try
            {   
                List<Student> students = ImportFromJson<Student>();
                while (loop)
                {
        
                    if (students.Count == 0)
                    {
                        Console.WriteLine("There are no students saved");
                    }
                    else
                    {
                        string? matricola = Utils.GetValidInput("Insert the Matricola of the student ", input => !string.IsNullOrEmpty(input) && input.Length == 7);
                        
                        Student? stud = students.Find(stud => stud.Matricola == matricola);
                        if(stud != null){

                            Console.WriteLine($"Are you sure you want to delete the student: {stud!.FullName} ? (y / n)");
                            char answer = Console.ReadKey().KeyChar;
                            Console.WriteLine();

                            if (answer == 'y')
                            {
                                students.Remove(stud);
                                ExportJson(students);
                                Console.WriteLine("Student removed successfully");
                                return;
                            }

                            Console.WriteLine("Student not found, try again");
                        }

                    }
                }
            }
            catch ( Exception ex ) 
            { 
                    Console.WriteLine($"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Delete an Employee object from the system by finding it, and removing it to its JSON file.
        /// </summary>
        public static void DeleteEmployee()
        {
            bool loop = true;
            try
            {
                List<Employee> employees = ImportFromJson<Employee>();
               
                while (loop)
                {
                   
                    if (employees.Count == 0)
                    {
                        Console.WriteLine("There are no employee saved");
                    }
                    else
                    {
                       
                        string id = Utils.GetValidInput("Insert the ID of the employee", input => !string.IsNullOrEmpty(input) && Guid.TryParse(input, out _));
                        Employee? emp = employees.Find(emp => emp.Id.ToString() == id);

                        if(emp != null){
                            Console.WriteLine($"Are you sure you want to delete the employee: {emp.FullName} ? (y / n)");
                            char answer = Console.ReadKey().KeyChar;
                            Console.WriteLine();

                            if (answer == 'y')
                            {
                                employees.Remove(emp);
                                ExportJson(employees);
                                Console.WriteLine("Employee removed successfully");
                                loop = false;
                            }
                        }
                        Console.WriteLine("Employee not found please, try again");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Delete an Exam object from the system by finding it, and removing it to its JSON file.
        /// </summary>
        public static void DeleteExam()
        {
            bool loop = true;
            try
            {   
                List<Exam> Exams = ImportFromJson<Exam>(); // Importa i dati esistenti
                while (loop)
                {
        
                    if (Exams.Count == 0)
                    {
                        Console.WriteLine("There are no Exam saved");
                    }
                    else
                    {
                        string? id = Utils.GetValidId();
                        Exam? exam = Exams.Find(exam => exam.Id == Guid.Parse(id));
                        if(exam != null){

                            Console.WriteLine($"Are you sure you want to delete the Exam: {exam!.Id} ? (y / n)");
                            char answer = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            
                            if (answer == 'y')
                            {
                                Exams.Remove(exam);
                                ExportJson(Exams);
                                Console.WriteLine("Exam removed successfully");
                                return;
                            }

                            Console.WriteLine("Exam not found, try again");
                        }

                    }
                }
            }
            catch ( Exception ex ) 
            { 
                    Console.WriteLine($"Error: {ex.Message}");
            }
        }     
        #endregion

        /// <summary>
        /// Imports a list of objects from a JSON file based on the specified type parameter.
        /// </summary>
        /// <typeparam name="T">The type of objects to import.</typeparam>
        /// <returns>
        /// A list of objects of type <typeparamref name="T"/> imported from the corresponding JSON file.
        /// Returns an empty list if an error occurs or the file is empty.
        /// </returns>
        
        internal static List<T> ImportFromJson<T>()
        {
            try
            {
                string Json = string.Empty;

                if (typeof(T) == typeof(Student))
                {
                    Json = File.ReadAllText(ConfigurationManager.AppSettings["FileStudentsJson"]!);
                }
                else if (typeof(T) == typeof(Professor))
                {
                    Json = File.ReadAllText(ConfigurationManager.AppSettings["FileProfessorsJson"]!);
                }
                else if (typeof(T) == typeof(Exam))
                {
                    Json = File.ReadAllText(ConfigurationManager.AppSettings["FileExamsJson"]!);
                }
                else if (typeof(T) == typeof(Courses))
                {
                    Json = File.ReadAllText(ConfigurationManager.AppSettings["FileCoursesJson"]!);
                }
                else if (typeof(T) == typeof(Faculty))
                {
                    Json = File.ReadAllText(ConfigurationManager.AppSettings["FileFacultiesJson"]!);
                } 
                else if(typeof(T) == typeof(Employee))
                {
                    Json = File.ReadAllText(ConfigurationManager.AppSettings["FileEmployeeJson"]!);
                }

                return JsonSerializer.Deserialize<List<T>>(Json) ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                return [];
            }
        }

        /// <summary>
        /// Exports a list of objects to a JSON file based on the specified type parameter.
        /// </summary>
        /// <typeparam name="T">The type of objects to import.</typeparam>
        /// <returns></returns>
        
        internal static bool ExportJson<T>(List<T> list)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
    
                List<T> dataToAdd = list.OfType<T>().ToList();
                string json = JsonSerializer.Serialize(dataToAdd, options);

                if(typeof(T) == typeof(Student)){
                    File.WriteAllText(ConfigurationManager.AppSettings["FileStudentsJson"]!, json);
                    
                } else if (typeof(T) == typeof(Professor))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileProfessorsJson"]!, json);
                }
                else if(typeof(T) == typeof(Exam))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileExamsJson"]!, json);
                }
                else if (typeof(T) == typeof(Courses))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileCoursesJson"]!, json);
                }
                else if (typeof(T) == typeof(Faculty))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileFacultiesJson"]!, json);
                } 
                else if (typeof(T) == typeof(Employee))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileEmployeeJson"]!, json);
                }

            return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Error: {ex.StackTrace}");
                return false;
            }
        }

    }
}