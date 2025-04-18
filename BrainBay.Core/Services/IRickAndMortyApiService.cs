using BrainBay.Core.Models;

namespace BrainBay.Core.Services
{
    public interface IRickAndMortyApiService
    {
        Task<IEnumerable<Character>> GetAliveCharactersAsync();
    }
} 