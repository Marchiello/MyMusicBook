using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MyMusicBook.Domain.Enums;
using MySqlConnector;

namespace MyMusicBook.Infraestructure.Migrations;

public class DatabaseMigration
{
    public static void Migrate(DatabaseType databaseType, string connectionString, IServiceProvider serviceProvider)
    {
        if (databaseType == DatabaseType.MySql)
            EnsureDatabaseCreated_MySql(connectionString);
        else
            EnsureDatabaseCreated_SqlServer(connectionString);

        MigrationDatabase(serviceProvider);
    }

    public static void EnsureDatabaseCreated_MySql(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

        var databaseName = connectionStringBuilder.Database;
        // recupera o database

        connectionStringBuilder.Remove("Database");
        // Remove o database pois essa tenta acessar o Database especificado na connection string e, caso
        // não exista, dará erro, pois ainda não à criamos, como faremos ali na frente.

        using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

// -----------------------------------------------------------------------------------

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var records = dbConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);

        if(records.Any() == false)
        {
            dbConnection.Execute($"CREATE DATABASE {databaseName}");
        }

    }

    public static void EnsureDatabaseCreated_SqlServer(string connectionString)
    {
        // mudamos aqui
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        // aqui também
        var databaseName = connectionStringBuilder.InitialCatalog;

        connectionStringBuilder.Remove("Database");

        // aqui tb
        using var dbConnection = new SqlConnection(connectionString);

        // -----------------------------------------------------------------------------------

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        // mudamos aqui
        var records = dbConnection.Query("SELECT * FROM sys.databases WHERE name = @name", parameters);

        if (records.Any() == false)
        {
            dbConnection.Execute($"CREATE DATABASE {databaseName}");
        }
    }

    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();

        runner.MigrateUp();
    }
}