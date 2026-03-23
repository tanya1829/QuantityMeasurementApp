namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>
    /// Input DTO for two-operand quantity operations
    /// (compare, convert, add, subtract, divide)
    /// </summary>
    public class QuantityInputDTO
    {
        public QuantityDTO ThisQuantityDTO { get; set; } = new(0, null!);
        public QuantityDTO ThatQuantityDTO { get; set; } = new(0, null!);
    }
}