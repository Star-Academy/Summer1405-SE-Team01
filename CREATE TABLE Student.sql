CREATE TABLE Student
(
    StudentNumber VARCHAR(8) NOT NULL,
    Grade FLOAT,
    FirstName VARCHAR(20) NOT NULL,
    LastName VARCHAR(20) NOT NULL,
    IsMale BIT NOT NULL,                  
    DateOfBirth DATETIME2 NOT NULL,       
    LeftUnitsCount INT NOT NULL
);

INSERT INTO Student (StudentNumber, Grade, FirstName, LastName, IsMale, DateOfBirth, LeftUnitsCount)
VALUES 
    ('99123460', 19.95, 'Ali', 'Hosseini', 1, '2001-03-15 00:00:00', 10),
    ('99123461', 18.50, 'Neda', 'Ahmadi', 0, '2002-11-05 00:00:00', 45),
    ('99123462', 11.80, 'Mohammad', 'Kaviani', 1, '2000-07-22 00:00:00', 18),
    ('99123463', 12.00, 'Zahra', 'Pirani', 0, '2001-09-30 00:00:00', 70),
    ('99123464', 10.90, 'Hossein', 'Ebrahimi', 1, '2002-01-12 00:00:00', 25),
    ('99123465', 16.25, 'Maryam', 'Ghasemi', 0, '2000-04-18 00:00:00', 32),
    ('99123466', 12.00, 'Amir', 'Salehi', 1, '2001-06-25 00:00:00', 5),
    ('99123467', 13.75, 'Fatemeh', 'Soltani', 0, '2002-02-14 00:00:00', 60),
    ('99123468', 14.10, 'Saman', 'Nouri', 1, '2000-12-08 00:00:00', 15),
    ('99123469', 15.50, 'Mahsa', 'Yazdani', 0, '2001-08-01 00:00:00', 40);

SELECT TOP 3 *
FROM Student
WHERE IsMale = 1 AND Grade BETWEEN 10 AND 15
ORDER BY Grade DESC;

CREATE TABLE Student2
(
    StudentNumber VARCHAR(8) NOT NULL PRIMARY KEY,
    Grade FLOAT,
    FirstName VARCHAR(20) NOT NULL,
    LastName VARCHAR(20) NOT NULL,
    IsMale BIT NOT NULL,                  -- تبدیل BOOLEAN به BIT
    DateOfBirth DATETIME2 NOT NULL,       -- تبدیل TIMESTAMP به DATETIME2
    LeftUnitsCount INT NOT NULL
);

CREATE TABLE Enrollment
(
    CourseName VARCHAR(20),
    ParticipantStudentNumber VARCHAR(8),
    CONSTRAINT studentNumberFK FOREIGN KEY (ParticipantStudentNumber)
    REFERENCES Student2(StudentNumber)
);

INSERT INTO Student2 (StudentNumber, Grade, FirstName, LastName, IsMale, DateOfBirth, LeftUnitsCount)
VALUES 
    ('99123460', 19.95, 'Ali', 'Hosseini', 1, '2001-03-15 00:00:00', 10),
    ('99123461', 18.50, 'Neda', 'Ahmadi', 0, '2002-11-05 00:00:00', 45),
    ('99123462', 11.80, 'Mohammad', 'Kaviani', 1, '2000-07-22 00:00:00', 18),
    ('99123463', 12.00, 'Zahra', 'Pirani', 0, '2001-09-30 00:00:00', 70),
    ('99123464', 10.90, 'Hossein', 'Ebrahimi', 1, '2002-01-12 00:00:00', 25),
    ('99123465', 16.25, 'Maryam', 'Ghasemi', 0, '2000-04-18 00:00:00', 32),
    ('99123466', 12.00, 'Amir', 'Salehi', 1, '2001-06-25 00:00:00', 5),
    ('99123467', 13.75, 'Fatemeh', 'Soltani', 0, '2002-02-14 00:00:00', 60),
    ('99123468', 14.10, 'Saman', 'Nouri', 1, '2000-12-08 00:00:00', 15),
    ('99123469', 15.50, 'Mahsa', 'Yazdani', 0, '2001-08-01 00:00:00', 40);

INSERT INTO Enrollment (CourseName, ParticipantStudentNumber)
VALUES 
    ('Database', '99123460'),
    ('OS', '99123460'),
    ('Database', '99123461'),
    ('AI', '99123461'),
    ('Network', '99123465'),
    ('Algorithm', '99123466');


SELECT S.StudentNumber, S.FirstName, S.LastName, E.CourseName
FROM Student2 S
INNER JOIN Enrollment E ON S.StudentNumber = E.ParticipantStudentNumber;

SELECT S.StudentNumber, S.FirstName, S.LastName, E.CourseName
FROM Student2 S
LEFT JOIN Enrollment E ON S.StudentNumber = E.ParticipantStudentNumber;

SELECT S.StudentNumber, S.FirstName, S.LastName, E.CourseName
FROM Student2 S
RIGHT JOIN Enrollment E ON S.StudentNumber = E.ParticipantStudentNumber;

SELECT S.StudentNumber, S.FirstName, S.LastName, E.CourseName
FROM Student2 S
FULL OUTER JOIN Enrollment E ON S.StudentNumber = E.ParticipantStudentNumber;


SELECT IsMale, COUNT(IsMale) AS TotalCount 
FROM Student2 
GROUP BY IsMale;

SELECT ParticipantStudentNumber, COUNT(ParticipantStudentNumber) AS TotalCourses
FROM Enrollment
GROUP BY ParticipantStudentNumber;