using University.BLogic.DbManager.Relationship;
using University.DataModel;
namespace University.BLogic{
    public static class Menu
    {
        public static void ShowMenu()
        {
        bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Choose the entity for CRUD operations:");
                Console.WriteLine("1. Employee");
                Console.WriteLine("2. Professor");
                Console.WriteLine("3. Student");
                Console.WriteLine("4. Exam");
                Console.WriteLine("5. Course");
                Console.WriteLine("6. ProfessorCourse");
                Console.WriteLine("7. ProfessorExam");
                Console.WriteLine("8. StudentCourse");
                Console.WriteLine("9. StudentExam");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadKey().KeyChar.ToString();

                switch (choice)
                {
                    case "1":
                        PerformCRUD<Employee>();
                        break;
                    case "2":
                        PerformCRUD<Professor>();
                        break;
                    case "3":
                        PerformCRUD<Student>();
                        break;
                    case "4":
                        PerformCRUD<Exam>();
                        break;
                    case "5":
                        PerformCRUD<Courses>();
                        break;
                    case "6":
                        PerformCRUD<ProfessorCourseLogic>();
                        break;
                    case "7":
                        PerformCRUD<ProfessorExamLogic>();
                        break;
                    case "8":
                        PerformCRUD<StudentCourseLogic>();
                        break;
                    case "9":
                        PerformCRUD<StudentExamLogic>();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }
            }   
        }

        static void PerformCRUD<T>()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"CRUD operations for {typeof(T).Name}:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Enter your choice: ");
                string operation = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();

                switch (operation)
                {
                    case "1":
                        Console.WriteLine($"Creating new {typeof(T).Name}...");
                        UniDbManager.Create<T>();
                        break;
                    case "2":
                        Console.WriteLine($"Reading {typeof(T).Name} data...");
                        UniDbManager.ReadAll<T>();

                        break;
                    case "3":
                        Console.WriteLine($"Updating {typeof(T).Name}...");
                        UniDbManager.Update<T>();
                        break;
                    case "4":
                        Console.WriteLine($"Deleting {typeof(T).Name}...");
                        UniDbManager.Delete<T>();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
