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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);

            using var serviceProvider = services.BuildServiceProvider();
            var characterService = serviceProvider.GetRequiredService<ICharacterService>();
            var apiService = serviceProvider.GetRequiredService<IRickAndMortyApiService>();

            try
            {
                System.Console.WriteLine("Fetching characters from Rick and Morty API...");
                var aliveCharacters = await apiService.GetAliveCharactersAsync();
                
                System.Console.WriteLine($"Found {aliveCharacters.Count()} alive characters.");
                
                await characterService.ClearCharactersAsync();
                await characterService.SaveCharactersAsync(aliveCharacters);
                
                System.Console.WriteLine("Characters saved successfully!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BrainBayContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpClient<IRickAndMortyApiService, RickAndMortyApiService>();
            services.AddScoped<ICharacterService, CharacterService>();
        }
    }
}
