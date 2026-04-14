namespace QuantityMeasurementApp.ModelLayer.DTO
{
    public class AuthResponseDTO
    {
        public object? Data                 { get; set; }  // Added this
        public string?  AccessToken         { get; set; }
        public string?  RefreshToken        { get; set; }
        public DateTime? AccessTokenExpiry  { get; set; }
        public string   Name                { get; set; } = string.Empty;  // Changed from Username
        public string   Email               { get; set; } = string.Empty;
        public string   Role                { get; set; } = string.Empty;
        public string   Message             { get; set; } = string.Empty;
    }
}