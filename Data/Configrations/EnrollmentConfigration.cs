using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        //  TABLE 
        builder.ToTable("Enrollments");

        // PRIMARY KEY 
        builder.HasKey(e => e.Id);

        // PROPERTIES 
        builder.Property(e => e.Grade)
            .HasPrecision(4, 2);
        // Nullable decimal — no .IsRequired() because Grade CAN be null
        // A currently enrolled student may not have a grade yet

        builder.Property(e => e.EnrolledAt)
            .IsRequired();

        // RELATIONSHIPS (Exercise 5) 

        // Enrollment → Student (Many enrollments belong to ONE student)
        builder.HasOne(e => e.Student)       // this enrollment has ONE student
            .WithMany(s => s.Enrollments)    // that student has MANY enrollments
            .HasForeignKey(e => e.StudentId) // FK column is StudentId
            .OnDelete(DeleteBehavior.Restrict);
        // Restrict: if you try to DELETE a Student who has enrollments
        // → PostgreSQL throws an ERROR and blocks the delete
        // WHY: silently deleting a student's enrollment records
        // would destroy academic history. Fail loudly instead.

        // Enrollment → Course (Many enrollments belong to ONE course)
        builder.HasOne(e => e.Course)        // this enrollment has ONE course
            .WithMany(c => c.Enrollments)    // that course has MANY enrollments
            .HasForeignKey(e => e.CourseId)  // FK column is CourseId
            .OnDelete(DeleteBehavior.Restrict);
        // Same reason: deleting a course that has enrollments
        // should be blocked, not cascade-delete student records
    }
}