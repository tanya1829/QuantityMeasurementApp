using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        // Method to check equality of two feet objects
        
        public static bool AreFeetEqual(double v1, double v2)
        {
            Feet f1 = new Feet(v1);
            Feet f2 = new Feet(v2);

            return f1.Equals(f2);
        }

         // Static method to compare two inches values
        public static bool AreInchesEqual(double v1, double v2)
        {
            Inches i1 = new Inches(v1);
            Inches i2 = new Inches(v2);

            return i1.Equals(i2);
        }

     
        // Generic equality check
        public static bool AreLengthEqual(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            return q1.Equals(q2);
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