namespace ClickerBackend.Repositoies
{
    public interface IUserRepository
    {
        public Task<DateTimeOffset?> FindLastConnectById(string userId);
        Task<int> CheckUserExists(string userId);
        Task CreateUser(string userId);
        Task UpdateConnect(string userId);
    }
}
