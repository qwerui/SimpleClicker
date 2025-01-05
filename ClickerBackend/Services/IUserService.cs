namespace ClickerBackend.Services
{
    public interface IUserService
    {
        Task<int> CheckUserExists(string userId);
        Task CreateUser(string userId);
        Task<DateTimeOffset?> FindLastConnectById(string userId);
        Task UpdateConnect(string userId);
    }
}
