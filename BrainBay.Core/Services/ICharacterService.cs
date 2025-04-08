using BrainBay.Core.Models;

namespace BrainBay.Core.Services
{
    public interface ICharacterService
    {
        Task<IEnumerable<Character>> GetAllCharactersAsync();
        Task<Character?> GetCharacterByIdAsync(int id);
        Task<IEnumerable<Character>> GetCharactersByOriginAsync(string origin);
        Task SaveCharactersAsync(IEnumerable<Character> characters);
        Task ClearCharactersAsync();
        Task<Character> CreateCharacterAsync(Character character);
    }
} 