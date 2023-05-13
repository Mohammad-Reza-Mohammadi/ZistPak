using Entities.Useres;

namespace Services.Services
{
    public interface IJwtSevice
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}