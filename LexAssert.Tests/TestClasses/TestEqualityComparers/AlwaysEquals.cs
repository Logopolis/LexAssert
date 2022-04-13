using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LexAssert.Tests.TestClasses.TestEqualityComparers
{
    internal class AlwaysEquals<T> : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y)
        {
            return true;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return 1;
        }
    }
}
