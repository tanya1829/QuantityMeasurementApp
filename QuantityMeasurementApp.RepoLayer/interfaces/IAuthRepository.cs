using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserEntity?>  GetByEmailAsync(string email);
        Task<UserEntity?>  GetByIdAsync(Guid id);
        Task<bool>         EmailExistsAsync(string email);
        Task<bool>         UsernameExistsAsync(string username);
        Task               AddAsync(UserEntity user);
        Task               UpdateAsync(UserEntity user);
        Task               SaveChangesAsync();
    }
}