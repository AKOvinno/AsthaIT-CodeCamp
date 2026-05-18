using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class QueryLoggingInterceptor : DbCommandInterceptor
{
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken ct = default)
    {
        Console.WriteLine("\n------Reader Executing------");
        return base.ReaderExecutingAsync(command, eventData, result, ct);
    }
    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken ct = default)
    {
        Console.WriteLine("\n------Reader Executed------");
        return base.ReaderExecutedAsync(command, eventData, result, ct);
    }
}
// Both ReaderExecutingAsync and ReaderExecutedAsync are used to log the execution of database commands. The first method is called before the command SaveChangesAsync is executed, while the second method is called after the command SaveChangesAsync has been executed. This allows you to log information about the command being executed and its results, which can be useful for debugging and monitoring database interactions in your application.