﻿using University.BLogic;

namespace University.DataModel
{

    public class UniModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Faculty> Faculties { get; set; }

        public UniModel(string name, string address)
        {
            Name = name;
            Address = address;
        }

    }

    public class Faculty
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Faculties Name { get; set; }
        public string Address { get; set; }
        public int StudentsNumber { get; set; }
        public int LabsNumber { get; set; }
        public bool HasLibrary { get; set; }
        public bool HasCanteen { get; set; }
        public List<Exam> Exams { get; set; }
        public List<Courses> Courses { get; set; }

    }

    public class Exam
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Faculties Faculty { get; set; }
        public string Name { get; set; }
        public int CFU { get; set; }
        public DateTime Date { get; set; }
        public bool IsOnline { get; set; }
        public int Participants { get; set; }
        public ExamType ExamType { get; set; }
        public bool IsProjectRequired { get; set; }
    }

    public class Courses
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Faculties Faculty { get; set; }
        public string Name { get; set; }
        public int CFU { get; set; }
        public bool IsOnline { get; set; }
        public Classroom Classroom { get; set; }
    }
}
