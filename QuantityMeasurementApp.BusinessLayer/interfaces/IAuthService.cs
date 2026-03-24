using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.BusinessLayer.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request);
        Task                  RevokeAsync(string email);
        Task                  RevokeAsync(string email, string? accessToken);
    }
}