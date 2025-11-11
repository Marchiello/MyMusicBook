using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyMusicBook.Application.Services.AutoMapper;
using MyMusicBook.Application.Services.Criptography;
using MyMusicBook.Application.UseCases.User.Register;

namespace MyMusicBook.Application;
public static class DependencyInjectionExtension
{
    // Para usarmos as variaveis de ambiente do appsettings.Development.json devemos instalar a lib
    // Microsft.Extensions.Configuration no NuGet e receber o IConfiguration na função.
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
        AddPasswordEncripter(services, configuration);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {

        // Ambos funcionam perfeitamente, mas o não-comentado é menos verboso.

        //var autoMapper = new AutoMapper.MapperConfiguration(options =>
        //{
        //    options.AddProfile(new AutoMapping());
        //}).CreateMapper();

        //services.AddScoped(option => autoMapper);

        services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }

    private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
    {
        // Passamos o "Path" da env definida no appsetings.Development.json como argumento do .GetSection()
        //var additionalKey = configuration.GetSection("Settings:Password:AdditionalKey").Value;

        // instale o Microsoft.Extensions.Configuration.Binder no NuGet para evitar necessidade de fazer
        // cast, já que independentemente se a env é uma string, inteiro, bool, o .GetSection().Value SEMPRE
        // retorna uma string. 

        // Dentro do GetValue<> se insere o tipo de dado que se deseja "converter".
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

        services.AddScoped(option => new PasswordEncripter(additionalKey!));
        // a exclamação no final é para desativar o warn.
    }

}