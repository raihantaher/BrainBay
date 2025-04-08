using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BrainBay.Core.Models;

namespace BrainBay.Core.Services
{
    public class RickAndMortyApiService : IRickAndMortyApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://rickandmortyapi.com/api";

        public RickAndMortyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Character>> GetAliveCharactersAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>($"{BaseUrl}/character/");
                return response?.Results?.Where(c => c.Status.ToLower() == "alive") ?? Enumerable.Empty<Character>();
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                throw new Exception("Failed to fetch characters from Rick and Morty API", ex);
            }
        }

        private class ApiResponse
        {
            public Character[] Results { get; set; }
        }
    }
} 