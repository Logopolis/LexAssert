using LexAssert.Tests.TestClasses;
using Xunit;
using Xunit.Sdk;

namespace LexAssert.Tests.LexAssertTests
{
    public class LexAssert_MembersEqual_Tests
    {
        [Fact]
        public void ShouldPassWhenSelectedMembersAreEqual()
        {
            // Arrange
            var x = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
                BazBool = true,
            };

            var y = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
                BazBool = false,
            };

            // Act, Assert
            Lassert.MembersEqual(x, y, c => c.FooString, c => c.BarInt);
        }

        [Fact]
        public void ShouldFailWhenSelectedMembersAreNotEqual()
        {
            // Arrange
            var x = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
                BazBool = true,
            };

            var y = new SimpleTestClass
            {
                FooString = "Foo",
                BarInt = 42,
                BazBool = false,
            };

            // Act, Assert
            Assert.Throws<EqualException>(() =>
                Lassert.MembersEqual(x, y, c => c.FooString, c => c.BazBool));
        }
    }
}
