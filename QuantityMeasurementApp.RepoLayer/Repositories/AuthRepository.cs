using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Data;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly QuantityMeasurementDbContext _context;

        public AuthRepository(QuantityMeasurementDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<UserEntity?> GetByIdAsync(Guid id) =>
            await _context.Users.FindAsync(id);

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<bool> UsernameExistsAsync(string username) =>
            await _context.Users.AnyAsync(u => u.Username == username);

        public async Task AddAsync(UserEntity user) =>
            await _context.Users.AddAsync(user);

        public Task UpdateAsync(UserEntity user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
