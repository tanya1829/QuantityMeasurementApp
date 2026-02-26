using System;
using QuantityMeasurementApp.Services;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Console application to compare and convert length units.
    /// Supports Feet, Inch, Yard and Centimeter.
    /// Covers UC1 to UC5.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            int choice;

            do
            {
                Console.WriteLine("\n===== Quantity Measurement App =====");
                Console.WriteLine("1. Compare Feet");
                Console.WriteLine("2. Compare Inches");
                Console.WriteLine("3. Compare Length (Any Unit)");
                Console.WriteLine("4. Convert Length Units");
                Console.WriteLine("5. Add Length Units");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CompareFeet();
                        break;

                    case 2:
                        CompareInches();
                        break;

                    case 3:
                        CompareLength();
                        break;

                    case 4:
                        ConvertLength();
                        break;

                    case 5:
                        AddLength();
                        break;

                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }

            } while (choice != 5);
        }

        private static void CompareFeet()
        {
            Console.Write("Enter first value in feet: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second value in feet: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            bool result = QuantityMeasurementService.AreLengthEqual(
                value1, LengthUnit.Feet,
                value2, LengthUnit.Feet);

            Console.WriteLine("Feet Equal: " + result);
        }

        private static void CompareInches()
        {
            Console.Write("Enter first value in inches: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second value in inches: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            bool result = QuantityMeasurementService.AreLengthEqual(
                value1, LengthUnit.Inch,
                value2, LengthUnit.Inch);

            Console.WriteLine("Inches Equal: " + result);
        }

        private static void CompareLength()
        {
            Console.Write("Enter first value: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter first unit (Feet/Inch/Yard/Centimeter): ");
            LengthUnit unit1 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            Console.Write("Enter second value: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second unit (Feet/Inch/Yard/Centimeter): ");
            LengthUnit unit2 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            bool result = QuantityMeasurementService.AreLengthEqual(
                value1, unit1,
                value2, unit2);

            Console.WriteLine("Length Equal: " + result);
        }

        private static void ConvertLength()
        {
            Console.Write("Enter value to convert: ");
            double value = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Convert FROM (Feet/Inch/Yard/Centimeter): ");
            LengthUnit fromUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            Console.WriteLine("Convert TO (Feet/Inch/Yard/Centimeter): ");
            LengthUnit toUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            double result = QuantityMeasurementService.ConvertLength(value, fromUnit, toUnit);

            Console.WriteLine($"Converted Value: {result} {toUnit}");
        }
        private static void AddLength()
        {
            Console.Write("Enter first value: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter first unit (Feet/Inch/Yard/Centimeter): ");
            LengthUnit unit1 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            Console.Write("Enter second value: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second unit (Feet/Inch/Yard/Centimeter): ");
            LengthUnit unit2 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            // Create Length objects
            Length length1 = new Length(value1, unit1);
            Length length2 = new Length(value2, unit2);

            // Perform Addition
            Length result = length1.Add(length2);

            Console.WriteLine($"Result: {result}");
        }
    }
}