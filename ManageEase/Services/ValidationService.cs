using ManageEase.Services.Interfaces;
using ManageEase.Utilities;

namespace ManageEase.Services
{
    public class ValidationService : IValidationService
    {
        // Validates if the provided email is valid by checking its length and matching it against the email regex pattern
        public bool IsValidEmail(string email)
        {
            return email.Length <= 256 && Regexes.EmailGeneratedRegex().IsMatch(email);
        }

        // Validates if the provided username is valid by checking its length and matching it against the username regex pattern
        public bool IsValidUsername(string username)
        {
            return username.Length <= 32 && Regexes.UsernameGeneratedRegex().IsMatch(username);
        }

        // Validates if the provided password is valid by checking if its length is between 8 and 256 characters
        public bool IsValidPassword(string password)
        {
            return password.Length is <= 256 and >= 8;
        }

        // Validates if the provided first name is valid by checking its length and matching it against the capitalized word regex pattern
        public bool IsValidFirstname(string firstname)
        {
            return firstname.Length <= 64 && Regexes.CapitalizedWordRegex().IsMatch(firstname);
        }

        // Validates if the provided last name is valid by checking its length and matching it against the capitalized word regex pattern
        public bool IsValidLastName(string lastname)
        {
            return lastname.Length <= 64 && Regexes.CapitalizedWordRegex().IsMatch(lastname);
        }

        // Validates if the provided phone number is valid by checking its length and matching it against the phone number regex pattern
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length <= 16 && Regexes.PhoneNumberGeneratedRegex().IsMatch(phoneNumber);
        }

        // Validates if the provided address is valid by checking its length and matching it against the address regex pattern
        public bool IsValidAddress(string address)
        {
            return address.Length <= 128 && Regexes.AddressRegex.IsMatch(address);
        }

        // Validates if the provided city name is valid by checking its length and matching it against the only character regex pattern
        public bool IsValidCity(string city)
        {
            return city.Length <= 64 && Regexes.OnlyCharacterRegex().IsMatch(city);
        }

        // Validates if the provided zip code is valid by checking its length and matching it against the only number regex pattern
        public bool IsValidZipCode(string zipCode)
        {
            return zipCode.Length <= 16 && Regexes.OnlyNumberRegex().IsMatch(zipCode);
        }

        // Validates if the provided country name is valid by checking its length and matching it against the only character regex pattern
        public bool IsValidCountry(string country)
        {
            return country.Length <= 64 && Regexes.OnlyCharacterRegex().IsMatch(country);
        }

        // Validates if the provided birthdate is valid by ensuring it is not in the future
        public bool IsValidBirthdate(DateOnly birthdate)
        {
            return birthdate <= DateOnly.FromDateTime(DateTime.Now);
        }

        // Validates if the provided salary is valid by ensuring it is not negative
        public bool IsValidSalary(int salary)
        {
            return salary >= 0;
        }
    }
}
