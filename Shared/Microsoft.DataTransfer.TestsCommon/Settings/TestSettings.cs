using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Microsoft.DataTransfer.TestsCommon.Settings
{
    [XmlRoot("Settings")]
    public sealed class TestSettings : ITestSettings
    {
        [XmlElement]
        public string DocumentDbConnectionStringFormat { get; set; }

        [XmlElement]
        public string SqlConnectionString { get; set; }

        [XmlElement]
        public string MongoConnectionString { get; set; }

        [XmlElement]
        public string AzureStorageConnectionString { get; set; }

        [XmlElement]
        public string RavenDbConnectionStringFormat { get; set; }

        [XmlElement]
        public string DynamoDbConnectionString { get; set; }

        [XmlElement]
        public string HBaseConnectionString { get; set; }

        public string DocumentDbConnectionString(string databaseName)
        {
            return String.Format(CultureInfo.InvariantCulture, DocumentDbConnectionStringFormat, databaseName);
        }

        public string RavenDbConnectionString(string databaseName)
        {
            return String.Format(CultureInfo.InvariantCulture, RavenDbConnectionStringFormat, databaseName);
        }
    }
}
