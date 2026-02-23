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
    }
}