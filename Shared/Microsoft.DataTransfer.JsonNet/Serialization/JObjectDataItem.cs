using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.JsonNet.Serialization
{
    sealed class JObjectDataItem : IDataItem
    {
        private JObject data;

        public JObjectDataItem(JObject data)
        {
            Guard.NotNull("data", data);
            this.data = data;
        }

        public IEnumerable<string> GetFieldNames()
        {
            return data.Properties().Select(p => p.Name);
        }

        public object GetValue(string fieldName)
        {
            Guard.NotNull("fieldName", fieldName);

            JToken token;
            if (!data.TryGetValue(fieldName, out token))
                throw CommonErrors.DataItemFieldNotFound(fieldName);

            return GetValue(token);
        }

        private object GetValue(JToken token)
        {
            if (token is JObject)
                return new JObjectDataItem((JObject)token);

            if (token is JArray)
            {
                var jArray = (JArray)token;
                var result = new object[jArray.Count];
                for (var index = 0; index < jArray.Count; ++index)
                    result[index] = GetValue(jArray[index]);
                return result;
            }

            return token.ToObject<object>();
        }
    }
}
