/*
*    THIS FILE IS COAUTHORED BY:
*    - Francesco Piersanti
*    - Delia Ricca
*    - Andreea Cojocaru
*    - Mattia Andrea Tarantino
*    All the enums written here are the result of a collective brainstormig work.
*/


namespace University.BLogic
{
    public enum StartMenuFunctions
    {
        INSERT = 1,
        VIEW,
        UPDATE,
        DELETE,
        EXIT
    }

    public enum Faculties
    {
        COMPUTER_SCIENCE = 1,
        BUSINESS_AND_MANAGEMENT,
        MATHEMATICS,
        PSYCHOLOGY,
        LAW,
        FASHION_DESIGN,
        NURSING,
        LANGUAGES,
        BIOLOGY
    }

    public enum Degrees
    {
        BACHELOR = 1,
        MASTER,
        PHD,
        FIVE
    }

    public enum Roles
    {
        TECHNICIAN = 1,
        SECRETARY,
        CLEANING_STAFF,
        RECTOR,
        PROFESSOR
    }

    public enum Status
    {
        SINGLE = 1,
        MARRIED,
        DIVORCED,
        WIDOWED
    }

    public enum ExamType
    {
        WRITTEN = 1,
        ORAL,
        WRITTEN_AND_ORAL,
    }

    public enum Classroom
    {
        A = 1,
        B,
        C,
        D,
        E,
        F,
        LAB_1,
        LAB_2,
        LAB_3
    }
}
