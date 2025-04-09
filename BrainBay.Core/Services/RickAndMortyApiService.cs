using System.Net.Http.Json;
using BrainBay.Core.Models;

namespace BrainBay.Core.Services
{
    public class RickAndMortyApiService : IRickAndMortyApiService
    {
        private readonly HttpClient _httpClient;

        public RickAndMortyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Character>> GetAliveCharactersAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>("character/");
                return response?.Results?.Where(c => c.Status.ToLower() == "alive") ?? Enumerable.Empty<Character>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch characters from Rick and Morty API", ex);
            }
        }

        private class ApiResponse
        {
            public Character[] Results { get; set; }
        }
    }
} 