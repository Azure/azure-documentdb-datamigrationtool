using Microsoft.DataTransfer.TestsCommon.SampleData;
using Microsoft.DataTransfer.TestsCommon.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.DataTransfer.TestsCommon
{
    public abstract class DataTransferTestBase
    {
        public TestContext TestContext { get; set; }

        private Lazy<ITestSettings> testSettings;

        protected ITestSettings Settings
        {
            get { return testSettings.Value; }
        }

        protected ISampleDataProvider SampleData { get; private set; }

        public DataTransferTestBase()
        {
            testSettings = new Lazy<ITestSettings>(LoadSettings, true);
            SampleData = new SampleDataProvider();
        }

        private ITestSettings LoadSettings()
        {
            return new TestSettings
            {
                DocumentDbConnectionString = GetConnectionString(nameof(ITestSettings.DocumentDbConnectionString)),
                MongoConnectionString = GetConnectionString(nameof(ITestSettings.MongoConnectionString)),
                AzureStorageConnectionString = GetConnectionString(nameof(ITestSettings.AzureStorageConnectionString)),
                SqlConnectionString = GetConnectionString(nameof(ITestSettings.SqlConnectionString)),
            };
        }

        private string GetConnectionString(string name)
        {
            return (TestContext.Properties[name] ?? String.Empty).ToString();
        }
    }
}
