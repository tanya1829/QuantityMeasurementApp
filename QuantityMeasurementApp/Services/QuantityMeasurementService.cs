using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        public static bool AreFeetEqual(double value1, double value2)
        {
            Feet f1 = new Feet(value1);
            Feet f2 = new Feet(value2);

            return f1.Equals(f2);
        }

        public static bool AreInchesEqual(double value1, double value2)
        {
            Inches i1 = new Inches(value1);
            Inches i2 = new Inches(value2);

            return i1.Equals(i2);
        }

        public static bool AreLengthEqual(double value1, LengthUnit unit1,
                                          double value2, LengthUnit unit2)
        {
            Length l1 = new Length(value1, unit1);
            Length l2 = new Length(value2, unit2);

            return l1.Equals(l2);
        }
    }
}

/*
SUMMARY:
This service class provides equality comparison methods for length measurements.

1. AreFeetEqual() compares two values specifically in Feet.
2. AreInchesEqual() compares two values specifically in Inches.
3. AreLengthEqual() is a generic comparison method that supports all length units
   including Feet, Inch, Yards, and Centimeters (UC4).

UC4 support is achieved through the Length class and LengthUnit enum,
which convert all units into the base unit (Feet) before comparison.

No additional method was required for UC4 because the design is scalable.
*/