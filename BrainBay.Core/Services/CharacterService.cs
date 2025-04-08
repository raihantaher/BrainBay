using Microsoft.EntityFrameworkCore;
using BrainBay.Core.Data;
using BrainBay.Core.Models;

namespace BrainBay.Core.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly BrainBayContext _context;

        public CharacterService(BrainBayContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Character>> GetAllCharactersAsync()
        {
            return await _context.Characters.ToListAsync();
        }

        public async Task<Character?> GetCharacterByIdAsync(int id)
        {
            return await _context.Characters.FindAsync(id);
        }

        public async Task<IEnumerable<Character>> GetCharactersByOriginAsync(string origin)
        {
            return await _context.Characters
                .Where(c => c.Origin != null && c.Origin.Name == origin)
                .ToListAsync();
        }

        public async Task SaveCharactersAsync(IEnumerable<Character> characters)
        {
            await _context.Characters.AddRangeAsync(characters);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCharactersAsync()
        {
            _context.Characters.RemoveRange(_context.Characters);
            await _context.SaveChangesAsync();
        }

        public async Task<Character> CreateCharacterAsync(Character character)
        {
            var maxId = await _context.Characters.MaxAsync(c => (int?)c.Id) ?? 0;
            
            character.Id = maxId + 1;
            
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            
            return character;
        }
    }
} 