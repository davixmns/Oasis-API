using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OasisAPI.Infra.Context;

namespace OasisAPI.Infra.Utils;

public static class DatabaseConnectionTester
{
    public static void TestConnection(IServiceProvider services)
    {
        try
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OasisDbContext>();
            db.Database.OpenConnection();
            db.Database.CloseConnection();
            Console.WriteLine("Database connected!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Database connection failed!");
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }
}