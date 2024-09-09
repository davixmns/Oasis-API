using Microsoft.EntityFrameworkCore;
using OasisAPI.Context;

namespace OasisAPI.Utils;

public static class DatabaseConnectionTester
{
    public static void TestConnection(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OasisDbContext>();
        db.Database.OpenConnection();
        db.Database.CloseConnection();
        Console.WriteLine("Database connected!");
    }
}