using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.Core.Autofac
{
    static class TypesHelper
    {
        public static bool IsOpenGenericType(Type type, object openGenericType)
        {
            Guard.NotNull("type", type);

            if (type.IsGenericType)
                type = type.GetGenericTypeDefinition();

            return type.Equals(openGenericType);
        }
    }
}
