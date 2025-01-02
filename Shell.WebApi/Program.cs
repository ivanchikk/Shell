using Shell.Application;
using Shell.Application.Common.Mappings;
using Shell.Persistence;
using Shell.WebApi.Middleware;

namespace Shell.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddPresentation()
                .AddApplication()
                .AddPersistence(builder.Configuration)
                .AddAutoMapper(cfg =>
                {
                    cfg.AddProfile(new AssemblyMappingProfile(typeof(DependencyInjection).Assembly));
                    cfg.AddProfile(new AssemblyMappingProfile(typeof(Application.DependencyInjection).Assembly));
                });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<ShellDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception e)
                {
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("DB INITIALIZE EXCEPTION");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine("-----------------------");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomExceptionHandler();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.MapControllers();
            app.MapGet("/", () => "root");
            app.MapGet("/api", () => "api");

            try
            {
                app.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }
    }
}
