using Microsoft.Extensions.Configuration;
using MyMusicBook.Domain.Enums;

namespace MyMusicBook.Infraestructure.Extensions;
public static class ConfigurationExtension
{
    // Método de extensão
    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }

    public static DatabaseType DatabaseType(this IConfiguration configuration)
    {
        var databaseType = configuration.GetConnectionString("DatabaseType");

        return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType);
    }
    public static string ConnectionString(this IConfiguration configuration)
    {
        var databaseType = configuration.DatabaseType();

        if (databaseType == Domain.Enums.DatabaseType.MySql)
            return configuration.GetConnectionString("ConnectionMySQLServer")!;    
        else
            return configuration.GetConnectionString("ConnectionSQLServer")!;
    }
}
