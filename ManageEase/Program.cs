using System.Text;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using ManageEase.Entities;
using ManageEase.Inputs;
using ManageEase.Services;
using ManageEase.Services.Interfaces;
using ManageEase.Utilities;

namespace ManageEase
{
    internal class Program
    {
        private static readonly IValidationService Validate = new ValidationService();
        private static readonly IPasswordHasher Hasher = new PasswordHasher();
        private static long? _loggedinuserid;

        private static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            
            await using(var db = new ManageEaseDbContext())
            {
                await db.Database.EnsureCreatedAsync();
            }

            while (true)
            {
                if (_loggedinuserid == null)
                {
                    await ShowLoginMenuAsync();
                }
                else
                {
                    await ShowMainMenuAsync();
                }
            }
        }
        
        private static async Task ShowMainMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("-- ManageEase felület --");
            Console.WriteLine("1. Új felhasználó hozzáadása");
            Console.WriteLine("2. Felhasználó adatainak módosítása");
            Console.WriteLine("3. Felhasználó törlése");
            Console.WriteLine("4. Kilépés");
            Console.Write("Válasszon egy opciót: ");
            
            switch (Console.ReadLine())
            {
                case "1":
                    await AddEmployeeAsync();
                    break;
                case "2":
                    await ModifyEmployeeAsync();
                    break;
                case "3":
                    await DeleteEmployeeAsync();
                    break;
                case "4":
                    _loggedinuserid = null;
                    break;
                default:
                    Console.WriteLine("Érvénytelen opció!");
                    break;
            }
        }

        private static async Task ShowLoginMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("-- Felhasználói felület --");
            Console.WriteLine("1. Bejelentkezés");
            Console.WriteLine("2. Regisztráció");
            Console.WriteLine("3. Kilépés");
            Console.Write("Válasszon egy opciót: ");
            
            switch (Console.ReadLine())
            {
                case "1":
                    await LoginAsync();
                    break;
                case "2":
                    await RegisterAsync();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Érvénytelen opció!");
                    break;
            }
        }

        /// <summary>
        /// Handles user registration by collecting input for username, email, and password.
        /// Validates the input and saves the new user to the database, handling unique constraint exceptions.
        /// </summary>
        private static async Task RegisterAsync()
        {
            Eleje:
            Console.Clear();
            Console.WriteLine("-- REGISZTRÁCIÓ --");
            var input = new RegisterUserInput();
            
            input.Username = ReadInput(
                "Felhasználónév: ",
                Validate.IsValidUsername,
                "A felhasználónév nem megfelelő. (max. 32 karakter)"
            );
            
            do
            {
                Console.Write("E-Mail: ");
                input.Email = Console.ReadLine() ?? string.Empty;
                if (!Validate.IsValidEmail(input.Email))
                {
                    Console.WriteLine("Nem megfelelő email cím. (pelda@mail.com)");
                }
                else
                {
                    break;
                }
            } while (true);
            
            do
            {
                Console.Write("Jelszó: ");
                var password = ConsoleExtensions.ReadPassword();

                input.Password = Hasher.HashPassword(password, input.Username);

                if (!Validate.IsValidPassword(password))
                {
                    Console.WriteLine("A jelszó túl rövid vagy hosszú. (min. 8 karakter)");
                }
                else
                {
                    break;
                }
            } while (true);
            
            try
            {
                await using (var db = new ManageEaseDbContext())
                {
                    var user = new User
                    {
                        Username = input.Username,
                        Password = input.Password,
                        Email = input.Email.ToLowerInvariant()
                    };
                    await db.Users.AddAsync(user);
                    await db.SaveChangesAsync();
                }
            }
            catch (UniqueConstraintException e)
            {
                foreach (var entries in e.ConstraintProperties)
                {
                    switch (entries)
                    {
                        case nameof(User.Email):
                            Console.WriteLine("Az email cím már foglalt!");
                            goto Eleje;
                        case nameof(User.Username):
                            Console.WriteLine("A felhasználónév már foglalt!");
                            goto Eleje;
                    }
                }

                throw;
            }

            Console.WriteLine("\nSikeres regisztráció!");
            await ConsoleExtensions.ReturnTimerAsync(3, "visszalép a főmenübe.");
        }

        /// <summary>
        /// Handles user login by collecting and validating the username and password.
        /// Sets the logged-in user ID if the credentials are correct, otherwise prompts the user to try again.
        /// </summary>
        private static async Task LoginAsync()
        {
            Eleje:
            Console.Clear();
            Console.WriteLine("-- BEJELENTKEZÉS --");
            
            Console.Write("Felhasználónév: ");
            var input = new LoginUserInput();
            input.Username = Console.ReadLine() ?? string.Empty;
            
            Console.Write("Jelszó: ");
            input.Password = Hasher.HashPassword(ConsoleExtensions.ReadPassword(), input.Username);
            long userid;
            
            await using (var db = new ManageEaseDbContext())
            {
                userid = await db.Users
                    .Where(u => u.Username == input.Username)
                    .Where(u => u.Password == input.Password)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();
            }
            
            if (userid <= 0)
            {
                Console.WriteLine("\n\nHibás felhasználónév vagy jelszó.");
                await Task.Delay(3000);
                goto Eleje;
            }
            
            _loggedinuserid = userid;
            Console.WriteLine("\nSikeres bejelentkezés!");
            await ConsoleExtensions.ReturnTimerAsync(3, "beléptet a rendszerbe.");
        }

        /// <summary>
        /// Handles adding a new employee by collecting input for various employee details.
        /// Validates the input and saves the new employee to the database, handling unique constraint exceptions.
        /// </summary>
        private static async Task AddEmployeeAsync()
        {
            Eleje:
            Console.Clear();
            Console.WriteLine("-- ÚJ FELHASZNÁLÓ HOZZÁADÁSA --");
            var input = new AddNewEmployeeInput();
            
            input.Lastname = ReadValidLastname();
            input.Firstname = ReadValidFirstname();
            input.Email = ReadValidEmail();
            input.Phone = ReadValidPhonenumber();
            input.Address = ReadValidAddress();
            input.City = ReadValidCity();
            input.Zip = ReadValidZip();
            input.Country = ReadValidCountry();
            input.Birthdate = ReadValidBirthdate();
            input.Salary = ReadValidSalary();
            input.RoleDiscriminator = ReadValidRole();
            
            try 
            {
                await using(var db = new ManageEaseDbContext())
                {
                    var employee = new Employee
                    {
                        Firstname = input.Firstname,
                        Lastname = input.Lastname,
                        Birthdate = input.Birthdate,
                        Email = input.Email,
                        Phone = input.Phone,
                        Address = input.Address,
                        City = input.City,
                        Zip = input.Zip,
                        Country = input.Country,
                        Salary = input.Salary
                    };
                    await db.Employees.AddAsync(employee);
                    await db.SaveChangesAsync();
                }
            }
            catch (UniqueConstraintException e)
            {
                foreach(var entries in e.ConstraintProperties)
                {
                    switch (entries)
                    {
                        case nameof(Employee.Email):
                            Console.WriteLine("Az email cím már foglalt!");
                            goto Eleje;
                    }
                }
                throw;
            }
            
            Console.WriteLine("\nSikeres hozzáadás!");
            await ConsoleExtensions.ReturnTimerAsync(3, "visszalép a főmenübe.");
        }

        /// <summary>
        /// Handles modifying an existing employee's data by listing employees,
        /// allowing the user to select an employee by ID, and then updating their details.
        /// </summary>
        private static async Task ModifyEmployeeAsync()
        {
            Console.Clear();
            Console.WriteLine("-- FELHASZNÁLÓ ADATAINAK MÓDOSÍTÁSA --");
            
            await ListEmployeesPaginatedAsync();

            long employeeId;
            await using var db = new ManageEaseDbContext();
            
            do
            {
                Console.Write("Adja meg a módosítani kívánt alkalmazott ID-jét: ");
                if (long.TryParse(Console.ReadLine(), out employeeId) &&
                    await db.Employees.AnyAsync(e => e.Id == employeeId))
                {
                    break;
                }

                Console.WriteLine("Nem létező alkalmazott ID.");
            } while (true);
            
            var employee = await db.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                Console.WriteLine("Alkalmazott nem található.");
                return;
            }
            
            employee.Lastname = ReadInput($"Vezetéknév[{employee.Lastname}]: ", Validate.IsValidLastName, "Nem megfelelő vezetéknév. (max. 64 karakter)");
            employee.Firstname = ReadInput($"Keresztnév[{employee.Firstname}]: ", Validate.IsValidFirstname, "Nem megfelelő keresztnév. (max. 64 karakter)");
            employee.Email = ReadInput($"E-Mail[{employee.Email}]: ",Validate.IsValidEmail,"Az e-mail cím nem megfelelő. (pelda@email.com)");
            employee.Phone = ReadOptionalInput($"Telefonszám[{employee.Phone}]: ", Validate.IsValidPhoneNumber, "Nem megfelelő telefonszám. (max. 16 karakter)");
            employee.Address = ReadOptionalInput($"Cím[{employee.Address}]: ", Validate.IsValidAddress, "Nem megfelelő cím. (max. 128 karakter)");
            employee.City = ReadOptionalInput($"Város[{employee.City}]: ", Validate.IsValidCity, "Nem megfelelő városnév. (max. 64 karakter)");
            employee.Zip = ReadOptionalInput($"Irányítószám[{employee.Zip}]: ", Validate.IsValidZipCode, "Nem megfelelő irányítószám. (max. 16 karakter)");
            employee.Country = ReadOptionalInput($"Ország[{employee.Country}]: ", Validate.IsValidCountry, "Nem megfelelő ország név. (max. 64 karakter)");
            employee.Birthdate = ReadValidBirthdate();
            employee.Salary = ReadValidSalary();
            
            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("\nSikeres módosítás!");
            }
            catch (UniqueConstraintException e)
            {
                if (e.ConstraintProperties.Contains(nameof(Employee.Email)))
                {
                    Console.WriteLine("Az email cím már foglalt!");
                }
                throw;
            }

            await ConsoleExtensions.ReturnTimerAsync(3, "visszalép a főmenübe.");
        }

        /// <summary>
        /// Handles deleting an employee by listing employees,
        /// allowing the user to select an employee by ID, and then removing them from the database.
        /// </summary>
        private static async Task DeleteEmployeeAsync()
        {
            Console.Clear();
            Console.WriteLine("-- FELHASZNÁLÓ TÖRLÉSE --");
            
            await ListEmployeesPaginatedAsync();

            long employeeId;
            await using var db = new ManageEaseDbContext();
            do
            {
                Console.Write("Adja meg a törölni kívánt alkalmazott ID-jét: ");
                if (long.TryParse(Console.ReadLine(), out employeeId) &&
                    await db.Employees.AnyAsync(e => e.Id == employeeId))
                {
                    break;
                }

                Console.WriteLine("Nem létező alkalmazott ID.");
            } while (true);
            
            var employee = await db.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                Console.WriteLine("Alkalmazott nem található.");
                return;
            }
            
            db.Employees.Remove(employee);
            
            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("\nSikeres törlés!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Hiba történt: {e.Message}");
            }

            await ConsoleExtensions.ReturnTimerAsync(3, "visszalép a főmenübe.");
        }

        /// <summary>
        /// Lists employees in a paginated manner, displaying a certain number of employees per page
        /// and allowing the user to navigate between pages.
        /// </summary>
        private static async Task ListEmployeesPaginatedAsync()
        {
            await using var db = new ManageEaseDbContext();
            int page = 0;
            const int pageSize = 100;

            while (true)
            {
                var employees = await db.Employees
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (employees.Count == 0)
                {
                    Console.WriteLine("Nincs több alkalmazott.");
                    break;
                }

                Console.Clear();
                Console.WriteLine("-- ALKALMAZOTTAK LISTÁJA --");
                
                Console.WriteLine($"{"Id",-5} {"Role",-10} {"Firstname",-15} {"Lastname",-15} {"Birthdate",-12} {"Email",-30} {"Phone",-15} {"Address",-20} {"City",-15} {"Zip",-10} {"Country",-15} {"Salary",-10} {"Title", -10}");
                Console.WriteLine(new string('-', 182));
                foreach (var employee in employees)
                {
                    if (employee is Boss boss)
                    {
                        Console.WriteLine($"{boss.Id,-5} {boss.RoleDiscriminator,-10:G} {boss.Firstname,-15} {boss.Lastname,-15} {boss.Birthdate,-12} {boss.Email,-30} {boss.Phone,-15} {boss.Address,-20} {boss.City,-15} {boss.Zip,-10} {boss.Country,-15} {boss.Salary,-10} {boss.Title,-10}");
                    }
                    else if (employee is Manager manager)
                    {
                        Console.WriteLine($"{manager.Id,-5} {manager.RoleDiscriminator,-10:G} {manager.Firstname,-15} {manager.Lastname,-15} {manager.Birthdate,-12} {manager.Email,-30} {manager.Phone,-15} {manager.Address,-20} {manager.City,-15} {manager.Zip,-10} {manager.Country,-15} {manager.Salary,-10} {manager.Title, -10}");
                    }
                    else
                    {
                        Console.WriteLine(employee);
                    }
                }
                
                Console.WriteLine($"\nOldal: {page + 1}");
                Console.WriteLine("Nyomjon Enter-t a következő oldalhoz, 'p' az előző oldalhoz vagy 'q' a kilépéshez.");

                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Q)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.P && page > 0)
                {
                    page--;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    page++;
                }
            }
        }

        /// <summary>
        /// Reads input from the console and validates it.
        /// If the input is invalid, an error message is displayed and the user is prompted to enter the input again.
        /// </summary>
        private static string ReadInput(string prompt, Func<string, bool> validate, string errorMessage)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine() ?? string.Empty;
                if (!validate(input))
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!validate(input));

            return input;
        }

        /// <summary>
        /// Reads optional input from the console and validates it.
        /// If the input is invalid, an error message is displayed and the user is prompted to enter the input again.
        /// If the input is empty, it is considered valid.
        /// </summary>
        private static string? ReadOptionalInput(string prompt, Func<string, bool> validate, string errorMessage)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || validate(input))
                {
                    break;
                }

                Console.WriteLine(errorMessage);
            } while (true);

            return input;
        }
        
        private static string ReadValidFirstname()
        {
            return ReadInput(
                "Vezetéknév: ",
                Validate.IsValidFirstname,
                "Nem megfelelő vezetéknév. (max. 64 karakter)");
        }

        private static string ReadValidLastname()
        {
            return ReadInput(
                "Keresztnév: ",
                Validate.IsValidFirstname,
                "Nem megfelelő keresztnév. (max. 64 karakter)");
        }

        private static string ReadValidPhonenumber()
        {
            return ReadInput(
                "Telefonszám: ",
                Validate.IsValidPhoneNumber,
                "Nem megfelelő telefonszám. (max. 16 karakter)");
        }

        private static string ReadValidAddress()
        {
            return ReadInput(
                "Cím: ",
                Validate.IsValidAddress,
                "Nem megfelelő cím. (max. 128 karakter)");
        }

        private static string ReadValidCity()
        {
            return ReadInput("Város: ",
                Validate.IsValidCity,
                "Nem megfelelő városnév. (max. 64 karakter)");
        }

        private static string ReadValidZip()
        {
            return ReadInput("Irányítószám: ",
                Validate.IsValidZipCode,
                "Nem megfelelő irányítószám. (max. 16 karakter)");
        }

        private static string ReadValidCountry()
        {
            return ReadInput(
                "Ország: ",
                Validate.IsValidCountry,
                "Nem megfelelő ország név. (max. 64 karakter)");
        }

        private static string ReadValidEmail()
        {
            return ReadInput(
                "E-Mail: ",
                Validate.IsValidEmail,
                "Az e-mail cím nem megfelelő. (pelda@email.com)"
            );
        }

        private static DateOnly? ReadValidBirthdate(string? current = null)
        {
            DateOnly? birthdate = null;
            do
            {
                Console.Write(current != null
                    ? $"Születési dátum (éééé-hh-nn)[{current}]: "
                    : $"Születési dátum (éééé-hh-nn): ");

                var input = Console.ReadLine();
                if (DateOnly.TryParse(input, out var parsedDate) && Validate.IsValidBirthdate(parsedDate))
                {
                    birthdate = parsedDate;
                    break;
                }

                if (string.IsNullOrEmpty(input)) break;
                Console.WriteLine("Nem megfelelő dátum.");
            } while (true);

            return birthdate;
        }

        private static int ReadValidSalary(string? current = null)
        {
            int salary;
            do
            {
                Console.Write(current != null
                    ? $"Fizetés[{current}]: "
                    : $"Fizetés: ");
                
                var input = Console.ReadLine();
                if (int.TryParse(input, out salary) && Validate.IsValidSalary(salary))
                {
                    break;
                }

                Console.WriteLine("Nem megfelelő fizetés (csak pozitív szám).");
            } while (true);

            return salary;
        }

        private static EmployeeRoleDiscriminator ReadValidRole()
        {
            EmployeeRoleDiscriminator roleDiscriminator;
            do
            {
                var roles = Enum.GetValues<EmployeeRoleDiscriminator>()
                    .Select(x => $"({x:G}:{x:D})")
                    .ToList();
                Console.WriteLine("Szerepkörök: " + string.Join(", ", roles));
                var input = Console.ReadLine();
                if (Enum.TryParse(input, out roleDiscriminator) && Enum.IsDefined(typeof(EmployeeRoleDiscriminator), roleDiscriminator))
                {
                    break;
                }

                Console.WriteLine("Nem megfelelő szerepkör.");
            } while (true);

            return roleDiscriminator;
        }
    }
}
