using University.BLogic;
namespace University.DataModel {
    public static class Menu
    {
        private static readonly string separator = new('-', 40);

        /// <summary>
        /// Displays the main menu to the user and routes them to the appropriate CRUD operation based on their choice.
        /// </summary>
        public static void ShowMainMenu()
        {
            bool showMenu = true;
            while (showMenu)
            {
                Console.Clear();
                Console.WriteLine(separator);
                Console.WriteLine("Welcome to the University Management System");
                Console.WriteLine(separator);
                Console.WriteLine("Please choose an operation:");
                Console.WriteLine("1. Manage Professors");
                Console.WriteLine("2. Manage Students");
                Console.WriteLine("3. Manage Employees");
                Console.WriteLine("4. Manage Exams");
                Console.WriteLine("5. Exit");
                Console.WriteLine(separator);
                Console.Write("Enter your choice (1-5): ");

                string choice = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine("\n");

                switch (choice)
                {
                    case "1":
                        ManageProfessors();
                        break;
                    case "2":
                        ManageStudents();
                        break;
                    case "3":
                        ManageEmployees();
                        break;
                    case "4":
                        ManageExams();
                        break;
                    case "5":
                        showMenu = false;
                        Console.WriteLine("Exiting the system. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option (1-5).");
                        break;
                }

                if (showMenu)
                {
                    Console.WriteLine("\nPress any key to return to the main menu...");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Allows the user to perform CRUD operations on Professors.
        /// </summary>
        private static void ManageProfessors()
        {
            Console.Clear();
            Console.WriteLine(separator);
            Console.WriteLine("Professor Management");
            Console.WriteLine(separator);
            Console.WriteLine("Please choose an operation:");
            Console.WriteLine("1. Add Professor");
            Console.WriteLine("2. Update Professor");
            Console.WriteLine("3. Delete Professor");
            Console.WriteLine("4. View All Professors");
            Console.WriteLine("5. Back to Main Menu");
            Console.WriteLine(separator);
            Console.Write("Enter your choice (1-5): ");

            string choice = Console.ReadKey().KeyChar.ToString();
            Console.WriteLine("\n");

            switch (choice)
            {
                // Add
                case "1":
                    UniversityManager.InsertProfessor();
                    break;
                
                // Update
                case "2":
                    UniversityManager.UpdateProfessor();
                    break;
                
                // Delete
                case "3":
                    UniversityManager.DeleteProfessor();
                    break;
                
                // Read
                case "4":
                    UniversityManager.ReadProfessors();
                    break;
                case "5":
                    return; // Go back to the main menu
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option (1-5).");
                    break;
            }
        }

        /// <summary>
        /// Allows the user to perform CRUD operations on Students.
        /// </summary>
        private static void ManageStudents()
        {
            Console.Clear();
            Console.WriteLine(separator);
            Console.WriteLine("Student Management");
            Console.WriteLine(separator);
            Console.WriteLine("Please choose an operation:");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. Update Student");
            Console.WriteLine("3. Delete Student");
            Console.WriteLine("4. View All Students");
            Console.WriteLine("5. Back to Main Menu");
            Console.WriteLine(separator);
            Console.Write("Enter your choice (1-5): ");

            string choice = Console.ReadKey().KeyChar.ToString();
            Console.WriteLine("\n");

            switch (choice)
            {
                // Create
                case "1":
                    UniversityManager.InsertStudent();
                    break;
                // Update
                case "2":
                    UniversityManager.UpdateStudent();
                    break;
                // Delete
                case "3":
                    UniversityManager.DeleteStudent();
                    break;
                // Read
                case "4":
                    UniversityManager.ReadStudents();
                    break;
                case "5":
                    return; // Go back to the main menu
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option (1-5).");
                    break;
            }
        }

        /// <summary>
        /// Allows the user to perform CRUD operations on Employees.
        /// </summary>
        private static void ManageEmployees()
        {
            Console.Clear();
            Console.WriteLine(separator);
            Console.WriteLine("Employee Management");
            Console.WriteLine(separator);
            Console.WriteLine("Please choose an operation:");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Update Employee");
            Console.WriteLine("3. Delete Employee");
            Console.WriteLine("4. View All Employees");
            Console.WriteLine("5. Back to Main Menu");
            Console.WriteLine(separator);
            Console.Write("Enter your choice (1-5): ");

            string choice = Console.ReadKey().KeyChar.ToString();
            Console.WriteLine("\n");

            switch (choice)
            {
                // Create
                case "1":
                    UniversityManager.InsertEmployee();
                    break;
                // Update
                case "2":
                    UniversityManager.UpdateEmployee();
                    break;
                // Delete
                case "3":                   
                    UniversityManager.DeleteEmployee();
                    break;
                // Read
                case "4": 
                    UniversityManager.ReadEmployees();
                    break;
                case "5":
                    return; // Go back to the main menu
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option (1-5).");
                    break;
            }
        }

        /// <summary>
        /// Allows the user to perform CRUD operations on Exams.
        /// </summary>
        private static void ManageExams()
        {
            Console.Clear();
            Console.WriteLine(separator);
            Console.WriteLine("Exam Management");
            Console.WriteLine(separator);
            Console.WriteLine("Please choose an operation:");
            Console.WriteLine("1. Add Exam");
            Console.WriteLine("2. Update Exam");
            Console.WriteLine("3. Delete Exam");
            Console.WriteLine("4. View All Exams");
            Console.WriteLine("5. Back to Main Menu");
            Console.WriteLine(separator);
            Console.Write("Enter your choice (1-5): ");

            string choice = Console.ReadKey().KeyChar.ToString();
            Console.WriteLine("\n");

            switch (choice)
            {
                // Create
                case "1":
                    UniversityManager.InsertExam();
                    break;
                // Update
                case "2":
                    UniversityManager.UpdateExams();
                    break;
                // Delete
                case "3":
                    UniversityManager.DeleteExam();
                    break;
                // Read
                case "4":
                    UniversityManager.ReadExams();
                    break;
                case "5":
                    return; // Go back to the main menu
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option (1-5).");
                    break;
            }
        }
    }

}
