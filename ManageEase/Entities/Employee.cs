using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManageEase.Entities;

public class Employee
{
    public long Id { get; set; }
    public EmployeeRoleDiscriminator RoleDiscriminator { get; protected set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required DateOnly? Birthdate { get; set; }
    public required string Email { get; set; }
    public required string? Phone { get; set; }
    public required string? Address { get; set; }
    public required string? City { get; set; }
    public required string? Zip { get; set; }
    public required string? Country { get; set; }
    public required int Salary { get; set; }

    public override string ToString()
    {
        return $"{Id,-5} {RoleDiscriminator.ToString("G"),-10} {Firstname,-15} {Lastname,-15} {Birthdate,-12} {Email,-30} {Phone,-15} {Address,-20} {City,-15} {Zip,-10} {Country,-15} {Salary,-10}";
    }
}

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u=>u.Email).HasMaxLength(256);
        builder.Property(u=>u.Firstname).HasMaxLength(64);
        builder.Property(u=>u.Lastname).HasMaxLength(64);
        builder.Property(u=>u.Phone).HasMaxLength(16);
        builder.Property(u=>u.Address).HasMaxLength(128);
        builder.Property(u=>u.City).HasMaxLength(64);
        builder.Property(u=>u.Zip).HasMaxLength(16);
        builder.Property(u=>u.Country).HasMaxLength(64);

        builder.UseTphMappingStrategy();
        builder.HasDiscriminator<EmployeeRoleDiscriminator>(u => u.RoleDiscriminator)
            .HasValue<Employee>(EmployeeRoleDiscriminator.Employee)
            .HasValue<Manager>(EmployeeRoleDiscriminator.Manager)
            .HasValue<Boss>(EmployeeRoleDiscriminator.Boss);
        builder.HasIndex(u=>u.RoleDiscriminator);
    }
}