using Xunit;

namespace LexAssert.Tests.Demo
{
    public class LexAssertDemos
    {
        internal class MyTestClass
        {
            string PropA => "foo";
            int PropB => 63;
        }

        [Fact]
        public void JsonEqual_Demo1()
        {
            var actual = new MyTestClass();
            var expected = new MyTestClass();

            LexAssert.JsonEqual(expected, actual); // Passes
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

            LexAssert.MembersEqual(expected, actual,
                c => c.PropA,
                c => c.PropB);  // Passes, because PropC is not compared.
        }

        public void MembersEqual_Demo2()
        {
            var actual = "foo";
            var expected = "bar";

            LexAssert.MembersEqual(expected, actual,
                c => c.ToLowerInvariant().Contains("z")); // Passes, because false == false.
        }
    }
}
