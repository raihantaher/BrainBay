using System.Net.Http.Json;
using BrainBay.Core.Models;
using BrainBay.Blazor.Models;

namespace BrainBay.Blazor.Services
{
    public class CharacterApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/characters";

        public CharacterApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Character>> GetCharactersAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Character>>(BaseUrl) 
                ?? Enumerable.Empty<Character>();
        }

        public async Task<IEnumerable<Character>> GetCharactersByOriginAsync(string origin)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Character>>($"{BaseUrl}/origin/{origin}") 
                ?? Enumerable.Empty<Character>();
        }

        public async Task<Character> CreateCharacterAsync(CharacterModel model)
        {
            var character = new Character
            {
                Name = model.Name,
                Status = model.Status,
                Species = model.Species,
                Origin = new Core.Models.Origin { Name = model.Origin }
            };

            var response = await _httpClient.PostAsJsonAsync(BaseUrl, character);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Character>() 
                ?? throw new Exception("Failed to create character");
        }
    }
} 