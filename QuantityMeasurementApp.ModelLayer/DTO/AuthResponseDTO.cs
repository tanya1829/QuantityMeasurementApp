namespace QuantityMeasurementApp.ModelLayer.DTO
{
    public class AuthResponseDTO
    {
        public string?  AccessToken         { get; set; }
        public string?  RefreshToken        { get; set; }
        public DateTime? AccessTokenExpiry  { get; set; }
        public string   Username            { get; set; } = string.Empty;
        public string   Email               { get; set; } = string.Empty;
        public string   Role                { get; set; } = string.Empty;
        public string   Message             { get; set; } = string.Empty;
    }
}
