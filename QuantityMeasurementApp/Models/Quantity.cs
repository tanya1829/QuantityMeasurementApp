using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic Quantity class supporting different measurement units
    /// like LengthUnit, WeightUnit and VolumeUnit.
    /// </summary>
    public class Quantity<U>
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            Value = value;
            Unit = unit;
        }

        // Convert quantity to another unit
        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(Value, Unit);
            double targetValue = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(targetValue, targetUnit);
        }

        // Add two quantities
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double sumBase = base1 + base2;

            double result = ConvertFromBase(sumBase, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        // Subtract quantities
        public Quantity<U> Subtract(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double resultBase = base1 - base2;

            double result = ConvertFromBase(resultBase, Unit);

            result = Math.Round(result, 2);

            return new Quantity<U>(result, Unit);
        }

        // Subtract quantities with target unit
        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double resultBase = base1 - base2;

            double result = ConvertFromBase(resultBase, targetUnit);

            result = Math.Round(result, 2);

            return new Quantity<U>(result, targetUnit);
        }

        // Division operation
        public double Divide(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            if (base2 == 0)
                throw new ArithmeticException("Division by zero");

            return base1 / base2;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Quantity<U> other)
                return false;

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Unit);
        }

        // Convert TO base unit
        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertToBaseUnit(value);

            if (unit is WeightUnit wu)
                return wu.ConvertToBaseUnit(value);

            if (unit is VolumeUnit vu)
                return vu.ConvertToBaseUnit(value);

            throw new InvalidOperationException("Unsupported unit type");
        }

        // Convert FROM base unit
        private static double ConvertFromBase(double baseValue, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertFromBaseUnit(baseValue);

            if (unit is WeightUnit wu)
                return wu.ConvertFromBaseUnit(baseValue);

            if (unit is VolumeUnit vu)
                return vu.ConvertFromBaseUnit(baseValue);

            throw new InvalidOperationException("Unsupported unit type");
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}