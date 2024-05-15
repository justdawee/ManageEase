namespace ManageEase.Services.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password, string salt);
}