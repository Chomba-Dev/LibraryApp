# Library Management System

This is a library management system built with ASP.NET Core and Entity Framework Core. It allows users to borrow and return books, manage user roles, and view borrowing history.

## Features
- User authentication and authorization
- CRUD operations for books and borrowings
- Role-based access control (Admin, Librarian, Member)

## Technologies Used
- ASP.NET Core
- Entity Framework Core
- MySQL
- Swagger for API documentation

## How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/Chomba-Dev/LibraryApp.git
   
2. Navigate to project directory:
   ```bash
   cd LibraryApp
   
3. Restore Dependencies:
   ```bash
   dotnet restore
   
4. Update database connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=librarydb;Uid=root;Pwd=password;"
   }
   
5. Run application:
   ```bash
   dotnet run
   
6. Api Documentation:
7. Open your browser and navigate to `https://localhost:5001/swagger` to view the API documentation.
```
