using Npgsql;
public class TodoService
{
    public async Task AddTodoAsync(string title, NpgsqlConnection connection)
    {
        // We are using parameterized queries instead of string interpolation because we want to prevent SQL injection
        var query = "INSERT INTO todos (title) VALUES (@title)";
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("title", title);
        
        await cmd.ExecuteNonQueryAsync();
    }
    public async Task<string?> GetTodoTitleAsync(int id, NpgsqlConnection connection)
    {
        var query = "SELECT title FROM todos WHERE id = @id";
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("id", id);
        
        // ExecuteReaderAsync allows us to read the rows returned by the database
        using var reader = await cmd.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            // Get the string from the first column (index 0)
            return reader.GetString(0);
        }

        return null; // Return null if no todo was found with that ID
    }
    public async Task<bool> DeleteTodoAsync(int id, NpgsqlConnection connection)
    {
        var query = "DELETE FROM todos WHERE id = @id";
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("id", id);
        // ExecuteNonQueryAsync returns the number of rows affected
        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        
        // Return true if at least one row was deleted
        return rowsAffected > 0;
    }
    public async Task<bool> UpdateTodoAsync(int id, string title, NpgsqlConnection connection)
    {
        var query = "UPDATE todos SET title = @title WHERE id = @id";
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("title", title);
        
        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}