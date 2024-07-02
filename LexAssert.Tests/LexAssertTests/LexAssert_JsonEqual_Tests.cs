using LexAssert.Tests.TestClasses;

using System.Collections.Generic;

using Xunit;
using Xunit.Sdk;

namespace LexAssert.Tests.LexAssertTests
{
    public class LexAssert_JsonEqual_Tests
    {
        [Fact]
        public void ShouldPassWhenObjectsHaveEqualProperties()
        {
            // Arrange
            var x = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
            };

            var y = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
            };

            // Act, Assert
            Lassert.JsonEqual(x, y);
        }

        [Fact]
        public void ShouldPassWhenObjectsMakeEqualJsonAndIfObjectsAreDifferent()
        {
            // Arrange
            var x = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
            };

            var y = new Dictionary<string, object>()
            {
                { "FooString", "Foo" },
                { "BarInt", 42 },
                { "BazBool", false }
            };

            // Act, Assert
            Lassert.JsonEqual(x, y);
        }

        [Fact]
        public void ShouldFailWhenObjectsDoNotHaveAllEqualProperties()
        {
            // Arrange
            var x = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
            };

            var y = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 43,
            };

            // Act, Assert
            Assert.Throws<EqualException>(() => Lassert.JsonEqual(x, y));
        }

        [Fact]
        public void ShouldBeAMessageWhenJsonEqualFails()
        {
            // Arrange
            var x = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
            };

            var y = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 43,
            };

            // Act
            try
            {
                // This should throw a JsonEqualException.
                // If not, the test "ShouldFailWhenObjectsDoNotHaveAllEqualProperties" will fail.
                Lassert.JsonEqual(x, y); 
            }

            // Assert
            catch(EqualException ex)
            {
                Assert.NotNull(ex.Message);
                Assert.NotSame(string.Empty, ex.Message);
            }
        }
    }
}
