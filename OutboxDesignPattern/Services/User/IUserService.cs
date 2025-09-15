using OutboxDesignPattern.Data.Entity;

namespace OutboxDesignPattern.Services.User
{
    public interface IUserService
    {
        Task SaveUserAsync(UserEntity user);
    }
}
