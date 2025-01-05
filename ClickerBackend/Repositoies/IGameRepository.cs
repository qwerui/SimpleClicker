using ClickerBackend.Dtos;
using ClickerBackend.Models;

namespace ClickerBackend.Repositoies
{
    public interface IGameRepository
    {
        Task<Game> FindGameById(string userId);
        Task InsertClear(GameDataDTO gameData);
        Task InsertNewGame(string userId, out string gameId);
        Task UpdateGame(GameDataDTO gameData);
    }
}
