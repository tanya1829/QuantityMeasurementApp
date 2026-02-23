using System;

namespace QuantityMeasurementApp.Models
{
    public class Length
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public Length(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        // Convert everything to base unit (Feet)
        private double ToFeet()
        {
            return value * unit.ToFeetFactor();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || GetType() != obj.GetType())
                return false;

            Length other = (Length)obj;

            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }
    }
}