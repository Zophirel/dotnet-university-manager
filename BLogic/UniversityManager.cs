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

        // prompt testo del sottomenu
        // lambda func - controllo all'interno del parametro del funzione quando la chiami
        // 

        /// <summary>
        /// Prompts the user a custom input and validates it using the provided validator function.
        /// </summary>
        /// <param name="prompt">The prompt message to display to the user.</param>
        /// <param name="validator">A function to validate the user's input.</param>
        /// <returns>A valid input string.</returns>
        private static string GetValidInput(string prompt, Func<string?, bool> validator)
        {
            try
            {
                string? input;
                do
                {
                    Console.WriteLine(prompt);
                    input = Console.ReadLine();
                    if (!validator(input))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input non valido, riprova.");
                        Console.ResetColor();
                    }
                } while (!validator(input));
                return input!;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Check the user input to match the Faculties enum.
        /// </summary>
        /// <returns>A string representing the integer that rapresent the correct Faculties enum.</returns>
        private static string GetFaculty(){
            string prompt =
                """
                Select Faculty:
                1 - COMPUTER_SCIENCE
                2 - BUSINESS_AND_MANAGEMENT,
                3 - MATHEMATICS,
                4 - PSYCHOLOGY,
                5 - LAW,
                6 - FASHION_DESIGN,
                7 - NURSING,
                8 - LANGUAGES,
                9 - BIOLOGY
                """;

            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 10);
        }
        
        /// <summary>
        /// Check the user input to match the Roles enum.
        /// </summary>
        /// <returns>A string representing the integer that rapresent the correct Roles enum.</returns>
        private static string GetRole(){
            string prompt = """
                Enter Role:
                1 - TECHNICIAN,
                2 - SECRETARY,
                3 - CLEANING_STAFF,
                4 - RECTOR,
                """;

            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 5);
        }
    
        /// <summary>
        /// Check the user input to match the Status enum.
        /// </summary>
        /// <returns>A string representing the integer that rapresent the correct Status enum.</returns>
        private static string GetMaritialStatus(){
            string prompt = """
                Enter Marital Status:
                1 - SINGLE,
                2 - MARRIED,
                3 - DIVORCED,
                4 - WIDOWED,
                """;
            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 5);
        }
        /// <summary>
        /// Check the user input to match the Degree enum.
        /// </summary>
        /// <returns>A string representing the integer that rapresent the correct Degree enum.</returns>
        private static string GetDegree(){
            string prompt = """
                Enter Degree:
                1 - BACHELOR,
                2 - MASTER,
                3 - PHD,
                4 - FIVE,
                """;
            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 5);
        }

        /// <summary>
        /// Check the user input to match the ExamType enum.
        /// </summary>
        /// <returns>A string representing the integer that rapresent the correct ExamType enum.</returns>
        private static string GetExamType(){
            string prompt = """
                Enter Exam Type:
                1 - WRITTEN,
                2 - ORAL,
                3 - WRITTEN_AND_ORAL,
                """;
            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 4);
        }   
        
        /// <summary>
        /// Check the user input to match the Classroom enum.
        /// </summary>
        /// <returns>A string representing the integer that rapresent the correct Classroom enum.</returns>
        private static string GetClassroom(){
            string prompt = """
                Select Classroom:
                1 - A,
                2 - B,
                3 - C,
                4 - D,
                5 - E,
                6 - F,
                7 - LAB_1,
                8 - LAB_2,
                9 - LAB_3
                """;

            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 10);
        }

        private static string GetGender() => GetValidInput("Enter Gender (Male / Female / X): ", input => string.IsNullOrEmpty(input) && (input?.ToUpper() == "MALE" || input?.ToUpper() == "FEMALE" || input?.ToUpper() == "X"));
        private static string GetValidId() => GetValidInput($"Enter the ID: ", input => !string.IsNullOrEmpty(input) && Guid.TryParse(input, out _));

        #region Create
        public void InsertUniversity()
        {
            try
            {
                Console.WriteLine("Insert University Name.");
                string? name = Console.ReadLine();

                Console.WriteLine("Insert University Address.");
                string? address = Console.ReadLine();

                UniModel university = new(){
                    Name = name!,
                    Address = address!,
                };

                string fileName = Convert.ToString(ConfigurationManager.AppSettings["FileUniversityJson"]);
                //ExportJson<UniModel>(fileName);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Inserts a new Employee object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        public static void InsertEmployee()
        {
            try
            {
                
                string? fullName = GetValidInput("Enter Employee Full Name: ", input => string.IsNullOrEmpty(input));

                string? gender = GetGender();

                string? address = GetValidInput("Enter Address: ", input => !string.IsNullOrEmpty(input));

                string? email = GetValidInput("Enter Email: ", input => !string.IsNullOrEmpty(input));

                string? phone = GetValidInput("Enter Phone Number: ", input => !string.IsNullOrEmpty(input));

                string birthYearString = GetValidInput("Enter Birth Year (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));

                string isFullTimeString = GetValidInput("Is the Employee full-time? (true/false): ", input => !string.IsNullOrEmpty(input) && bool.TryParse(input, out _));
                 
                string maritalStatus = GetMaritialStatus();

                string role = GetRole();
                
                string faculty = GetFaculty();
            
                string hiringYearString = GetValidInput("Enter Hiring Day (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                
                string salaryString = GetValidInput("Enter Salary: ", input => !string.IsNullOrEmpty(input) && decimal.TryParse(input, out _));  

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
                    IsFullTime = bool.Parse(isFullTimeString),
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
            }
        }

        
        /// <summary>
        /// Inserts a new Student object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        public static void InsertStudent()
        {
        
            string? fullName = GetValidInput("Enter Student Full Name: ", input => !string.IsNullOrEmpty(input));
        
            string gender = GetGender();
        
            string address = GetValidInput("Enter Student Address: ", input => !string.IsNullOrEmpty(input));
        
            string email = GetValidInput("Enter Student Email: ", input => !string.IsNullOrEmpty(input));
        
            string phone = GetValidInput("Enter Student Phone Number: ", input => !string.IsNullOrEmpty(input));
        
            string birthYearString = GetValidInput("Insert new birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        
            string isFullTime = GetValidInput("Enter Full Time (true/false) ", input => string.IsNullOrEmpty(input) && (input?.ToUpper() == "TRUE" || input?.ToUpper() == "FALSE"));
        
            string maritalStatus = GetMaritialStatus();
        
            string matricola = GetValidInput("Enter Student Matricola: ", input => !string.IsNullOrEmpty(input) && input.Length == 7);
        
            string registrationYear = GetValidInput("Insert new registration year (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        
            string degree = GetDegree();
        
            string isee = GetValidInput("Insert new ISEE ", input => Decimal.TryParse(input, out _) && Convert.ToDecimal(input) > 0);
        
            Student student = new()
            {
                FullName = fullName,
                Gender = gender,
                Address = address,
                Email = email,
                Phone = phone,
                BirthYear = DateTime.Parse(birthYearString),
                IsFullTime = Boolean.Parse(isFullTime),
                MaritalStatus = (Status)Enum.Parse(typeof(BLogic.Status), maritalStatus!.ToUpper()), 
                Matricola = matricola,
                RegistrationYear = DateTime.Parse(registrationYear),
                Degree = (Degrees)Enum.Parse(typeof(BLogic.Degrees), degree!.ToUpper()),
                ISEE = Decimal.Parse(isee)
        
            };
        }
        /// <summary>
        /// Inserts a new Professor object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        static void InsertProfessor()
        {
            Console.Write("Enter Professor Full Name: ");
            string fullName = Console.ReadLine();

            Console.Write("Enter Gender (Male/Female): ");
            string gender = Console.ReadLine();

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Birth Year (YYYY-MM-DD): ");
            DateTime birthYear = DateTime.Parse(Console.ReadLine());

            Console.Write("Is the professor full-time? (true/false): ");
            bool isFullTime = bool.Parse(Console.ReadLine());

            string maritalStatus = GetMaritialStatus();
            string faculty = GetFaculty();
           
            Console.Write("Enter Hiring Year (YYYY-MM-DD): ");
            DateTime hiringYear = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

           Professor professore = new()
           {
                FullName = fullName,
                Gender = gender,
                Address = address,
                Email = email,
                Phone = phone,
                BirthYear = birthYear,
                IsFullTime = isFullTime,
                MaritalStatus = (Status)Enum.Parse(typeof(Status), maritalStatus!.ToUpper()),
                Role = Roles.PROFESSOR,
                Faculty = (Faculties)int.Parse(faculty),
                HiringYear = hiringYear,
                Salary = salary
            };
        }
        /// <summary>
        ///     Inserts a new Faculty object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        /// <author>
        ///     
        /// </author>
        private static void InsertFaculty()
        {
            {
                string name = GetFaculty();

                Console.Write("Enter Faculty Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Number of Students: ");
                int studentsNumber = int.Parse(Console.ReadLine());

                Console.Write("Enter Number of Labs: ");
                int labsNumber = int.Parse(Console.ReadLine());

                Console.Write("Does the Faculty have a library? (true/false): ");
                bool hasLibrary = bool.Parse(Console.ReadLine());

                Console.Write("Does the Faculty have a canteen? (true/false): ");
                bool hasCanteen = bool.Parse(Console.ReadLine());

                int nameint = int.Parse(name);

                Faculty faculty = new()
                {
                    Name = (Faculties) nameint,
                    Address = address!,
                    StudentsNumber = studentsNumber,
                    LabsNumber = labsNumber,
                    HasLibrary = hasLibrary,
                    HasCanteen = hasCanteen
                };
            }
        }

        /// <summary>
        ///     Inserts a new Exam object into the system by collecting user input and saving it to a JSON file.
        /// </summary>
        /// <author>
        ///     Delia Ricca
        /// </author>
        public static void InsertExam()
        {
            try
            {
                string? name = GetValidInput("Enter Exam Name: ", input => string.IsNullOrEmpty(input));
        
                string faculty = GetFaculty();
        
                string cfu = GetValidInput("Enter CFU (Credits): ", input => !string.IsNullOrEmpty(input) && int.TryParse(input, out _));
        
                string date = GetValidInput("Enter Exam Date (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        
                string isOnline = GetValidInput("Is the Exam Online? (true / false): ", input => !string.IsNullOrEmpty(input) && bool.TryParse(input, out _));
        
                string participants = GetValidInput("Enter Number of Participants: ", input => !string.IsNullOrEmpty(input) && int.TryParse(input, out _));
        
                string examType = GetExamType();
        
                string isProjectRequired = GetValidInput("Is a Project Required? (true/false): ", input => !string.IsNullOrEmpty(input) && bool.TryParse(input, out _));
        
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
        
                var updatedList = ImportFromJson<Exam>();
                updatedList.Add(exam);
                ExportJson(updatedList);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void InsertCourse()
        {
            
            string faculty = GetFaculty();
            Console.Write("Enter Course Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter CFU (Credits): ");
            int cfu = int.Parse(Console.ReadLine());

            Console.Write("Is the Course Online? (true/false): ");
            bool isOnline = bool.Parse(Console.ReadLine());

            string classroom = GetClassroom();

            int classroomint = int.Parse(classroom);

            Courses course = new()
            {
                Name = name,
                Faculty = (Faculties) int.Parse(faculty),
                CFU = cfu,
                IsOnline = isOnline,
                Classroom = (Classroom) classroomint,
            };
        }

        #endregion 

        #region Read

        public static void ReadEmployees()
        {
            // Assuming you have a method to get the list of employees
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

        public static void UpdateProfessors()
        {
            try
            {
                List<Professor> professors = ImportFromJson<Professor>(); // Import existing data

                if (professors == null || professors.Count == 0)
                {
                    Console.WriteLine("There are no professors saved.\n");
                    return;
                }

                
                string id = GetValidId();

                Professor? prof = professors.Find(prof => prof.Id == Guid.Parse(id));
                if (prof == null)
                {
                    Console.WriteLine("Professor not found.");
                    return;
                }

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

                do
                {
                    Console.WriteLine($"Changing professor {prof.Id} information");
                    Console.WriteLine(prompt);
                    n = GetValidInput("Which info do you want to change? (press a number)", input => int.Parse(input!) > 0 && int.Parse(input!) < 12);

                    switch (n)
                    {
                        case "1":
                            Console.WriteLine("Insert new name");
                            prof.FullName = Console.ReadLine();
                            break;

                        case "2":
                            
                            prof.Gender = GetGender(); 
                            break;

                        case "3":
                            Console.WriteLine("Insert new address");
                            prof.Address = Console.ReadLine();
                            break;

                        case "4":
                            Console.WriteLine("Insert new email");
                            prof.Email = Console.ReadLine(); // Assuming email needs to be updated
                            break;

                        case "5":
                            Console.WriteLine("Insert new phone (10 digits)");
                            string? phone = Console.ReadLine();
                            while (phone?.Length != 10 || !long.TryParse(phone, out _))
                            {
                                Console.WriteLine("Invalid input - the phone number must be 10 digits.");
                                phone = Console.ReadLine();
                            }
                            prof.Phone = phone;
                            break;

                        case "6":
                            Console.WriteLine("Insert new birth year (YYYY-MM-DD)");
                            prof.BirthYear = DateTime.Parse(Console.ReadLine());
                            break;

                        case "7":
                            Console.WriteLine($"Change Full-Time Status: from {prof.IsFullTime} to {!prof.IsFullTime}? (yes / no)");
                            string answer = Console.ReadLine().ToLower();
                            if (answer.Equals("yes"))
                            {
                                prof.IsFullTime = !prof.IsFullTime;
                            }
                            break;

                        case "8":
                            Console.WriteLine("Insert new hiring year (YYYY-MM-DD)");
                            prof.HiringYear = DateTime.Parse(Console.ReadLine());
                            break;

                        case "9":
                            string faculty = GetFaculty();
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

                    Console.WriteLine("Do you want to change something else about this professor? (yes / no)");
                    string answerFinal = Console.ReadLine().ToLower();

                    doWhile = answerFinal.Equals("yes");

                } while (doWhile);

                // Save changes
                ExportJson(professors);
                Console.WriteLine("Professor information updated and saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void UpdateStudents()
        {
            try
            {
                List<Student> students = ImportFromJson<Student>();
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
        
                if (students?.Count == 0)
                {
                    Console.WriteLine("There are no students saved.\n");
                    return;
                }
                else
                {
                    string? matricola = GetValidInput("Insert the Matricola of the student ", input => !string.IsNullOrEmpty(input) && input.Length == 7);
        
                    do
                    {
                        Student? stud = students!.Find(stud => stud.Matricola == matricola);
                        if(stud != null){
                            Console.WriteLine($"Changing student {stud?.Matricola} infos");
                            Console.WriteLine(separator);
                            Console.WriteLine(prompt);
                            Console.WriteLine("Which info do you want to change? (press a number)");
            
                            n = Console.ReadKey().KeyChar.ToString();
            
                            switch (n)
                            {
                                case "1":
                                    stud!.FullName = GetValidInput("Insert new name ", input => !string.IsNullOrEmpty(input));
                                    break;
            
                                case "2":
                                    stud!.Gender = GetGender();
                                    break;
            
                                case "3":
                                    stud!.Address = GetValidInput("Insert new address ", input => !string.IsNullOrEmpty(input));
                                    break;
            
                                case "4":
                                    stud!.Email = GetValidInput("Insert new email ", input => !string.IsNullOrEmpty(input));
                                    break;
            
                                case "5":
                                    stud!.Phone = GetValidInput("Insert new phone ", input => !string.IsNullOrEmpty(input));
                                    break;
            
                                case "6":
                                    string birthYearString = GetValidInput("Insert new birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                                    stud!.BirthYear = DateTime.Parse(birthYearString);
                                    break;
            
                                case "7":
                                    string maritalStatus = GetMaritialStatus();
                                    stud!.MaritalStatus = (Status)int.Parse(maritalStatus);
                                    break;
            
                                case "8":
                                    Console.WriteLine($"Change Full Time: from {stud!.IsFullTime} to {!stud.IsFullTime} ? (y / n)");
                                    string answer = Console.ReadKey().KeyChar.ToString();
                                    if (answer.Equals("y"))
                                    {
                                        stud.IsFullTime = !(stud.IsFullTime); 
                                    }
                                    break;
            
                                case "9":
                                    string registrationYear = GetValidInput("Insert new registration year (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                                    stud!.RegistrationYear = DateTime.Parse(registrationYear);
                                    break;
            
                                case "10":
                                    string faculty = GetFaculty();
                                    stud!.Faculty = (Faculties)int.Parse(faculty);
                                    break;
            
                                case "11":
                                    string degree = GetDegree();
                                    stud!.Degree = (Degrees)int.Parse(degree);
                                    break;
            
                                case "12":
                                    string isee = GetValidInput("Insert new ISEE ", input => Decimal.TryParse(input, out _) && Convert.ToDecimal(input)>0);
                                    stud!.ISEE = Convert.ToDecimal(isee);
                                    break;
            
                                default:
                                    Console.WriteLine("Invalid input.");
                                    break;
                            }
            
                            Console.WriteLine(separator);
                            Console.WriteLine("Do you want to change something else about this student? (y / n)");
                            char answerFinal = Console.ReadKey().KeyChar;
            
                            if (answerFinal  == 'n')
                            {
                                ExportJson(students!);
                                Console.WriteLine("Update saved successfully");
                                doWhile = false;
                            }                            
                        } else {
                            Console.WriteLine("Student not found, try again.");
                        }

                    } while (doWhile);
                }
        
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
                
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
                    do
                    {
                        string? id = GetValidId();
                        Employee? emp = employees.Find(emp => emp.Id == Guid.Parse(id!));
                        if (emp != null){
                            Console.WriteLine($"Changing employee {emp.FullName} infos");
                            Console.WriteLine(separator);
                            Console.WriteLine(prompt);
                            Console.WriteLine("Which info do you want to change? (press a number)");
                            n = Console.ReadKey().KeyChar.ToString();

                            switch (n)
                            {
                                case "1":
                                    emp.FullName = GetValidInput("Insert new name ", input => !string.IsNullOrEmpty(input));
                                    break;

                                case "2":
                                    emp.Gender = GetGender();
                                    break;

                                case "3":
                                    emp.Address = GetValidInput("Insert new address ", input => !string.IsNullOrEmpty(input));
                                    break;

                                case "4":
                                    emp.Email = GetValidInput("Insert new email", input => !string.IsNullOrEmpty(input));
                                    break;

                                case "5":
                                    string phone = GetValidInput("Insert new phone ", input => !string.IsNullOrEmpty(input));
                                    break;

                                case "6":
                                    string birthYearString = GetValidInput("Insert new birth date (YYYY-MM-DD)", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                                    emp.BirthYear = DateTime.Parse(birthYearString);
                                    break;

                                case "7":
                                    string maritalStatus = GetMaritialStatus();
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
                                    string faculty = GetFaculty();
                                    emp.Faculty = (Faculties)int.Parse(faculty);
                                    break;

                                case "10":
                                    string role = GetRole();
                                    emp.Role = (Roles)int.Parse(role);
                                    break;

                                case "11":
                                    string newYear = GetValidInput("Insert new hiring year ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                                    emp.HiringYear = DateTime.Parse(newYear);
                                    break;

                                case "12":
                                    string salary = GetValidInput("Insert new salary ", input => !string.IsNullOrEmpty(input) && decimal.TryParse(input, out _) && decimal.Parse(input) > 0);
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
                                doWhile = false;
                            }
                        } else {
                            Console.WriteLine("Employee not found, try again.");
                        }

                    } while (doWhile);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n{ex.StackTrace}");
            }
        }
        
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
                    
                    bool doWhile = true;
                    string n;
                    string prompt = """
                        1 - Faculty
                        2 - Name
                        3 - CFU
                        4 - Date
                        5 - Modality (Online/On-site)
                        6 - Number of participants
                        7 - Exam type (Written, Oral, Written and Oral)
                        8 - Project (is required or not)
                        """;
                    string? id = GetValidId();
                    Exam? exam = exams.Find(exam => exam.Id == Guid.Parse(id));

                    if (exam != null)
                    {
                        Console.WriteLine(prompt);

                        string? preChoice = GetValidInput("Enter a number ", input => !string.IsNullOrEmpty(input));
                        int choice = int.Parse(preChoice);

                        switch (choice)
                        {
                            case 1:
                                exam.Faculty = (Faculties)int.Parse(GetFaculty());
                                break;

                            case 2:
                                Console.WriteLine("You have chosen to upgrade the name of the exam\n");
                                Console.WriteLine($"Currently, the exam name is: {exam.Name},\n");
                                string? newExamName = GetValidInput("Enter the new exam name: ", input => string.IsNullOrEmpty(input));
                                if (newExamName != null)
                                {
                                    exam.Name = newExamName;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid name!");
                                    return;
                                }
                                break;

                            case 3:
                                Console.WriteLine("You have chosen to upgrade the number of CFU!\n");
                                Console.WriteLine($"Currently, the exam number of CFU is: {exam.CFU},\n");
                                string? inputCfu = GetValidInput("Enter the new number of CFU: ", input => string.IsNullOrEmpty(input));
                                int newExamCFU = int.Parse(inputCfu);

                                if (newExamCFU > 0)
                                {
                                    exam.CFU = newExamCFU;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid number of CFU!");
                                    return;
                                }
                                break;

                            case 4:
                                Console.WriteLine("You have chosen to upgrade the date of the exam!\n");
                                Console.WriteLine($"Currently, the exam date is: {exam.Date},\n");

                                string? inputExamDate = GetValidInput("Enter the new date (YYYY-MM-DD): ", input => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                                break;

                            case 5:
                                Console.WriteLine("You have chosen to upgrade the modality of the exam!\n");

                                Console.WriteLine($"Currently, the modality of the exam is: {(exam.IsOnline ? "Online" : "On-Site")}\n!");
                                string? inputModality = GetValidInput("Please enter 1 if you want to change the exam online flag,\n" +
                                    "Enter 0 otherwise!", input => !string.IsNullOrEmpty(input));
                                bool changeModalityExam = bool.Parse(inputModality);

                                if (changeModalityExam)
                                {
                                    exam.IsOnline = !exam.IsOnline;
                                }
                                else
                                {
                                    return;
                                }

                                break;

                            case 6:
                                Console.WriteLine("You have chosen to upgrade the number of participants of the exam!\n");
                                Console.WriteLine($"Currently, the number of participants is: {exam.Participants}\n" +
                                        "Please enter the new number of participants!");
                                string? inputParticipants = GetValidInput("Please enter the new number of participants!: ", input => !string.IsNullOrEmpty(input));
                                int newParticipants = int.Parse(inputParticipants);

                                exam.Participants = newParticipants;
                                break;

                            case 7:
                                exam.ExamType = (ExamType)int.Parse(GetExamType());
                                break;
                            case 8:
                                Console.WriteLine("You have chosen to upgrade the project request for the exam!\n");

                                Console.WriteLine($"Currently, the project is {(exam.IsProjectRequired ? "required" : "not required")}!\n!");
                                string? inputRequired = GetValidInput("Please enter 1 if you want to change the required flag,\n" +
                                    "Enter 0 otherwise!", input => !string.IsNullOrEmpty(input));
                                bool changeModalityProject = bool.Parse(inputRequired);

                                if (changeModalityProject)
                                {
                                    exam.IsProjectRequired = !exam.IsProjectRequired;
                                }
                                else
                                {
                                    return;
                                }

                                break;
                            default:
                                Console.WriteLine("Invalid choice!");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Exam not found.");
                        return;
                    }
                }
                Console.WriteLine(ConfigurationManager.AppSettings["SeparationLine"]);
                ExportJson(exams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        #endregion

        #region Delete
        public static void DeleteProfessor()
        {
            try
            {
                List<Professor> professors = ImportFromJson<Professor>(); // Importa i dati esistenti

                if (professors.Count == 0)
                {
                    Console.WriteLine("There are no professors saved.\n");
                    return;
                }

                Console.WriteLine("Insert the ID of the professor you want to delete (UUID format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx):");
                string id = Console.ReadLine();

                // Verifica che l'ID inserito sia un UUID valido
                while (!Guid.TryParse(id, out Guid guid))
                {
                    Console.WriteLine("Invalid input - the ID must be in UUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                    Console.WriteLine("Insert the ID of the professor again:");
                    id = Console.ReadLine();
                }

                // Trova il professore con l'ID specificato
                Professor? prof = professors.Find(p => p.Id ==  Guid.Parse(id));
                if (prof == null)
                {
                    Console.WriteLine("Professor not found.");
                    return;
                }

                // Rimuovi il professore dalla lista
                professors.Remove(prof);
                Console.WriteLine($"Professor {prof.FullName} has been removed successfully.");
                Console.WriteLine(professors.FirstOrDefault(p => p.Id == Guid.Parse(id)) == null);
                // Salvataggio delle modifiche nel file JSON usando ExportJson
                ExportJson(professors); // Usa ExportJson per salvare

                Console.WriteLine("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void DeleteStudents()
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
                        string? matricola = GetValidInput("Insert the ID Matricola of the student ", input => !string.IsNullOrEmpty(input) && input.Length == 7);
                        
                        Student? stud = students.Find(stud => stud.Matricola == matricola);
                        if(stud != null){

                            Console.WriteLine($"Are you sure you want to delete the student: {stud!.FullName} ? (y / n)");
                            char answer = Console.ReadKey().KeyChar;
            
                            if (answer == 'y')
                            {
                                students.Remove(stud);
                                ExportJson(students);
                                Console.WriteLine("Student removed successfully");
                                loop = false;
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
                       
                        string id = GetValidInput("Insert the ID of the employee", input => !string.IsNullOrEmpty(input) && Guid.TryParse(input, out _));
                        Employee? emp = employees.Find(emp => emp.Id.ToString() == id);

                        if(emp != null){
                            Console.WriteLine($"Are you sure you want to delete the employee: {emp.FullName} ? (y / n)");
                            char answer = Console.ReadKey().KeyChar;

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

    public static void DeleteExams()
{
    try
    {
        List<Exam> exams = ImportFromJson<Exam>();
        List<Professor> professors = ImportFromJson<Professor>();

        if (exams.Count == 0)
        {
            Console.WriteLine("There are no exams.\n");
            return;
        }
        else
        {
            bool loop = true;
            while (loop){
                string id = GetValidInput("Insert the ID of the professor", input => !string.IsNullOrEmpty(input));
                Exam? exam = exams.Find(e => e.Id == Guid.Parse(id));

                if (exam != null)
                {
                    exams.Remove(exam); //Deletes the exam from the exam list
                    ExportJson(exams);

                    foreach (Professor professor in professors)
                    {
                        professor.Exams.RemoveAll(e => e.Id == exam.Id); //Deletes the exam from the professor's exam list
                    }

                    ExportJson(professors);

                    Console.WriteLine($"The exam '{exam.Name}' has been successfully deleted.");
                    loop = false;
                }
                else
                {
                    Console.WriteLine("The exam was not found, please try again.");
                }
            }
        }
    }
    catch (Exception ex)
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