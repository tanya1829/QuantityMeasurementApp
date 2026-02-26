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

        // Convert everything to Inches (base unit)
        private double ToInches()
        {
            return unit switch
            {
                LengthUnit.Feet => value * 12,
                LengthUnit.Yard => value * 36,
                LengthUnit.Inch => value,
                LengthUnit.Centimeter => value * 0.393701,
                _ => throw new ArgumentException("Invalid length unit")
            };
        }

        public Length Add(Length other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double resultInInches = this.ToInches() + other.ToInches();

            // return result in unit of first operand
            return unit switch
            {
                LengthUnit.Feet => new Length(resultInInches / 12, LengthUnit.Feet),
                LengthUnit.Yard => new Length(resultInInches / 36, LengthUnit.Yard),
                LengthUnit.Inch => new Length(resultInInches, LengthUnit.Inch),
                LengthUnit.Centimeter => new Length(resultInInches / 0.393701, LengthUnit.Centimeter),
                _ => throw new ArgumentException("Invalid length unit")
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is not Length other)
                return false;

            return Math.Abs(this.ToInches() - other.ToInches()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToInches().GetHashCode();
        }
    }
}