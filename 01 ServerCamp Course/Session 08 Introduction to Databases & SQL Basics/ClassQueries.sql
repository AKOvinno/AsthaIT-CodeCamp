-- Creating Database
CREATE DATABASE UniversityDB;
-- Selecting Database 
USE UniversityDB;
-- Creating table
DROP TABLE IF EXISTS Students;
CREATE TABLE Students (
	StudentID INT PRIMARY KEY,
	FirstName VARCHAR(50),
	LastName VARCHAR(50),
	EnrollmentDate DATETIME
);
-- Inserting data into a table
INSERT INTO Students (StudentID, FirstName, LastName, EnrollmentDate) 
VALUES 
(1, 'Alice', 'Johnson', '2023-09-01 09:00:00'), 
(2, 'John', 'Smith', '2023-09-01 10:30:00'), 
(3, 'Charlie', 'Davis', '2023-09-02 08:45:00'), 
(4, 'Diana', 'Prince', '2023-09-02 11:15:00'), 
(5, 'Ethan', 'Hunt', '2023-09-03 14:00:00'), 
(6, 'Fiona', 'Gallagher', '2023-09-03 15:30:00'), 
(7, 'George', 'Miller', '2023-09-04 09:20:00'), 
(8, 'Hannah', 'Abbott', '2023-09-04 13:45:00'), 
(9, 'Ian', 'Wright', '2023-09-05 10:00:00'), 
(10, 'Julia', 'Roberts', '2023-09-05 16:20:00'); 

-- Get all data from Students table
SELECT * FROM Students;
-- Retrieve Specific columns only
SELECT FirstName, LastName FROM Students;
-- Find Students named 'John'
SELECT * FROM Students WHERE FirstName = 'John';
-- Show 5 Most recently enrolled
SELECT TOP 6 * -- SQL Server uses TOP, Other databases use LIMIT for the same purpose
FROM Students
ORDER BY EnrollmentDate DESC;

-- Create Course Table
DROP TABLE IF EXISTS Courses;
CREATE TABLE Courses (
	CourseID INT PRIMARY KEY,
	CourseName VARCHAR(50),
	Credits INT
);
-- Insert into Course Table
INSERT INTO Courses (CourseID, CourseName, Credits) 
VALUES 
(1, 'Introduction to Computer Science', 4), 
(2, 'Calculus I', 4), 
(3, 'English Composition', 3), 
(4, 'Art History', 2), 
(5, 'Database Systems', 3);

SELECT * FROM Courses;

-- Query Filtered Results (Credits > 3)
SELECT * FROM Courses WHERE Credits > 3;
-- Sort Alphabetically (ORDER BY CourseName ASC)
SELECT * FROM Courses ORDER BY CourseName ASC;