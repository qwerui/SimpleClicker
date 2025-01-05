using ClickerBackend.Config;
using Microsoft.EntityFrameworkCore;

namespace ClickerBackend.Repositoies
{
    public class UserRepository : IUserRepository
    {
        readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<int> CheckUserExists(string userId)
        {
            return _context.User
                .Where(item => item.GameUserId == userId)
                .CountAsync();
        }

        public Task CreateUser(string userId)
        {
            _context.User.Add(new Models.User {GameUserId = userId });
            return _context.SaveChangesAsync();
        }

        public Task<DateTimeOffset?> FindLastConnectById(string userId)
        {
            return _context.User
                .Where(item=>item.GameUserId == userId)
                .Select(item=>item.LastConnect)
                .SingleAsync();
        }

        public Task UpdateConnect(string userId)
        {
            return _context.User
                .Where(item => item.GameUserId == userId)
                .ExecuteUpdateAsync(setter =>setter
                .SetProperty(item=>item.LastConnect, DateTimeOffset.UtcNow));
        }
    }
}
