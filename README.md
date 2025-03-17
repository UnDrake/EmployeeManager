# Employee Management Application

## Description
This is a **C# ASP.NET**, **Avalonia UI** and **SQL** application designed for managing company employee data. The application utilizes **OOP principles** and stores all employee-related data in an **SQL database** (normalized to NF). The interaction with the database is handled using **direct SQL queries** (without Entity Framework or ORM).

## Features
### Data Storage
The application stores and manages the following information:
1. **Departments**
2. **Positions**
3. **Employee Details**
   - Full Name
   - Address
   - Phone Number
   - Date of Birth
   - Hiring Date
   - Salary
4. **Company Information**

All data is stored atomically, following the principles of **database normalization (NF)**.

### Application Menu
#### 1. Home Page
- Displays **menu options** and **company information**.

#### 2. Employee Overview Page
- **Search and filtering options**:
  - Filter employees by **position, department, name, etc.**
- **Employee Profile Card**:
  - View employee details
  - **Edit employee information**

#### 3. Salary Report Page
- **Filter employees** based on various criteria.
- **Generate a salary report**:
  - Select a department and **view total salary expense**.
  - **Display results in a table** within the application.
- **Export data to a TXT file**.

## Setup Instructions
### 1. Database Setup
Before running the application, you need to create and populate the database.

1. **Create Database & Tables:** Run the script:
   ```sql
   CreateDataBaseScript.sql
   ```
2. **Seed Initial Data:** Populate tables using:
   ```sql
   SeedDataScript.sql
   ```

### 2. Running the Application
1. You may need to update the **database connection string** in `appsettings.json` to match your database configuration.
2. Build and run the application.
3. Navigate through the **menu** to access different features.
