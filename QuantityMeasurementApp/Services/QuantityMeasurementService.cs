using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        // Static method for Feet equality
        public static bool AreFeetEqual(double value1, double value2)
        {
            Feet f1 = new Feet(value1);
            Feet f2 = new Feet(value2);

            return f1.Equals(f2);
        }

        // Static method for Inches equality
        public static bool AreInchesEqual(double value1, double value2)
        {
            Inches i1 = new Inches(value1);
            Inches i2 = new Inches(value2);

            return i1.Equals(i2);
        }


        public static bool AreLengthEqual(double value1, LengthUnit unit1,double value2, LengthUnit unit2)
        {
            Length l1 = new Length(value1, unit1);
            Length l2 = new Length(value2, unit2);

            return l1.Equals(l2);
        }
    }
}
