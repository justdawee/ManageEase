using ManageEase.Entities;

namespace ManageEase.Inputs;

public record AddNewEmployeeInput
{
    public EmployeeRoleDiscriminator RoleDiscriminator { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateOnly? Birthdate { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public int Salary { get; set; }
}