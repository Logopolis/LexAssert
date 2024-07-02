using System;
using System.Text.Json;

using Xunit;
using Xunit.Sdk;

namespace LexAssert
{
    public class Lassert : Assert
    {
        public static void JsonEqual(object expected, object actual)
        {
            var expectedJson = JsonSerializer.Serialize(expected);
            var actualJson = JsonSerializer.Serialize(actual);

            if(expectedJson != actualJson)
            {
                throw EqualException.ForMismatchedValues(expectedJson, actualJson, "Lassert.JsonEqual() Failure");
            }
        }

        public static void MembersEqual<T>(
            T expected,
            T actual,
            params Func<T, object>[] memberSelectors)
        {
            foreach (var memberSelector in memberSelectors)
            {
                Equal(memberSelector(expected), memberSelector(actual));
            }
        }
    }
}
