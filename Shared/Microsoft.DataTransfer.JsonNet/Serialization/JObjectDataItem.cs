using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.JsonNet.Serialization
{
    /// <summary>
    /// Represents <see cref="JObject"/> that can be transferred between source and sink.
    /// </summary>
    public class JObjectDataItem : IDataItem
    {
        private JObject data;

        /// <summary>
        /// Creates a new intance of <see cref="JObjectDataItem"/>.
        /// </summary>
        /// <param name="data"><see cref="JObject"/> that holds the data.</param>
        public JObjectDataItem(JObject data)
        {
            Guard.NotNull("data", data);
            this.data = data;
        }

        /// <summary>
        /// Provides collection of field names available in the <see cref="JObject"/>.
        /// </summary>
        /// <returns>Collection of field names.</returns>
        public IEnumerable<string> GetFieldNames()
        {
            return data.Properties().Select(p => p.Name);
        }


        /// <summary>
        /// Provides a value of the specified <see cref="JObject"/> field.
        /// </summary>
        /// <param name="fieldName">Name of <see cref="JObject"/> field.</param>
        /// <returns>Value of the field.</returns>
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
