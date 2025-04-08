using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BrainBay.Core.Services;
using BrainBay.Core.Models;

namespace BrainBay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "Characters";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public CharactersController(ICharacterService characterService, IMemoryCache cache)
        {
            _characterService = characterService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            if (_cache.TryGetValue(CacheKey, out IEnumerable<Character> cachedCharacters))
            {
                Response.Headers.Add("from-database", "true");
                return Ok(cachedCharacters);
            }

            var characters = await _characterService.GetAllCharactersAsync();
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuration);
            
            _cache.Set(CacheKey, characters, cacheOptions);
            
            Response.Headers.Add("from-database", "false");
            return Ok(characters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(int id)
        {
            var character = await _characterService.GetCharacterByIdAsync(id);
            
            if (character == null)
            {
                return NotFound();
            }

            return Ok(character);
        }

        [HttpGet("origin/{origin}")]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharactersByOrigin(string origin)
        {
            var characters = await _characterService.GetCharactersByOriginAsync(origin);
            return Ok(characters);
        }
    }
} 