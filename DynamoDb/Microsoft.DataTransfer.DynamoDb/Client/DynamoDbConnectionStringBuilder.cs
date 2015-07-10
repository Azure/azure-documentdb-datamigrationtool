using Microsoft.DataTransfer.Basics;
using System;
using System.Data.Common;
using System.Globalization;

namespace Microsoft.DataTransfer.DynamoDb.Client
{
    sealed class DynamoDbConnectionStringBuilder : DbConnectionStringBuilder, IDynamoDbConnectionSettings
    {
        public string ServiceUrl
        {
            get { return GetValue<string>("ServiceURL"); }
            set { base["ServiceURL"] = value; }
        }

        public string AccessKey
        {
            get { return GetValue<string>("AccessKey"); }
            set { base["AccessKey"] = value; }
        }

        public string SecretKey
        {
            get { return GetValue<string>("SecretKey"); }
            set { base["SecretKey"] = value; }
        }

        public static DynamoDbConnectionStringBuilder Parse(string connectionString)
        {
            Guard.NotEmpty("connectionString", connectionString);

            return new DynamoDbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
        }

        private T GetValue<T>(string name)
        {
            object value;
            return TryGetValue(name, out value)
                ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
                : default(T);
        }
    }
}
