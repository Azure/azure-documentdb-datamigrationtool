using Microsoft.DataTransfer.Basics.IO;
using Microsoft.DataTransfer.TestsCommon.SampleData;
using Microsoft.DataTransfer.TestsCommon.Settings;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.DataTransfer.TestsCommon
{
    public abstract class DataTransferTestBase
    {
        private const string TestSettingsFileName = @"TestSettings.xml";

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
            var settingsFilePath = PathHelper.Combine(AppDomain.CurrentDomain.BaseDirectory, TestSettingsFileName);
            if (!File.Exists(settingsFilePath))
                throw Errors.TestSettingsFileMissing(settingsFilePath);

            using (var reader = XmlReader.Create(settingsFilePath, new XmlReaderSettings { XmlResolver = null }))
            {
                return (ITestSettings)new XmlSerializer(typeof(TestSettings)).Deserialize(reader);
            }
        }
    }
}
