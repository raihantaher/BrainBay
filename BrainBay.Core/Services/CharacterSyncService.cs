namespace BrainBay.Core.Services
{
    public class CharacterSyncService : ICharacterSyncService
    {
        private readonly ICharacterService _characterService;
        private readonly IRickAndMortyApiService _apiService;

        public CharacterSyncService(ICharacterService characterService, IRickAndMortyApiService apiService)
        {
            _characterService = characterService ?? throw new ArgumentNullException(nameof(characterService));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }

        public async Task SyncCharactersFromApiAsync()
        {
            try
            {
                var aliveCharacters = await _apiService.GetAliveCharactersAsync();

                await _characterService.ClearCharactersAsync();

                await _characterService.SaveCharactersAsync(aliveCharacters);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to sync characters from API", ex);
            }
        }
    }
} 