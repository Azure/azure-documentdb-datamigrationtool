using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    sealed class RavenJObjectDataItem : IDataItem
    {
        private RavenJObject data;

        public RavenJObjectDataItem(RavenJObject data)
        {
            Guard.NotNull("data", data);
            this.data = data;
        }

        public IEnumerable<string> GetFieldNames()
        {
            return data.Keys.Where(k => !IsInternalField(k));
        }

        public object GetValue(string fieldName)
        {
            Guard.NotNull("fieldName", fieldName);

            RavenJToken token;
            if (IsInternalField(fieldName) || !data.TryGetValue(fieldName, out token))
                throw CommonErrors.DataItemFieldNotFound(fieldName);

            return GetValue(token);
        }

        private static bool IsInternalField(string fieldName)
        {
            // Another way could be to use DynamicJsonObject, but it does not have any strongly-typed way to enumerate property names
            return !String.IsNullOrEmpty(fieldName) && fieldName[0] == '$';
        }

        private object GetValue(RavenJToken token)
        {
            if (token is RavenJObject)
                return new RavenJObjectDataItem((RavenJObject)token);

            if (token is RavenJArray)
            {
                var jArray = (RavenJArray)token;
                var result = new object[jArray.Length];
                for (var index = 0; index < jArray.Length; ++index)
                    result[index] = GetValue(jArray[index]);
                return result;
            }

            return token.Value<object>();
        }
    }
}
