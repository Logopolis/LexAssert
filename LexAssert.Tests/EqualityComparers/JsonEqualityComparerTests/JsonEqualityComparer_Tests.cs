using LexAssert.EqualityComparers;
using LexAssert.Tests.TestClasses;

using Xunit;
using Xunit.Sdk;

namespace LexAssert.Tests.EqualityComparers.JsonEqualityComparerTests
{
    public class JsonEqualityComparer_Tests
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
            Assert.Equal(
                x,
                y,
                new JsonEqualityComparer<SimpleTestClass>());
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
            Assert.Throws<EqualException>(() =>
                Assert.Equal(
                    x,
                    y,
                    new JsonEqualityComparer<SimpleTestClass>())
            );
        }

        [Fact]
        public void ShouldYieldSameHashCodesWhenObjectsHaveEqualProperties()
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

            var ComparerUnderTest = new JsonEqualityComparer<SimpleTestClass>();
            
            // Act
            var xHash = ComparerUnderTest.GetHashCode(x);
            var yHash = ComparerUnderTest.GetHashCode(y);

            // Assert
            Assert.Equal(xHash, yHash);
        }

        [Fact]
        public void ShouldYieldDifferentHashCodesWhenObjectsHaveDifferentProperties()
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

            var ComparerUnderTest = new JsonEqualityComparer<SimpleTestClass>();

            // Act
            var xHash = ComparerUnderTest.GetHashCode(x);
            var yHash = ComparerUnderTest.GetHashCode(y);

            // Assert
            Assert.NotEqual(xHash, yHash);
        }
    }
}
