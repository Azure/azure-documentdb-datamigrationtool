using Microsoft.DataTransfer.Basics;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.DataTransfer.ConsoleHost.DynamicConfiguration
{
    sealed class PropertyNameAndTypeEqualityComparer : IEqualityComparer<PropertyInfo>
    {
        public static readonly PropertyNameAndTypeEqualityComparer Instance = new PropertyNameAndTypeEqualityComparer();

        public bool Equals(PropertyInfo x, PropertyInfo y)
        {
            if (x == null || y == null)
                return x == y;

            return x.Name == y.Name && x.PropertyType == y.PropertyType;
        }

        public int GetHashCode(PropertyInfo obj)
        {
            return obj == null ? 0 : obj.Name.GetHashCode() ^ obj.PropertyType.GetHashCode();
        }
    }
}
