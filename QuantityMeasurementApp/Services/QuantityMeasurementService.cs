using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        public static bool AreLengthEqual(double value1, LengthUnit unit1,
                                          double value2, LengthUnit unit2)
        {
            Length l1 = new Length(value1, unit1);
            Length l2 = new Length(value2, unit2);

            return l1.Equals(l2);
        }

        public static double Convert(double value,
                                     LengthUnit sourceUnit,
                                     LengthUnit targetUnit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Invalid numeric value");

            if (sourceUnit == null || targetUnit == null)
                throw new ArgumentException("Units cannot be null");

            double valueInFeet = value * sourceUnit.ToFeetFactor();

            return valueInFeet / targetUnit.ToFeetFactor();
        }
    }
}

/*
SUMMARY:

This class now supports UC5 explicit unit-to-unit conversion.

Convert() method:
1. Validates input value (must be finite).
2. Normalizes the source value to the base unit (Feet).
3. Converts from base unit to target unit.
4. Returns the converted numeric result.

The formula used:
result = value × (sourceFactor / targetFactor)

Supports Feet, Inch, Yards, and Centimeters.
Throws ArgumentException for invalid inputs.
*/