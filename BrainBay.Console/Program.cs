using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BrainBay.Core.Data;
using BrainBay.Core.Services;

namespace BrainBay.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var services = ConfigureServices(configuration);

            using var serviceProvider = services.BuildServiceProvider();
            var syncService = serviceProvider.GetRequiredService<ICharacterSyncService>();

            try
            {
                System.Console.WriteLine("Starting character synchronization...");
                await syncService.SyncCharactersFromApiAsync();
                System.Console.WriteLine("Characters synchronized successfully!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        private static IServiceCollection ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddDbContext<BrainBayContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpClient<IRickAndMortyApiService, RickAndMortyApiService>(client =>
            {
                client.BaseAddress = new Uri("https://rickandmortyapi.com/api/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<ICharacterSyncService, CharacterSyncService>();

            return services;
        }
    }
}
