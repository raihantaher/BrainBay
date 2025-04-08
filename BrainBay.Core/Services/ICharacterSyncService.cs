namespace BrainBay.Core.Services
{
    public interface ICharacterSyncService
    {
        Task SyncCharactersFromApiAsync();
    }
} 