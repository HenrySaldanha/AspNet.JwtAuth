using Application.IServices;
using Application.Options;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.IRepository;
using Repository.Repository;

namespace DependencyInjection;
public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserService, UserService>();
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepositoryReader, UserRepositoryReader>();
        services.AddTransient<IUserRepositoryWriter, UserRepositoryWriter>();
    }

    public static void RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenSecretOptions>(configuration.GetSection("TokenSecret"));
    }
}