using Microsoft.EntityFrameworkCore;

namespace ClickerBackend.Models
{
    [PrimaryKey(nameof(GameUserId))]
    public class User
    {
        public string GameUserId { set; get; }
        public DateTimeOffset? LastConnect { set; get; }

        public IList<Game> Games { get; set; }
    }
}
