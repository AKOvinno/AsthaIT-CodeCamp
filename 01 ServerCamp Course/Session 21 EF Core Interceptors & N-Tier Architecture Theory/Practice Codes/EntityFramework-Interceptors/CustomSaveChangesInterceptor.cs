using Microsoft.EntityFrameworkCore.Diagnostics;
public class CustomSaveChangesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken ct = default
    )
    {
        Console.WriteLine("We are saving the change");
        return await base.SavingChangesAsync(eventData, result, ct);
    }
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken ct = default
    )
    {
        Console.WriteLine("The changes are saved");
        return await base.SavedChangesAsync(eventData, result, ct);
    }

}