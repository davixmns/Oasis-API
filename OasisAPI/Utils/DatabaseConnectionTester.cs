using Microsoft.EntityFrameworkCore;
using OasisAPI.Context;

namespace OasisAPI.Utils;

public static class DatabaseConnectionTester
{
    public static void TestConnection(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OasisDbContext>();
        
        try
        {
            db.Database.OpenConnection();
            db.Database.CloseConnection();
            Console.WriteLine("Conexão com o banco de dados estabelecida com sucesso!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Falha ao conectar com o banco de dados: " + e.Message);
            throw;
        }
    }
}