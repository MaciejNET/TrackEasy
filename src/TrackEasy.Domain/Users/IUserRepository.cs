namespace TrackEasy.Domain.Users;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User> GetByEmailAsync(string requestEmail, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken);
    void Add(User user);
    void Delete(User user);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}