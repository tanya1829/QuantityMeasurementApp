namespace QuantityMeasurementApp.Models
{
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yards,
        Centimeters
    }

    public static class LengthUnitExtensions
    {
        public static double ToFeetFactor(this LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return 1.0;

                case LengthUnit.Inch:
                    return 1.0 / 12.0;

                case LengthUnit.Yards:
                    return 3.0;

                case LengthUnit.Centimeters:
                    return 0.0328084;

                default:
                    throw new ArgumentException("Unsupported length unit");
            }
        }
    }
}

/*
SUMMARY:
This file defines all supported length units including Feet, Inch, Yards, and Centimeters.
Feet is used as the base unit for conversion.
The ToFeetFactor() method provides conversion factors to convert any unit into Feet.
This allows new units to be added easily without modifying other classes.
*/