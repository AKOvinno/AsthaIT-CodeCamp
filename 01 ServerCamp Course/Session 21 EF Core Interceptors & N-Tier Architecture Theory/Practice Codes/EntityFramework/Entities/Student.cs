public class Student
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<Enrollment>? Enrollments { get; set; } // When we will join any table with enrollment table then the property of enrollment will be added to the table.
}