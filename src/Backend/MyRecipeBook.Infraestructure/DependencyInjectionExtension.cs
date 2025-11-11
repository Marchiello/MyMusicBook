using FirebirdSql.Data.Services;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyMusicBook.Domain.Enums;
using MyMusicBook.Domain.Repositories;
using MyMusicBook.Domain.Repositories.User;
using MyMusicBook.Infraestructure.DataAccess;
using MyMusicBook.Infraestructure.DataAccess.Repositories;
using MyMusicBook.Infraestructure.Extensions;
using System.Reflection;

namespace MyMusicBook.Infraestructure;
public static class DependencyInjectionExtension
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);

        // Olha se o valor "InMemoryTest" no appsettings é true. Se for, significa que estamos
        // em um teste de integração.
        if (configuration.IsUnitTestEnvironment())
            return;

        var databaseType = configuration.DatabaseType();

        //                     (DatabaseType) está fazendo um Parse
        //var databaseTypeEnum = (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);
        //  A exclamação em databaseType serve apenas para "desativar" o warn.

        if (databaseType == DatabaseType.MySql)
        {
            AddDbContext_MySqlServer(services, configuration);
            AddFluentMigrator_MySql(services, configuration);
        }
        else
        {
            AddDbContext_SqlServer(services, configuration);
            AddFluentMigrator_SqlServer(services, configuration);

        }

    }

    private static void AddDbContext_MySqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 43));

        services.AddDbContext<MyMusicBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString, serverVersion);
        });
    }
    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<MyMusicBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        });
    }
    private static void AddRepositories(IServiceCollection services)
    {
        // Com o .AddScoped, todos os componentes que requisitam esse serviço durante uma mesma request
        // receberão a mesma instância, que será descartada após a conclusão da request.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Recebendo IUserWriteOnlyRepository, Devo retornar um UserRepository
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddMySql5()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("MyMusicBook.Infraestructure")).For.All();
        });
    }

    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddSqlServer()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("MyMusicBook.Infraestructure")).For.All();
        });
    }
}
