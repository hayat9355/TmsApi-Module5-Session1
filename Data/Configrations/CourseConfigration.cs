using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        //  TABLE 
        builder.ToTable("Courses");

        //  PRIMARY KEY 
        builder.HasKey(c => c.Id);

        //  PROPERTIES 
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20);   // "CS-101", "MAT-101" → short codes

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);  // Full course name

        builder.Property(c => c.Capacity)
            .IsRequired();

        //  UNIQUE INDEX (Natural Key) 
        builder.HasIndex(c => c.Code)
            .IsUnique();
        // No two courses can share the same Code
        // e.g. you cannot have two "CS-101" courses
    }
}