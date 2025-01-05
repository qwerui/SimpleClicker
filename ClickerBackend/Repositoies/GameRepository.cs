using ClickerBackend.Config;
using ClickerBackend.Dtos;
using ClickerBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClickerBackend.Repositoies
{
    public class GameRepository : IGameRepository
    {
        ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Game> FindGameById(string userId)
        {
            return _context.Game
                .Where(item => item.GameUserId == userId && item.ClearTime == null)
                .SingleAsync();
        }

        public Task InsertClear(GameDataDTO gameData)
        {
            return _context.Game
                .Where(item => item.GameUserId == gameData.UserId && item.ClearTime == null)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(item => item.ClearTime, DateTimeOffset.UtcNow)
                .SetProperty(item => item.ClickCount, gameData.ClickCount)
                .SetProperty(item => item.TotalGold, gameData.TotalGold)
                .SetProperty(item => item.KillCount, gameData.KillCount));
        }

        public Task InsertNewGame(string userId, out string gameId)
        {
            gameId = $"{userId}_{Guid.NewGuid()}";

            _context.Game.Add(new Game
            {
                GameUserId = userId,
                StartTime = DateTimeOffset.Now,
                GameId = gameId,
                ClickCount = 0,
                TotalGold = 0,
                KillCount = 0,
                Gold = 0,
            });

            return _context.SaveChangesAsync();
        }

        public Task UpdateGame(GameDataDTO gameData)
        {
            return _context.Game
                .Where(item => item.GameUserId == gameData.UserId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(item => item.KillCount, gameData.KillCount)
                .SetProperty(item => item.Gold, gameData.Gold)
                .SetProperty(item => item.TotalGold, gameData.TotalGold)
                .SetProperty(item => item.ClickCount, gameData.ClickCount));
        }
    }
}
