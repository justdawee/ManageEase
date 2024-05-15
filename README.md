# ManageEase

ManageEase is a console application designed to manage employees in a company. It provides functionalities for adding, modifying, and deleting employee records, as well as user authentication.

## Features

- User registration and login
- Add new employee
- Modify existing employee details
- Delete employee
- Paginated list of employees

## Technologies Used

- C#
- .NET Core
- Entity Framework Core
- MariaDB
- Pomelo.EntityFrameworkCore.MySql

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 5.0 or higher)
- [MariaDB](https://mariadb.org/download/)

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/your-username/ManageEase.git
    cd ManageEase
    ```

2. Update the database connection string in `ManageEaseDbContext.cs`:
    ```csharp
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseExceptionProcessor()
            .UseMySql(
            "server=your_server;database=ManageEase;user=your_user;password=your_password;port=3306",
            new MariaDbServerVersion("10.6.16"));
    }
    ```

3. Run the application:
    ```sh
    dotnet run
    ```

### Usage

Upon running the application, you will be presented with a menu. Depending on whether you are logged in or not, you will see different options:

#### Login Menu

1. **Bejelentkezés** - Login with an existing account.
2. **Regisztráció** - Register a new account.
3. **Kilépés** - Exit the application.

#### Main Menu

1. **Új felhasználó hozzáadása** - Add a new employee.
2. **Felhasználó adatainak módosítása** - Modify an existing employee.
3. **Felhasználó törlése** - Delete an employee.
4. **Kilépés** - Logout and return to the login menu.

### Functionality

#### Register User

Registers a new user with a username, email, and password. Passwords are hashed before being stored.

#### Login User

Logs in an existing user by verifying the username and hashed password.

#### Add New Employee

Adds a new employee to the database. Prompts for details such as first name, last name, birthdate, email, phone, address, city, zip code, country, and salary.

#### Modify Employee

Modifies the details of an existing employee. The user is prompted to select an employee by ID from a paginated list.

#### Delete Employee

Deletes an existing employee from the database. The user is prompted to select an employee by ID from a paginated list.

### Validation

All input fields are validated for correctness before being accepted:
- **Email** - Must be a valid email format.
- **Username** - Must be alphanumeric and up to 32 characters long.
- **Password** - Must be between 8 and 256 characters long.
- **Names, Address, City, Country** - Various length and character validations.
- **Phone Number** - Must be a valid phone number format.
- **Zip Code** - Must be numeric and up to 16 characters long.
- **Birthdate** - Must be a valid date.
- **Salary** - Must be a non-negative integer.

### Error Handling

Custom exceptions are used to handle errors such as duplicate email or username registrations.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

