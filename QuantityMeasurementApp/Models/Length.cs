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

        private double ConvertToFeet()
        {
            return value * unit.ToFeetFactor();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Length))
                return false;

            Length other = (Length)obj;

            return Math.Abs(this.ConvertToFeet() - other.ConvertToFeet()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }
    }
}

/*
SUMMARY:
This class represents a generic length quantity.
Each length has a value and a unit.
All values are internally converted to the base unit (Feet) for comparison.
The Equals() method compares two Length objects after conversion.
The design ensures scalability and avoids duplication when adding new units.
*/