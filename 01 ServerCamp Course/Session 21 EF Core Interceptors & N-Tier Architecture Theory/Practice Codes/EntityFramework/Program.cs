using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Application and database connection for two lines
var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=root;Database=codecampdb";
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

app.MapPost("/user", async(AppDbContext context, UserRequest request) =>
{
    var user = new User
    {
        Name = request.Name,
        Email = request.Email
    };
    await context.Users.AddAsync(user);
    await context.SaveChangesAsync();
});
app.MapPost("/one-to-one", async(AppDbContext context, UserRequestWithAddress request) =>
{
    var user = new User
    {
        Name = request.Name,
        Email = request.Email,
        UserProfile = new UserProfile
        {
            Address = request.Address
        }
    };
    Console.WriteLine($"User Name: {user.Name}, Email: {user.Email}, Address: {user.UserProfile.Address}");
    await context.Users.AddAsync(user); // This will add the user and the user profile in the same transaction. Unit of Work hold the transaction but not commit it. Change Tracker will track the changes and when we call SaveChangesAsync, it will commit the transaction and save the changes to the database.
    await context.SaveChangesAsync(); // This will save the user and the user profile to the database, ensuring that the relationship is maintained correctly. SaveChangesAsync follows the Unit of Work pattern, which means that it will save all changes made in the context to the database in a single transaction. This ensures that if any part of the operation fails, the entire transaction will be rolled back, maintaining data integrity.
});
app.MapPost("/one-to-many", async(AppDbContext context, UserRequestWithOrders request) =>
{
    var user = new User
    {
        Name = request.Name,
        Email = request.Email,
        UserProfile = new UserProfile
        {
            Address = request.Address
        },
        UserOrders = new List<Order>()
    };
    foreach (var order in request.Orders)
    {
        user.UserOrders.Add(new Order
        {
            Total = order.Total
        });
    }
    await context.Users.AddAsync(user);
    await context.SaveChangesAsync();
});
app.MapPost("/many-to-many", async(AppDbContext context) =>
{
    var student = new Student
    {
        Name = "John Doe"
    };
    var course = new Course
    {
        Title = "Introduction to EF Core"
    };
    var enrollment = new Enrollment
    {
        Student = student,
        Course = course
    };
    await context.Enrollments.AddAsync(enrollment);
    await context.SaveChangesAsync();
});
app.MapGet("/students-courses", (AppDbContext context) =>{
    
})
app.Run();