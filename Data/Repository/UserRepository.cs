namespace dotnetApi.Data.Repository
{
    using dotnetApi.Models;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly DataContextEF _efContext;

        public UserRepository(IConfiguration config)
        {
            _efContext = new DataContextEF(config);
        }

        public async Task<bool> SaveAsync()
        {
            return await _efContext.SaveChangesAsync() > 0;
        }
        public async Task AddEntityAsync<T>(T entity)
        {
            if (entity != null) _efContext.Add(entity); 
        }
        public async Task UpdateEntityAsync<T>(T entity)
        {
            if (entity != null) _efContext.Update(entity); 
        }
        public async Task RemoveEntityAsync<T>(T entity)
        {
            if (entity != null) _efContext.Remove(entity); 
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _efContext.Users.FindAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _efContext.Users.ToListAsync();
        }
    }
}