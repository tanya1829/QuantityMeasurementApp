using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementTests
    {
        //  UC1 - FEET TEST CASES 
        [TestMethod]
        public void FeetEquality_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreFeetEqual(1.0, 1.0));
        }

        [TestMethod]
        public void FeetEquality_DifferentValue_ReturnsFalse()
        {
            Assert.IsFalse(QuantityMeasurementService.AreFeetEqual(1.0, 2.0));
        }

        [TestMethod]
        public void FeetEquality_SameReference_ReturnsTrue()
        {
            Feet f1 = new Feet(1.0);
            Assert.IsTrue(f1.Equals(f1));
        }

        [TestMethod]
        public void FeetEquality_NullComparison_ReturnsFalse()
        {
            Feet f1 = new Feet(1.0);
            Feet? f2 = null;

            Assert.IsFalse(f1.Equals(f2));
        }

        //  UC2 - INCHES TEST CASES 

        [TestMethod]
        public void InchesEquality_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreInchesEqual(5.0, 5.0));
        }

        [TestMethod]
        public void InchesEquality_DifferentValue_ReturnsFalse()
        {
            Assert.IsFalse(QuantityMeasurementService.AreInchesEqual(5.0, 6.0));
        }

        [TestMethod]
        public void InchesEquality_SameReference_ReturnsTrue()
        {
            Inches i1 = new Inches(2.0);
            Assert.IsTrue(i1.Equals(i1));
        }

        [TestMethod]
        public void InchesEquality_NullComparison_ReturnsFalse()
        {
            Inches i1 = new Inches(2.0);
            Inches? i2 = null;

            Assert.IsFalse(i1.Equals(i2));
        }

        //  UC3 - GENERIC LENGTH TEST CASES 

        [TestMethod]
        public void Length_FeetToFeet_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Feet,
                    1.0, LengthUnit.Feet));
        }

        [TestMethod]
        public void Length_InchToInch_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    5.0, LengthUnit.Inch,
                    5.0, LengthUnit.Inch));
        }

        [TestMethod]
        public void Length_FeetToInch_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Feet,
                    12.0, LengthUnit.Inch));
        }

        [TestMethod]
        public void Length_InchToFeet_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    12.0, LengthUnit.Inch,
                    1.0, LengthUnit.Feet));
        }

        [TestMethod]
        public void Length_DifferentValues_ReturnsFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.Feet,
                    2.0, LengthUnit.Feet));
        }

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
    }
}