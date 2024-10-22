using University.BLogic.DbManager.Entity;
using University.BLogic.DbManager.Relationship;
using University.DataModel;

namespace University.BLogic {
    internal class UniDbManager {
    
        #region Create
        public static void Create<T>(){
            if(typeof(T) == typeof(Professor))
                ProfessorLogic.CreateProfessor();  

            else if(typeof(T) == typeof(Student))
                StudentLogic.CreateStudent();
                
            else if(typeof(T) == typeof(Employee))
                EmployeeLogic.CreateEmployee();
                  
            else if(typeof(T) == typeof(Exam))
                ExamLogic.CreateExam();
           
            else if(typeof(T) == typeof(ProfessorCourseLogic))
                ProfessorCourseLogic.UpdateProfessorCourse('1');
           
            else if(typeof(T) == typeof(ProfessorExamLogic))
                ProfessorExamLogic.UpdateProfessorExam('1');
           
            else if(typeof(T) == typeof(StudentCourseLogic))
                StudentCourseLogic.UpdateStudentCourse('1');
           
            else if(typeof(T) == typeof(StudentExamLogic))
                StudentExamLogic.UpdateStudentExam('1'); 
        }
        
        #endregion
        
        #region Read

        public static void ReadAll<T>(){
            Console.WriteLine($"READ ALL: {typeof(T).Name}");
            if(typeof(T) == typeof(Professor))
                ProfessorLogic.ReadProfessors();  

            else if(typeof(T) == typeof(Student))
                StudentLogic.ReadStudents();
                
            else if(typeof(T) == typeof(Employee))
                EmployeeLogic.ReadEmployees();
                  
            else if(typeof(T) == typeof(Exam))
                ExamLogic.ReadExams();
           
            else if(typeof(T) == typeof(ProfessorCourseLogic))
                ProfessorCourseLogic.ReadAllProfessorCourse();
           
            else if(typeof(T) == typeof(ProfessorExamLogic))
                ProfessorExamLogic.ReadAllProfessorExam();
           
            else if(typeof(T) == typeof(StudentCourseLogic))
                StudentCourseLogic.ReadAllStudentCourse();
           
            else if(typeof(T) == typeof(StudentExamLogic))
                StudentExamLogic.ReadAllStudentExam();
        }

        public static void Read<T>(Guid? id = null, string? matricola = null){
            if(typeof(T) == typeof(Professor))
                ProfessorLogic.ReadProfessor(id!.Value);  

            else if(typeof(T) == typeof(Student))
                StudentLogic.ReadStudent(matricola!);
                
            else if(typeof(T) == typeof(Employee))
                EmployeeLogic.ReadEmployee(id!.Value);
                  
            else if(typeof(T) == typeof(Exam))
                ExamLogic.ReadExam(id!.Value);
           
            else if(typeof(T) == typeof(ProfessorCourseLogic))
                ProfessorCourseLogic.ReadProfessorCourse(id!.Value);
           
            else if(typeof(T) == typeof(ProfessorExamLogic))
                ProfessorExamLogic.ReadProfessorExam(id!.Value);
           
            else if(typeof(T) == typeof(StudentCourseLogic))
                StudentCourseLogic.ReadStudentCourse(matricola!);
           
            else if(typeof(T) == typeof(StudentExamLogic))
                StudentExamLogic.ReadStudentExam(matricola!); 
        }

        #endregion

        #region Update

        public static void Update<T>(){
            if(typeof(T) == typeof(Professor))
                ProfessorLogic.UpdateProfessor();  

            else if(typeof(T) == typeof(Student))
                StudentLogic.UpdateStudent();
                
            else if(typeof(T) == typeof(Employee))
                EmployeeLogic.UpdateEmployee();
                  
            else if(typeof(T) == typeof(Exam))
                ExamLogic.UpdateExam();
        }
        #endregion

        #region Delete

        public static void Delete<T>(Guid? id = null, string? matricola = null){
            if(typeof(T) == typeof(Professor))
                ProfessorLogic.DeleteProfessor();  

            else if(typeof(T) == typeof(Student))
                StudentLogic.DeleteStudent();
                
            else if(typeof(T) == typeof(Employee))
                EmployeeLogic.DeleteEmployee();
                  
            else if(typeof(T) == typeof(Exam))
                ExamLogic.DeleteExam();
           
            else if(typeof(T) == typeof(ProfessorCourseLogic))
                ProfessorCourseLogic.UpdateProfessorCourse('2');
           
            else if(typeof(T) == typeof(ProfessorExamLogic))
                ProfessorExamLogic.UpdateProfessorExam('2');
           
            else if(typeof(T) == typeof(StudentCourseLogic))
                StudentCourseLogic.UpdateStudentCourse('2');
           
            else if(typeof(T) == typeof(StudentExamLogic))
                StudentExamLogic.UpdateStudentExam('2'); 
        }

        #endregion
    }
}