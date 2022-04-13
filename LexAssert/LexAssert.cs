﻿using LexAssert.EqualityComparers;

using Xunit;

namespace LexAssert
{
    public class LexAssert : Assert
    {
        public void JsonEqual<T>(T expected, T actual)
        {
            Equal(expected, actual, new JsonEqualityComparer<T>());
        }
    }
}
