using System;

namespace QuantityMeasurementApp.Models
{
    public class Inches
    {
        private readonly double value;

        public Inches(double value)
        {
            this.value = value;
        }

        public override bool Equals(object? obj)
        {
            if (this == obj)
                return true;

            if (obj == null || GetType() != obj.GetType())
                return false;

            Inches other = (Inches)obj;

            return this.value.CompareTo(other.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}