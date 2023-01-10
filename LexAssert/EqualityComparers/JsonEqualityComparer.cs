using System.Collections.Generic;
using System.Text.Json;

namespace LexAssert.EqualityComparers
{
    public class JsonEqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return JsonSerializer.Serialize(x) == JsonSerializer.Serialize(y);
        }

        public int GetHashCode(T obj)
        {
            return JsonSerializer.Serialize(obj).GetHashCode();
        }
    }
}
