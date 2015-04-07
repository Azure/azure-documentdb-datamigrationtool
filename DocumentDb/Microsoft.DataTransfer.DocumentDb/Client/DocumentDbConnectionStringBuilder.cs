using Microsoft.DataTransfer.Basics;
using System;
using System.Data.Common;
using System.Globalization;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    sealed class DocumentDbConnectionStringBuilder : DbConnectionStringBuilder, IDocumentDbConnectionSettings
    {
        public string AccountEndpoint
        {
            get { return GetValue<string>("AccountEndpoint"); }
            set { base["AccountEndpoint"] = value; }
        }

        public string AccountKey
        {
            get { return GetValue<string>("AccountKey"); }
            set { base["AccountKey"] = value; }
        }

        public string Database
        {
            get { return GetValue<string>("Database"); }
            set { base["Database"] = value; }
        }

        public static DocumentDbConnectionStringBuilder Parse(string connectionString)
        {
            Guard.NotEmpty("connectionString", connectionString);

            return new DocumentDbConnectionStringBuilder
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
