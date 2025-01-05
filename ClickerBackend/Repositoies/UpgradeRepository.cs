using ClickerBackend.Config;
using ClickerBackend.Dtos;
using ClickerBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClickerBackend.Repositoies
{
    public class UpgradeRepository : IUpgradeRepository
    {
        ApplicationDbContext _context;

        public UpgradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<UpgradeDTO>> FindUpgradesByGame(string gameId)
        {
            return _context.Upgrade
                .Where(item => item.GameId == gameId)
                .Select(item => new UpgradeDTO 
                {
                    UpgradeId = item.UpgradeId,
                    Amount = item.Amount 
                })
                .ToListAsync();
        }

        public void UpdateUpgrade(UpgradeDTO upgradeDTO)
        {
            var upgrade = _context.Upgrade
                .Where(item => item.GameId == upgradeDTO.GameId && item.UpgradeId == upgradeDTO.UpgradeId)
                .SingleOrDefault();

            if(upgrade == null)
            {
                _context.AddAsync(new Upgrade
                {
                    UpgradeId = upgradeDTO.UpgradeId,
                    GameId = upgradeDTO.GameId,
                    Amount = upgradeDTO.Amount
                });
            }
            else
            {
                upgrade.Amount = upgradeDTO.Amount;
            }    

            _context.SaveChangesAsync();
        }
    }
}
