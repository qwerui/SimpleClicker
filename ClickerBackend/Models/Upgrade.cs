using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace ClickerBackend.Models
{
    [PrimaryKey(nameof(UpgradeId), nameof(GameId))]
    public class Upgrade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UpgradeId { set; get; }
        public string GameId { set; get; }
        public int Amount { set; get; }

        public virtual Game Game { set; get; }
    }
}
