using LexAssert.EqualityComparers;
using System;
using Xunit;

namespace LexAssert
{
    public class LexAssert : Assert
    {
        public static void JsonEqual<T>(T expected, T actual)
        {
            Equal(expected, actual, new JsonEqualityComparer<T>());
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
