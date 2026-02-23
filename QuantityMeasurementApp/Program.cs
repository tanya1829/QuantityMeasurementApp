using System;
using QuantityMeasurementApp.Services;
using QuantityMeasurementApp.Models;
namespace QuantityMeasurementApp
{
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
                Console.WriteLine("3. Compare Length ");
                Console.WriteLine("4. Exit");
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
                        Console.WriteLine("Exiting application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }

            } while (choice != 4);
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

            Console.Write("Enter first unit (Feet/Inch): ");
            LengthUnit unit1 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            Console.Write("Enter second value: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second unit (Feet/Inch): ");
            LengthUnit unit2 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            Length l1 = new Length(value1, unit1);
            Length l2 = new Length(value2, unit2);

            bool result = QuantityMeasurementService.AreLengthEqual(value1, unit1, value2, unit2);

            Console.WriteLine("Length Equal: " + result);
        }
    }
}