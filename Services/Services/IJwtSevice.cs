using Entities.Useres;

namespace Services.Services
{
    public interface IJwtSevice
    {
        Task<string> GenerateAsync(User user,CancellationToken cancellationToken);
    }
}