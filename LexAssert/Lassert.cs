using LexAssert.Exceptions;

using Newtonsoft.Json;

using System;
using Xunit;

namespace LexAssert
{
    public class Lassert : Assert
    {
        public static void JsonEqual(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);

            if(expectedJson != actualJson)
            {
                throw new JsonEqualException(expectedJson, actualJson);
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
