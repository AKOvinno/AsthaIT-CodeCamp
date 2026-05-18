public class Enrollment
{
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }
    public Guid CourseId { get; set; }
    public Course? Course { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
}
// In case of many-to-many relationship, we need a middle entity to represent the relationship between the two entities. In this case, we have a many-to-many relationship between Student and Course, so we need an Enrollment entity to represent the relationship between them. The Enrollment entity will have a foreign key to both the Student and Course entities, allowing us to track which students are enrolled in which courses. Also called pivot table or join table. We can also establish without pivot table but it is not recommended as it can lead to data integrity issues and can make it difficult to query/control the data. Then EF Core will create a hidden table to represent the relationship between the two entities, but it will not have any additional properties or fields to track the relationship. This can lead to issues when we want to query the data or when we want to add additional properties to the relationship in the future. Therefore, it is recommended to use a pivot table to represent many-to-many relationships in EF Core.