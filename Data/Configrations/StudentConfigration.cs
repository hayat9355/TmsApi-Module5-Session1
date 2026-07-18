using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // TABLE 
        builder.ToTable("Students");

        //  PRIMARY KEY 
        builder.HasKey(s => s.Id);
        // EF Core would guess this from "Id" name, but explicit is better

        // PROPERTIES 
        builder.Property(s => s.Name)
            .IsRequired()        // NOT NULL in PostgreSQL
            .HasMaxLength(100);  // VARCHAR(100) — prevents huge strings

        builder.Property(s => s.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);   // "TMS-2026-0001" → 13 chars, 20 is safe

        builder.Property(s => s.GPA)
            .HasPrecision(4, 2);
        // 4 total digits, 2 after decimal → allows 0.00 to 99.99
        // GPA max is 4.0 so this is more than enough

        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true); // Default at database level

        //  UNIQUE INDEX (Natural Key) 
        builder.HasIndex(s => s.RegistrationNumber)
            .IsUnique();
        // PostgreSQL will REFUSE inserting two students with same reg number
        // This constraint lives in the DATABASE — not just in your C# code
    }
}