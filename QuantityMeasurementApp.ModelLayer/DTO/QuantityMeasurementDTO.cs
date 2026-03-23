using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>
    /// Response DTO returned from all quantity measurement operations.
    /// </summary>
    public class QuantityMeasurementDTO
    {
        public double?  ThisValue             { get; set; }
        public string?  ThisUnit              { get; set; }
        public string?  ThisMeasurementType   { get; set; }
        public double?  ThatValue             { get; set; }
        public string?  ThatUnit              { get; set; }
        public string?  ThatMeasurementType   { get; set; }
        public string?  Operation             { get; set; }
        public string?  ResultString          { get; set; }
        public double?  ResultValue           { get; set; }
        public string?  ResultUnit            { get; set; }
        public string?  ResultMeasurementType { get; set; }
        public string?  ErrorMessage          { get; set; }
        public bool     IsError               { get; set; }

        // ── Factory: build from a successful operation result ─────────────
        public static QuantityMeasurementDTO FromOperation(
            QuantityDTO   thisQty,
            QuantityDTO   thatQty,
            string        operation,
            string?       resultString          = null,
            double?       resultValue           = null,
            string?       resultUnit            = null,
            string?       resultMeasurementType = null) => new()
        {
            ThisValue           = thisQty.Value,
            ThisUnit            = thisQty.Unit?.ToString(),
            ThisMeasurementType = GetMeasurementType(thisQty.Unit),
            ThatValue           = thatQty.Value,
            ThatUnit            = thatQty.Unit?.ToString(),
            ThatMeasurementType = GetMeasurementType(thatQty.Unit),
            Operation           = operation,
            ResultString        = resultString,
            ResultValue         = resultValue,
            ResultUnit          = resultUnit,
            ResultMeasurementType = resultMeasurementType,
            IsError             = false
        };

        // ── Factory: build from an error ──────────────────────────────────
        public static QuantityMeasurementDTO FromError(
            QuantityDTO thisQty,
            QuantityDTO thatQty,
            string      operation,
            string      errorMessage) => new()
        {
            ThisValue           = thisQty.Value,
            ThisUnit            = thisQty.Unit?.ToString(),
            ThisMeasurementType = GetMeasurementType(thisQty.Unit),
            ThatValue           = thatQty.Value,
            ThatUnit            = thatQty.Unit?.ToString(),
            ThatMeasurementType = GetMeasurementType(thatQty.Unit),
            Operation           = operation,
            ErrorMessage        = errorMessage,
            IsError             = true
        };

        // ── Factory: convert entity list to DTO list ──────────────────────
        public static List<QuantityMeasurementDTO> FromEntityList(
            List<QuantityMeasurementEntity> entities) =>
            entities.Select(e => new QuantityMeasurementDTO
            {
                Operation    = e.Operation,
                ThisUnit     = e.OperandOne,
                ThatUnit     = e.OperandTwo,
                ResultString = e.Result,
                ThisMeasurementType = e.MeasureType
            }).ToList();

        private static string GetMeasurementType(object? unit)
        {
            if (unit == null) return "Unknown";
            var name = unit.GetType().Name;
            if (name.Contains("Length"))      return "LengthUnit";
            if (name.Contains("Weight"))      return "WeightUnit";
            if (name.Contains("Volume"))      return "VolumeUnit";
            if (name.Contains("Temperature")) return "TemperatureUnit";
            return unit switch
            {
                Enum e when e.GetType().Name.Contains("Length")      => "LengthUnit",
                Enum e when e.GetType().Name.Contains("Weight")      => "WeightUnit",
                Enum e when e.GetType().Name.Contains("Volume")      => "VolumeUnit",
                Enum e when e.GetType().Name.Contains("Temperature") => "TemperatureUnit",
                _ => "Unknown"
            };
        }
    }
}