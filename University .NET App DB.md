### Entity: Course

**Table Structure:**

| Field Name | Id               | Name | Faculty  | CFU      | IsOnline | Classroom |
| ---------- | ---------------- | ---- | -------- | -------- | -------- | --------- |
| Field Type | uniqueidentifier | text | smallint | smallint | bit      | smallint  |

**SQL Command:**

```sql
CREATE TABLE [dbo].[Course](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [text] NOT NULL,
	[Faculty] [smallint] NOT NULL,
	[CFU] [smallint] NOT NULL,
	[IsOnline] [bit] NOT NULL,
	[Classroom] [smallint] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

---
### Entity: Exam

**Table Structure:**

| Field Name  | Id | Name | Faculty | CFU | ExamDate | IsOnline | ExamType | IsProjectRequired |
|-------------| ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- |
| Field Type  | uniqueidentifier | text | smallint | smallint | date | bit | smallint | bit |

**SQL Command:**

```sql
CREATE TABLE [dbo].[Exam](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [text] NOT NULL,
	[Faculty] [smallint] NOT NULL,
	[CFU] [smallint] NOT NULL,
	[ExamDate] [date] NOT NULL,
	[IsOnline] [bit] NOT NULL,
	[ExamType] [smallint] NOT NULL,
	[IsProjectRequired] [bit] NOT NULL,
 CONSTRAINT [PK_Exam] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

---

### Entity: Faculty

**Table Structure:**

| Field Name  | Name | Address | LabsNumber | HasLibrary | HasCanteen |
|-------------| ---- | ---- | ---- | ---- | ---- |
| Field Type  | smallint | text | smallint | bit | bit |

**SQL Command:**

```sql
CREATE TABLE [dbo].[Faculty](
	[Name] [smallint] NOT NULL,
	[Address] [text] NOT NULL,
	[LabsNumber] [smallint] NOT NULL,
	[HasLibrary] [bit] NOT NULL,
	[HasCanteen] [bit] NOT NULL,
 CONSTRAINT [PK_Faculty] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

---
### Entity: Employee

**Table Structure:**

| Field Name  | Id | FullName | Gender | Address | Email | Phone | BirthYear | IsFullTime | MaritalStatus | Role | Faculty | HiringYear | Salary |
|-------------| ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- |
| Field Type  | uniqueidentifier | text | nchar | text | text | nchar | date | bit | smallint | smallint | smallint | date | decimal |

**SQL Command:**

```sql
CREATE TABLE [dbo].[Employee](
	[Id] [uniqueidentifier] NOT NULL,
	[FullName] [text] NOT NULL,
	[Gender] [nchar](1) NOT NULL,
	[Address] [text] NOT NULL,
	[Email] [text] NOT NULL,
	[Phone] [nchar](13) NOT NULL,
	[BirthYear] [date] NOT NULL,
	[IsFullTime] [bit] NOT NULL,
	[MaritalStatus] [smallint] NOT NULL,
	[Role] [smallint] NOT NULL,
	[Faculty] [smallint] NOT NULL,
	[HiringYear] [date] NOT NULL,
	[Salary] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

---
### Entity: Student

**Table Structure:**

| Field Name  | Id | FullName | Gender | Address | Email | Phone | BirthYear | IsFullTime | MaritalStatus | Matricola | RegistrationYear | Degree | ISEE |
|-------------| ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- |
| Field Type  | uniqueidentifier | text | nchar | text | text | nchar | date | bit | smallint | nchar | date | smallint | decimal |

**SQL Command:**

```sql
CREATE TABLE [dbo].[Student](
	[Id] [uniqueidentifier] NOT NULL,
	[FullName] [text] NOT NULL,
	[Gender] [nchar](1) NOT NULL,
	[Address] [text] NOT NULL,
	[Email] [text] NOT NULL,
	[Phone] [nchar](13) NOT NULL,
	[BirthYear] [date] NOT NULL,
	[IsFullTime] [bit] NOT NULL,
	[MaritalStatus] [smallint] NOT NULL,
	[Matricola] [nchar](7) NOT NULL,
	[RegistrationYear] [date] NOT NULL,
	[Degree] [smallint] NOT NULL,
	[ISEE] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[Matricola] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

---

### Relationship: Students_Exams

**Table Structure:**

| Field Name  | Id | ExamId | Matricola |
|-------------| ---- | ---- | ---- |
| Field Type  | uniqueidentifier | uniqueidentifier | nchar |

**SQL Command:**

```sql
CREATE TABLE [dbo].[Students_Exams](
	[Id] [uniqueidentifier] NULL,
	[ExamId] [uniqueidentifier] NOT NULL,
	[Matricola] [nchar](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ExamId] ASC,
	[Matricola] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```

---

### Relationship: Students_Faculties

**Table Structure:**

| Field Name  | Matricola | Faculty |
|-------------| ---- | ---- |
| Field Type  | nchar | smallint |

**SQL Command:**

```sql
CREATE TABLE [dbo].[Students_Faculties](
	[Matricola] [nchar](7) NOT NULL,
	[Faculty] [smallint] NOT NULL,
 CONSTRAINT [PK_Students_Faculties] PRIMARY KEY CLUSTERED 
(
	[Matricola] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```

---
### Relationship: Students_Courses

**Table Structure:**

| Field Name     | StudentId    | CourseId    |
|----------------|--------------|-------------|
| Field Type     | uniqueidentifier | uniqueidentifier |

**SQL Command:**

```sql
CREATE TABLE [dbo].[Students_Courses](
    [StudentId] [uniqueidentifier] NOT NULL,
    [CourseId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Students_Courses] PRIMARY KEY CLUSTERED 
(
    [StudentId] ASC,
    [CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [FK_Students] FOREIGN KEY([StudentId]) REFERENCES [dbo].[Student] ([Id]),
 CONSTRAINT [FK_Courses] FOREIGN KEY([CourseId]) REFERENCES [dbo].[Course] ([Id])
) ON [PRIMARY]
GO
```