using Microsoft.AspNetCore.Mvc;
// When we will write API we have to inherit from ControllerBase instead of Controller because ControllerBase is used for API and Controller is used for MVC
[ApiController] // This attribute indicates that this class is an API controller. It enables some API-specific behaviors, such as automatic model validation and binding source inference. It isn't strictly necessary to use this attribute, but it provides some useful features for API controllers. It's like a shortcut to enable certain behaviors that are common for API controllers, so it's generally recommended to use it when creating API controllers in ASP.NET Core.
[Route("todos")] // This attribute defines the route for this controller. In this case, it means that any HTTP requests to the /todos endpoint will be handled by this controller. The [Route] attribute can be used to specify the base route for the controller, and you can also use it on individual action methods to define more specific routes if needed.
public class TodoController : ControllerBase
{
    private readonly List<string> todos = new List<string>
    {
        "1. First Todo"
    };
    [HttpGet] // This attribute indicates that this action method should handle HTTP GET requests. When a GET request is made to the /todos endpoint, this method will be invoked to handle the request and return the list of todos.
    public IActionResult GetTodos() // Here, IActionResult is an interface from Microsoft.AspNetCore.Mvc that represents the result of an action method. It allows us to return different types of responses (like Ok, NotFound, BadRequest, etc.) from our API endpoint.
    {
        return Ok(todos); // Here, Ok() is a helper method from ControllerBase that returns a 200 OK response with the provided data (todos in this case).
    }
    [HttpPost("/create")] // This attribute indicates that this action method should handle HTTP POST requests to the /todos/create endpoint. When a POST request is made to /todos/create, this method will be invoked to handle the request and create a new todo item. 
    public IActionResult CreateTodo(string title)
    {
        if(string.IsNullOrEmpty(title))
        {
            return BadRequest("Title cannot be empty."); // This returns a 400 Bad Request response if the title is null or empty, along with a message indicating the issue.
        }
        todos.Add(title); // This adds the new todo item (the title) to the list of todos.
        return Created($"/todos/{todos.Count -1}", title); // This returns a 201 Created response, indicating that a new resource has been created. The first parameter is the location of the newly created resource (in this case, we are using the count of todos to create a unique URL), and the second parameter is the content of the response (the title of the new todo).
    }
}
class Todo
{
    public string Title {get; set;} = string.Empty;
}
