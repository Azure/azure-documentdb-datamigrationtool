using Microsoft.DataTransfer.Basics;
using System;
using System.Data.Common;
using System.Globalization;

namespace Microsoft.DataTransfer.HBase.Client
{
    sealed class StargateConnectionStringBuilder : DbConnectionStringBuilder
    {
        public string ServiceURL
        {
            get { return GetValue<string>("ServiceURL"); }
            set { base["ServiceURL"] = value; }
        }

        public string Username
        {
            get { return GetValue<string>("Username"); }
            set { base["Username"] = value; }
        }

        public string Password
        {
            get { return GetValue<string>("Password"); }
            set { base["Password"] = value; }
        }

        public static StargateConnectionStringBuilder Parse(string connectionString)
        {
            Guard.NotEmpty("connectionString", connectionString);

            return new StargateConnectionStringBuilder
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
