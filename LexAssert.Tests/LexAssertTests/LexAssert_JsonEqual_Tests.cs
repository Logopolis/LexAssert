using LexAssert.Tests.TestClasses;
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
    }
}
