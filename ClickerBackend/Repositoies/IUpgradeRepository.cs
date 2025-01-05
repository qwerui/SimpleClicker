using ClickerBackend.Dtos;
using ClickerBackend.Models;

namespace ClickerBackend.Repositoies
{
    public interface IUpgradeRepository
    {
        Task<List<UpgradeDTO>> FindUpgradesByGame(string gameId);
        void UpdateUpgrade(UpgradeDTO upgradeDTO);
    }
}
