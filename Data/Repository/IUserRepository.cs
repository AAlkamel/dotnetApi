namespace dotnetApi.Data.Repository
{
    using dotnetApi.Models;
    public interface IUserRepository
    {
        Task<bool> SaveAsync();
        Task AddEntityAsync<T>(T entity);
        Task UpdateEntityAsync<T>(T entity);
        Task RemoveEntityAsync<T>(T entity);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
    }
}