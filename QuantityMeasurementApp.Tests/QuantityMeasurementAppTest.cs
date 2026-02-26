using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;


namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementTests
    {
        // UC1 - Feet Equality

        [TestMethod]
        public void Feet_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Feet,
                    1.0, LengthUnit.Feet));
        }

        [TestMethod]
        public void Feet_DifferentValue_ReturnsFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Feet,
                    2.0, LengthUnit.Feet));
        }

        // UC2 - Inch Equality

        [TestMethod]
        public void Inch_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    5.0, LengthUnit.Inch,
                    5.0, LengthUnit.Inch));
        }

        [TestMethod]
        public void Inch_DifferentValue_ReturnsFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementService.AreLengthEqual(
                    5.0, LengthUnit.Inch,
                    6.0, LengthUnit.Inch));
        }

        // UC3 - Cross Unit Comparison

        [TestMethod]
        public void Feet_To_Inch_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Feet,
                    12.0, LengthUnit.Inch));
        }

        [TestMethod]
        public void Inch_To_Feet_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    12.0, LengthUnit.Inch,
                    1.0, LengthUnit.Feet));
        }

        [TestMethod]
        public void Different_Lengths_ReturnsFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Feet,
                    2.0, LengthUnit.Feet));
        }

        // UC4 - Null & Reference Checks

        [TestMethod]
        public void Length_NullComparison_ReturnsFalse()
        {
            Length l = new Length(1.0, LengthUnit.Feet);
            Assert.IsFalse(l.Equals(null));
        }

        [TestMethod]
        public void Length_SameReference_ReturnsTrue()
        {
            Length l = new Length(1.0, LengthUnit.Feet);
            Assert.IsTrue(l.Equals(l));
        }
        [TestMethod]
        public void UC5_YardToFeet_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Yard,
                    3.0, LengthUnit.Feet));
        }

        [TestMethod]
        public void UC5_FeetToYard_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    3.0, LengthUnit.Feet,
                    1.0, LengthUnit.Yard));
        }

        [TestMethod]
        public void UC5_YardToInch_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Yard,
                    36.0, LengthUnit.Inch));
        }

        [TestMethod]
        public void UC5_DifferentUnits_NotEqual_ReturnsFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Yard,
                    1.0, LengthUnit.Feet));
        }

        // ---------------- UC5 - ConvertLength Tests ----------------

        [TestMethod]
        public void Convert_Feet_To_Inch_ReturnsCorrectValue()
        {
            double result = QuantityMeasurementService.ConvertLength(
                1.0, LengthUnit.Feet, LengthUnit.Inch);

            Assert.AreEqual(12.0, result);
        }

        [TestMethod]
        public void Convert_Yard_To_Feet_ReturnsCorrectValue()
        {
            double result = QuantityMeasurementService.ConvertLength(
                1.0, LengthUnit.Yard, LengthUnit.Feet);

            Assert.AreEqual(3.0, result);
        }

        [TestMethod]
        public void Convert_Centimeter_To_Inch_ReturnsCorrectValue()
        {
            double result = QuantityMeasurementService.ConvertLength(
                2.54, LengthUnit.Centimeter, LengthUnit.Inch);

            Assert.AreEqual(1.0, result);
        }
        // ---------------- UC6 - Addition Tests ----------------

        [TestMethod]
        public void UC6_Add_FeetPlusFeet_ReturnsThreeFeet()
        {
            Length l1 = new Length(1.0, LengthUnit.Feet);
            Length l2 = new Length(2.0, LengthUnit.Feet);

            Length result = l1.Add(l2);

            Assert.AreEqual(new Length(3.0, LengthUnit.Feet), result);
        }

        [TestMethod]
        public void UC6_Add_InchPlusInch_ReturnsTwelveInch()
        {
            Length l1 = new Length(6.0, LengthUnit.Inch);
            Length l2 = new Length(6.0, LengthUnit.Inch);

            Length result = l1.Add(l2);

            Assert.AreEqual(new Length(12.0, LengthUnit.Inch), result);
        }

        [TestMethod]
        public void UC6_Add_FeetPlusInch_ReturnsTwoFeet()
        {
            Length l1 = new Length(1.0, LengthUnit.Feet);
            Length l2 = new Length(12.0, LengthUnit.Inch);

            Length result = l1.Add(l2);

            Assert.AreEqual(new Length(2.0, LengthUnit.Feet), result);
        }

        [TestMethod]
        public void UC6_Add_InchPlusFeet_ReturnsTwentyFourInch()
        {
            Length l1 = new Length(12.0, LengthUnit.Inch);
            Length l2 = new Length(1.0, LengthUnit.Feet);

            Length result = l1.Add(l2);

            Assert.AreEqual(new Length(24.0, LengthUnit.Inch), result);
        }

        [TestMethod]
        public void UC6_Add_YardPlusFeet_ReturnsTwoYards()
        {
            Length l1 = new Length(1.0, LengthUnit.Yard);
            Length l2 = new Length(3.0, LengthUnit.Feet);

            Length result = l1.Add(l2);

            Assert.AreEqual(new Length(2.0, LengthUnit.Yard), result);
        }

        [TestMethod]
        public void UC6_Add_CentimeterPlusInch_ReturnsCorrectCentimeter()
        {
            Length l1 = new Length(2.54, LengthUnit.Centimeter);
            Length l2 = new Length(1.0, LengthUnit.Inch);

            Length result = l1.Add(l2);

            Assert.AreEqual(new Length(5.08, LengthUnit.Centimeter), result);
        }

        [TestMethod]
        public void UC6_Add_WithZero_ReturnsSameValue()
        {
            Length l1 = new Length(5.0, LengthUnit.Feet);
            Length l2 = new Length(0.0, LengthUnit.Inch);

            Length result = l1.Add(l2);

            Assert.AreEqual(new Length(5.0, LengthUnit.Feet), result);
        }

        [TestMethod]
        public void UC6_Add_NegativeValue_ReturnsCorrectResult()
        {
            Length l1 = new Length(5.0, LengthUnit.Feet);
            Length l2 = new Length(-2.0, LengthUnit.Feet);

            Length result = l1.Add(l2);

            Assert.AreEqual(new Length(3.0, LengthUnit.Feet), result);
        }
        [TestMethod]
        public void UC6_Add_NullValue_ThrowsException()
        {
            Length l1 = new Length(5.0, LengthUnit.Feet);
            Length l2 = null;

            try
            {
                l1.Add(l2);
                Assert.Fail("Expected ArgumentNullException was not thrown.");
            }
            catch (ArgumentNullException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void UC6_Add_CommutativeProperty()
        {
            Length l1 = new Length(1.0, LengthUnit.Feet);
            Length l2 = new Length(12.0, LengthUnit.Inch);

            Length result1 = l1.Add(l2);
            Length result2 = l2.Add(l1);

            Assert.IsTrue(result1.Equals(result2));
        }
    }
}