using System.Collections.Generic;

using Xunit;

namespace LexAssert.Tests.Demo
{
    public class LexAssertDemos
    {
        internal class MyTestClass
        {
            public string PropA => "foo";
            public int PropB => 63;
        }

        [Fact]
        public void JsonEqual_Demo1()
        {
            var actual = new MyTestClass();
            var expected = new MyTestClass();

            Lassert.JsonEqual(expected, actual); // Passes
        }

        [Fact]
        public void JsonEqual_Demo2()
        {
            var expected = new Dictionary<string, object>
            {
                { "PropA", "foo" },
                { "PropB", 63 }
            };

            var actual = new MyTestClass();

            Lassert.JsonEqual(expected, actual); // Passes
        }

        internal class AnotherTestClass
        {
            public string PropA { get; set; }
            public int PropB { get; set; }
            public System.Guid PropC { get; set; }
        }

        [Fact]
        public void MembersEqual_Demo1()
        {
            var actual = new AnotherTestClass
            {
                PropA = "foo",
                PropB = 63,
                PropC = System.Guid.NewGuid()
            };

            var expected = new AnotherTestClass
            {
                PropA = "foo",
                PropB = 63,
                PropC = System.Guid.NewGuid()
            };

            Lassert.MembersEqual(expected, actual,
                c => c.PropA,
                c => c.PropB);  // Passes, because PropC is not compared.
        }

        [Fact]
        public void MembersEqual_Demo2()
        {
            var actual = "foo";
            var expected = "bar";

            Lassert.MembersEqual(expected, actual,
                c => c.ToLowerInvariant().Contains("z")); // Passes, because false == false.
        }
    }
}
