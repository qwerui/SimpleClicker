using ClickerBackend.Dtos;

namespace ClickerBackend.Services
{
    public interface IGameService
    {
        Task InitializeGameData(string userId, out string gameId);
        Task<GameDataDTO> GetGameData(string userId);
        Task UpdateGameData(GameDataDTO gameData);
        Task InsertClear(GameDataDTO gameData);
        void UpdateUpgrade(UpgradeDTO upgrade);
    }
}
