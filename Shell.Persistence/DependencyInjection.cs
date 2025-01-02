using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shell.Application.Common.Interfaces;
using Shell.Persistence.Services;

namespace Shell.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ShellDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IShellDbContext>(provider => provider.GetService<ShellDbContext>());
            services.AddSingleton<IFileSystemService, FileSystemService>();

            return services;
        }
    }
}
