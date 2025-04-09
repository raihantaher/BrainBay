using Microsoft.EntityFrameworkCore;
using BrainBay.Core.Data;
using BrainBay.Core.Models;
using BrainBay.Core.Services;

namespace BrainBay.Tests.Services
{
    public class CharacterServiceIntegrationTests : IDisposable
    {
        private readonly BrainBayContext _context;
        private readonly CharacterService _service;

        public CharacterServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<BrainBayContext>()
                .UseInMemoryDatabase(databaseName: $"Test_{Guid.NewGuid()}")
                .Options;

            _context = new BrainBayContext(options);
            _service = new CharacterService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task CreateAndRetrieveCharacter_ShouldWorkCorrectly()
        {
            // Arrange
            var character = new Character
            {
                Name = "Test Character",
                Status = "Alive",
                Species = "Human",
                Origin = new Origin { Name = "Earth" }
            };

            // Act
            var createdCharacter = await _service.CreateCharacterAsync(character);
            var retrievedCharacter = await _service.GetCharacterByIdAsync(createdCharacter.Id);

            // Assert
            Assert.NotNull(retrievedCharacter);
            Assert.Equal(character.Name, retrievedCharacter.Name);
            Assert.Equal(character.Status, retrievedCharacter.Status);
            Assert.Equal(character.Species, retrievedCharacter.Species);
            Assert.Equal(character.Origin.Name, retrievedCharacter.Origin?.Name);
        }

        [Fact]
        public async Task GetCharactersByOrigin_ShouldReturnCorrectCharacters()
        {
            // Arrange
            var earthOrigin = new Origin { Name = "Earth" };
            var marsOrigin = new Origin { Name = "Mars" };

            var earthCharacter = new Character
            {
                Name = "Earth Character",
                Status = "Alive",
                Species = "Human",
                Origin = earthOrigin
            };

            var marsCharacter = new Character
            {
                Name = "Mars Character",
                Status = "Alive",
                Species = "Martian",
                Origin = marsOrigin
            };

            await _service.CreateCharacterAsync(earthCharacter);
            await _service.CreateCharacterAsync(marsCharacter);

            // Act
            var earthCharacters = await _service.GetCharactersByOriginAsync("Earth");
            var marsCharacters = await _service.GetCharactersByOriginAsync("Mars");

            // Assert
            Assert.Single(earthCharacters);
            Assert.Single(marsCharacters);
            Assert.Equal("Earth Character", earthCharacters.First().Name);
            Assert.Equal("Mars Character", marsCharacters.First().Name);
        }
    }
} 