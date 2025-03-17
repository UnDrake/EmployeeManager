USE CompanyDB;

INSERT INTO Companies (Name, Info) 
VALUES ('TechCorp', 'A leading technology company specializing in AI solutions.');
GO

INSERT INTO Departments (Name) 
VALUES ('Software Development'), ('HR'), ('Finance'), ('Marketing');
GO

INSERT INTO Positions (Name, DepartmentID)
VALUES 
    ('Software Engineer', 1),
    ('HR Manager', 2),
    ('Financial Analyst', 3),
    ('Marketing Specialist', 4);
GO

INSERT INTO Addresses (Address)
VALUES 
    ('123 Silicon Valley, CA'), 
    ('456 Park Avenue, NY'), 
    ('789 Business St, TX'),
    ('101 Innovation Road, WA'), 
    ('202 AI Blvd, MA');
GO

INSERT INTO Employees (FullName, Phone, BirthDate, HireDate, Salary, PositionID, AddressID, CompanyID)
VALUES 
    ('John Smith', '+1-555-1234', '1990-05-15', '2023-01-10', 75000.00, 1, 1, 1),
    ('Emily Johnson', '+1-555-5678', '1985-09-22', '2022-03-15', 82000.00, 2, 2, 1),
    ('Michael Brown', '+1-555-9876', '1993-07-30', '2021-07-05', 68000.00, 3, 3, 1),
    ('Sophia Davis', '+1-555-6543', '1997-12-10', '2024-02-01', 72000.00, 1, 4, 1),
    ('Daniel Wilson', '+1-555-3210', '1988-06-18', '2019-11-20', 95000.00, 2, 5, 1);
GO