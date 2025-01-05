
using ClickerBackend.Repositoies;

namespace ClickerBackend.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public Task<int> CheckUserExists(string userId)
        {
            return _userRepository.CheckUserExists(userId);
        }

        public Task CreateUser(string userId)
        {
            return _userRepository.CreateUser(userId);
        }

        public Task<DateTimeOffset?> FindLastConnectById(string userId)
        {
            return _userRepository.FindLastConnectById(userId);
        }

        public Task UpdateConnect(string userId)
        {
            return _userRepository.UpdateConnect(userId);
        }
    }
}
