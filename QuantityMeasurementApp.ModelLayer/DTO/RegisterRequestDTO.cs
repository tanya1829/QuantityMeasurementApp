namespace QuantityMeasurementApp.ModelLayer.DTO
{
    public class RegisterRequestDTO
    {
        public string Name     { get; set; } = string.Empty;  // Changed from Username
        public string Email    { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role     { get; set; } = "User";
    }
} 