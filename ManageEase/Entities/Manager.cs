using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManageEase.Entities;

public class Manager : Employee
{
    public string Title { get; set; }
}

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.Property(u=>u.Title).HasMaxLength(64);
    }
}