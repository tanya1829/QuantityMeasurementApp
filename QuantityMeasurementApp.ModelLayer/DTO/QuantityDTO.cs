using QuantityMeasurementApp.ModelLayer.Models;

namespace QuantityMeasurementApp.ModelLayer.DTO
{
    public class QuantityDTO
    {
        public double  Value           { get; set; }
        public object? Unit            { get; set; }
        public string? UnitName        { get; set; }
        public string? MeasurementType { get; set; }

        public QuantityDTO() { }

        public QuantityDTO(double value, object unit)
        {
            Value = value;
            Unit  = unit;
        }

        public void ResolveUnit()
        {
            // If Unit is already a real enum, nothing to do
            if (Unit is Enum) return;

            // Use UnitName if provided, otherwise try Unit as string
            string? nameToResolve = UnitName;

            if (string.IsNullOrWhiteSpace(nameToResolve) && Unit is string unitStr)
                nameToResolve = unitStr;

            if (string.IsNullOrWhiteSpace(nameToResolve)) return;

            if (!string.IsNullOrWhiteSpace(MeasurementType))
            {
                Unit = MeasurementType switch
                {
                    "LengthUnit"      => ParseEnum<LengthEnum>(nameToResolve),
                    "WeightUnit"      => ParseEnum<WeightEnum>(nameToResolve),
                    "VolumeUnit"      => ParseEnum<VolumeEnum>(nameToResolve),
                    "TemperatureUnit" => ParseEnum<TemperatureEnum>(nameToResolve),
                    _                 => TryResolveWithoutType(nameToResolve)
                };
            }
            else
            {
                Unit = TryResolveWithoutType(nameToResolve);
            }
        }

        private static object TryResolveWithoutType(string unitName)
        {
            if (Enum.TryParse<LengthEnum>(unitName, true, out var l))      return l;
            if (Enum.TryParse<WeightEnum>(unitName, true, out var w))      return w;
            if (Enum.TryParse<VolumeEnum>(unitName, true, out var v))      return v;
            if (Enum.TryParse<TemperatureEnum>(unitName, true, out var t)) return t;
            throw new System.ArgumentException($"Unknown unit: {unitName}");
        }

        private static object ParseEnum<T>(string value) where T : struct, Enum
        {
            if (Enum.TryParse<T>(value, true, out var result)) return result;
            throw new System.ArgumentException(
                $"Unit ''{value}'' is not valid for {typeof(T).Name}");
        }
    }
}
