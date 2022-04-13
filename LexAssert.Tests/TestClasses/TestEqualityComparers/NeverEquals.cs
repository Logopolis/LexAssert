using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LexAssert.Tests.TestClasses.TestEqualityComparers
{
    internal class NeverEquals<T> : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y)
        {
            return false;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return new Random().Next();
        }
    }
}
