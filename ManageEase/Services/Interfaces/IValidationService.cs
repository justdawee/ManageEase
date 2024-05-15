namespace ManageEase.Services.Interfaces;

public interface IValidationService
{
    bool IsValidEmail(string email);
    bool IsValidUsername(string username);
    bool IsValidPassword(string password);
    bool IsValidFirstname(string firstname);
    bool IsValidLastName(string lastname);
    bool IsValidPhoneNumber(string phoneNumber);
    bool IsValidAddress(string address);
    bool IsValidCity(string city);
    bool IsValidZipCode(string zipCode);
    bool IsValidCountry(string country);
    bool IsValidBirthdate(DateOnly birthdate);
    bool IsValidSalary(int salary);
}