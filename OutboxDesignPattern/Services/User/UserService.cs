using OutboxDesignPattern.Data;
using OutboxDesignPattern.Data.Entity;

namespace OutboxDesignPattern.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveUserAsync(UserEntity user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
