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
    }
}