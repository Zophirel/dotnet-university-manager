using System.Configuration;
using System.Text.Json;
using University.DataModel;

namespace University.BLogic {
    internal class Utils {
        public static List<T> ImportFromCsv<T>(string path)
        {
            var list = new List<T>();
            var lines = File.ReadAllLines(path);
            var header = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var obj = Activator.CreateInstance<T>();

                for (int j = 0; j < header.Length; j++)
                {
                    var property = obj?.GetType().GetProperty(header[j]);
            
                    if (property != null)
                    {
                        object convertedValue = ConvertValue(values[j], property.PropertyType);
                        property.SetValue(obj, convertedValue);
                    }
                }
                list.Add(obj);
            }
                     
            UniversityManager.ExportJson(list);
            return list;
        }
        private static object ConvertValue(string value, Type targetType)
        {
            Console.WriteLine($"{value} {targetType}");
            if (targetType == typeof(Guid))
            {
            
                return Guid.Parse(value);
            }
            else if (targetType.IsEnum)
            {
                if(value.Split(" ").Length > 1)
                {
                    value = value.Replace(" ", "_"); 
                }
                
                value = value.ToUpper();
                return Enum.Parse(targetType, value);
            }
            else if(targetType == typeof(Faculty))
            {
                List<Faculty> faculties = UniversityManager.ImportFromJson<Faculty>()!;
                Faculty faculty = faculties?.Find(f => f.Name == (Faculties)Enum.Parse(typeof(Faculties), value))!;
                return faculty;
               
            }else
            {

                return Convert.ChangeType(value, targetType);
            }
        }
        public static void PopulateProfessors()
        {
            List<Professor> professors = UniversityManager.ImportFromJson<Professor>()!;
            List<Exam> exams = UniversityManager.ImportFromJson<Exam>()!;
            List<Courses> courses = UniversityManager.ImportFromJson<Courses>()!;
            Random rInt = new ();
            Random rInt2 = new ();
            Console.WriteLine("Professors: " + professors.Count);
           
            for(int i = 0; i < professors.Count; i++){
                Console.WriteLine(professors[i].Exams.Count);
                if(professors[i].Exams == null || professors[i].Exams.Count == 0)
                {
                    List<Exam> professorExams = exams.FindAll(e => e.Faculty == professors[i].Faculty);
                    
                    
                    int numberOfExams = rInt2.Next(1, 4);
                    for(int j = 0; j < numberOfExams; j++)
                    {
                        professors[i].Exams.Add(professorExams[rInt.Next(0, professorExams.Count)]);
                    }
                }

                
                if(professors[i].Courses == null || professors[i].Courses.Count == 0)
                {
                    List<Courses> course = courses.FindAll(c => c.Faculty == professors[i].Faculty);
                    foreach(Exam exam in professors[i].Exams)
                    {
                        int randCourse = rInt2.Next(0, course.Count);    
                        professors[i].Courses.Add(course[randCourse ]);
                    }
                }
            }
            Console.WriteLine(professors[0].Exams.Count);
            UniversityManager.ExportJson(professors);

        }
        public static void PopulateFaculties(){
            List<Faculty> faculties = UniversityManager.ImportFromJson<Faculty>()!;
            List<Exam> exams = UniversityManager.ImportFromJson<Exam>()!;
            List<Courses> courses = UniversityManager.ImportFromJson<Courses>()!;

            foreach(Faculty faculty in faculties)
            {
                if(faculty.Exams == null || faculty.Exams.Count == 0)
                {
                    Console.WriteLine(faculty.Name);
                    faculty.Exams = exams.FindAll(e => e.Faculty == faculty.Name);

                }

                if(faculty.Courses == null || faculty.Courses.Count == 0)
                {
                    faculty.Courses = courses.FindAll(c => c.Faculty == faculty.Name);
                }
            }

            Console.WriteLine(faculties[0].Exams.Count);
            var option = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(faculties, option);
            File.WriteAllText(ConfigurationManager.AppSettings["FileFacultiesJson"]!, json);
        }
        
        /// <summary>
        /// Prompts the user a custom input and validates it using the provided validator function.
        /// </summary>
        /// <param name="prompt">The prompt message to display to the user.</param>
        /// <param name="validator">A function to validate the user's input.</param>
        /// <returns>A valid input string.</returns>
        internal static string GetValidInput(string prompt, Func<string?, bool> validator)
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
        internal static string GetFaculty(){
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
        internal static string GetRole(){
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
        internal static string GetMaritialStatus(){
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
        internal static string GetDegree(){
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
        internal static string GetExamType(){
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
        internal static string GetClassroom(){
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

        internal static string GetGender() => GetValidInput("Enter Gender (Male / Female / X): ", input => !string.IsNullOrEmpty(input) && (input?.ToUpper() == "MALE" || input?.ToUpper() == "FEMALE" || input?.ToUpper() == "X"));
        
        /// <summary>
        /// Check the user input to match the Guid value.
        /// </summary>
        /// <returns>A string representing the integer that rapresent the correct Classroom enum.</returns>
        internal static string GetValidId() => GetValidInput($"Enter the ID: ", input => !string.IsNullOrEmpty(input) && Guid.TryParse(input, out _));
    }    
}