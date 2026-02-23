using System;
using QuantityMeasurementApp.Services;

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
                Console.WriteLine("3. Exit");
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
                        Console.WriteLine("Exiting application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }

            } while (choice != 3);
        }

        private static void CompareFeet()
        {
            Console.Write("Enter first value in feet: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second value in feet: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            bool result = QuantityMeasurementService.AreFeetEqual(value1, value2);
            Console.WriteLine("Feet Equal: " + result);
        }

        private static void CompareInches()
        {
            Console.Write("Enter first value in inches: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second value in inches: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            bool result = QuantityMeasurementService.AreInchesEqual( value1, value2);
            Console.WriteLine("Inches Equal: " + result);
        }
    }
}