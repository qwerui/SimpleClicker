using ClickerBackend.Dtos;
using ClickerBackend.Repositoies;

namespace ClickerBackend.Services
{
    public class GameService : IGameService
    {
        IGameRepository _gameRepository;
        IUpgradeRepository _upgradeRepository;

        public GameService(IGameRepository gameRepository, IUpgradeRepository upgradeRepository)
        {
            _gameRepository = gameRepository;
            _upgradeRepository = upgradeRepository;
        }

        public Task InitializeGameData(string userId, out string gameId)
        {
            return _gameRepository.InsertNewGame(userId, out gameId);
        }

        public async Task<GameDataDTO> GetGameData(string userId)
        {
            var game = await _gameRepository.FindGameById(userId);

            var upgrade = await _upgradeRepository.FindUpgradesByGame(game.GameId);

            return new GameDataDTO
            {
                StartTime = game.StartTime,
                GameId = game.GameId,
                ClickCount = game.ClickCount,
                KillCount = game.KillCount,
                Gold = game.Gold,
                TotalGold = game.TotalGold,
                Upgrades = upgrade
            };
        }

        public Task UpdateGameData(GameDataDTO gameData)
        {
            return _gameRepository.UpdateGame(gameData);
        }

        public void UpdateUpgrade(UpgradeDTO upgrade)
        {
            _upgradeRepository.UpdateUpgrade(upgrade);
        }

        public Task InsertClear(GameDataDTO gameData)
        {
            return _gameRepository.InsertClear(gameData);
        }
    }
}
