-- Part 1: Schema Design & Creation (10 Marks)

CREATE DATABASE TechZoneDB;

CREATE TABLE Products (
	ProductID INT PRIMARY KEY,
	ProductName VARCHAR(250) NOT NULL,
	Category VARCHAR(100),
	PRICE DECIMAL(10, 2) NOT NULL,
	StockQuantity INT DEFAULT 0
);

CREATE TABLE Customers (
	CustomerID INT PRIMARY KEY,
	FirstName VARCHAR(250) NOT NULL,
	LastName VARCHAR(250),
	Email VARCHAR(250) NOT NULL,
	RegistrationDate DATE NOT NULL
);

CREATE TABLE Orders (
	OrderID INT PRIMARY KEY,
	CustomerID INT NOT NULL,
	ProductID INT NOT NULL,
	OrderDate DATE NOT NULL,
	Quantity INT,
	FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
	FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Part 2: Data Population (10 Marks)

INSERT INTO Products (ProductID, ProductName, Category, PRICE, StockQuantity) VALUES
(1, 'MacBook Pro', 'Laptop', 1200.00, 10),
(2, 'iPhone 17', 'Phone', 999.00, 20),
(3, 'Dell XPS', 'Laptop', 1100.00, 8),
(4, 'Samsung S25', 'Phone', 850.00, 15),
(5, 'Sony Headphones', 'Audio', 200.00, 50);

INSERT INTO Customers (CustomerID, FirstName, LastName, Email, RegistrationDate) VALUES
(1, 'John', 'Doe', 'john@mail.com', '2026-01-10'),
(2, 'Jane', 'Smith', 'jane@mail.com', '2026-02-15'),
(3, 'Alice', 'Brown', 'alice@mail.com', '2026-03-20'),
(4, 'Bob', 'White', 'bob@mail.com', '2026-04-05');

INSERT INTO Orders (OrderID, CustomerID, ProductID, OrderDate, Quantity) VALUES
(1, 1, 1, '2026-01-11', 1),
(2, 2, 2, '2026-02-16', 2),
(3, 3, 5, '2026-03-21', 1),
(4, 4, 3, '2026-04-07', 1),
(5, 1, 4, '2026-01-12', 1);

select * from Products;
select * from customers;
select * from orders;

-- Part 3: Data Manipulation & Maintenance (10 Marks)

UPDATE Products SET PRICE = (PRICE+50)
WHERE ProductName = 'iPhone 17';

UPDATE Products SET StockQuantity = (StockQuantity-5)
WHERE ProductName = 'Sony Headphones';

DELETE FROM Orders WHERE OrderID = (
	SELECT O.OrderID 
	FROM Orders O
	JOIN Customers C ON O.CustomerID = C.CustomerID
	WHERE C.FirstName = 'Alice' AND C.LastName = 'Brown'
);

-- Part 4: Business Reporting & Analysis (10 Marks)

SELECT (C.FirstName + ' ' + C.LastName) AS FullName, P.ProductName, O.OrderDate
FROM Orders O
JOIN Products P ON O.ProductID = P.ProductID
JOIN Customers C ON O.CustomerID = C.CustomerID;

SELECT SUM(PRICE * O.Quantity) AS TotalRevenueFromLaptop
FROM Orders O
JOIN Products P ON O.ProductID = P.ProductID
WHERE P.Category = 'Laptop';

SELECT (C.FirstName + ' ' + C.LastName) AS FullName, SUM(O.Quantity) AS TotalQuantity
FROM Orders O
JOIN Customers C ON O.CustomerID = C.CustomerID
GROUP BY C.FirstName, C.LastName
ORDER BY TotalQuantity DESC;

-- Part 5: Advanced Engineering (10 Marks)

CREATE VIEW HighValueProducts AS
SELECT ProductID, ProductName, Category, PRICE
FROM Products
WHERE PRICE > 1000;

CREATE PROCEDURE RegisterCustomer	
	@CustomerID INT,
	@FirstName VARCHAR(250),
	@LastName VARCHAR(250),
	@Email VARCHAR(250)
AS 
BEGIN
	INSERT INTO Customers (CustomerID, FirstName, LastName, Email, RegistrationDate) VALUES
	(@CustomerID, @FirstName, @LastName, @Email, GETDATE())
END;
