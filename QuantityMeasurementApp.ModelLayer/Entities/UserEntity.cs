namespace QuantityMeasurementApp.ModelLayer.Entities
{
    public class UserEntity
    {
        public Guid      Id                  { get; set; } = Guid.NewGuid();
        public string    Username            { get; set; } = string.Empty;
        public string    Email               { get; set; } = string.Empty;
        public string    PasswordHash        { get; set; } = string.Empty;
        public string    Role                { get; set; } = "User";
        public string?   RefreshToken        { get; set; }
        public DateTime? RefreshTokenExpiry  { get; set; }
        public bool      IsActive            { get; set; } = true;
        public DateTime  CreatedAt           { get; set; } = DateTime.UtcNow;
        public DateTime  UpdatedAt           { get; set; } = DateTime.UtcNow;
    }
}
