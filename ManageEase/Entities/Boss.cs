using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManageEase.Entities;

public class Boss : Employee
{
    public string Title { get; set; }
}

public class BossConfiguration : IEntityTypeConfiguration<Boss>
{
    public void Configure(EntityTypeBuilder<Boss> builder)
    {
        builder.Property(u=>u.Title).HasMaxLength(64);
    }
}