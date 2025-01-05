using Microsoft.EntityFrameworkCore;

namespace ClickerBackend.Models
{
    [PrimaryKey(nameof(GameId))]
    public class Game
    {
        public string GameId { set; get; }
        public string GameUserId { set; get; }
        public DateTimeOffset? StartTime { set; get; }
        public DateTimeOffset? ClearTime { set; get; }
        public int KillCount { set; get; }
        public int ClickCount { set; get; }
        public long Gold { set; get; }
        public long TotalGold { set; get; }

        public virtual User User { set; get; }
        public IList<Upgrade> Upgrades { set; get; }
    }
}
