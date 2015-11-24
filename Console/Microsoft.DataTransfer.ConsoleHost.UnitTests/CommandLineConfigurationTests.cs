using Microsoft.DataTransfer.ConsoleHost.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ConsoleHost.UnitTests
{
    [TestClass]
    public class CommandLineConfigurationTests
    {
        [TestMethod]
        public void Parse_SourceAndTargetNameSet_ConfigurationParsed()
        {
            var configuration = CommandLineConfiguration.Parse(new[]
                {
                    "/s:TestSource",
                    "/t:TestTarget"
                });

            Assert.AreEqual("TestSource", configuration.SourceName, TestResources.InvalidSourceNameParsed);
            Assert.AreEqual("TestTarget", configuration.TargetName, TestResources.InvalidTargetNameParsed);

            Assert.AreEqual(0, configuration.InfrastructureConfiguration.Count, TestResources.InvalidInfrastructureConfigurationParsed);
            Assert.AreEqual(0, configuration.SourceConfiguration.Count, TestResources.InvalidSourceConfigurationParsed);
            Assert.AreEqual(0, configuration.TargetConfiguration.Count, TestResources.InvalidTargetConfigurationParsed);
        }

        [TestMethod]
        public void Parse_SimpleSourceConfigurationSet_ConfigurationParsed()
        {
            var configuration = CommandLineConfiguration.Parse(new[]
                {
                    "/s:TestSource",
                    "/s.Property1:value1",
                    "/s.Property2:value2"
                });

            Assert.AreEqual("TestSource", configuration.SourceName, TestResources.InvalidSourceNameParsed);
            Assert.IsNull(configuration.TargetName, TestResources.InvalidTargetNameParsed);

            Assert.AreEqual(0, configuration.InfrastructureConfiguration.Count, TestResources.InvalidInfrastructureConfigurationParsed);

            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "Property1", "value1" },
                    { "Property2", "value2" }
                },
                configuration.SourceConfiguration.ToArray(),
                TestResources.InvalidSourceConfigurationParsed);

            Assert.AreEqual(0, configuration.TargetConfiguration.Count, TestResources.InvalidTargetConfigurationParsed);
        }

        [TestMethod]
        public void Parse_CompexSourceConfigurationSet_ConfigurationParsed()
        {
            var configuration = CommandLineConfiguration.Parse(new[]
                {
                    "/s:TestSource",
                    "/s.Property1:semi-colon : \"quotes\" and spaces",
                    "/s.Property2:/s.NotAProperty",
                    "/s.SomeSwitch"
                });

            Assert.AreEqual("TestSource", configuration.SourceName, TestResources.InvalidSourceNameParsed);
            Assert.IsNull(configuration.TargetName, TestResources.InvalidTargetNameParsed);

            Assert.AreEqual(0, configuration.InfrastructureConfiguration.Count, TestResources.InvalidInfrastructureConfigurationParsed);

            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "Property1", "semi-colon : \"quotes\" and spaces" },
                    { "Property2", "/s.NotAProperty" },
                    { "SomeSwitch", Boolean.TrueString }
                },
                configuration.SourceConfiguration.ToArray(),
                TestResources.InvalidSourceConfigurationParsed);

            Assert.AreEqual(0, configuration.TargetConfiguration.Count, TestResources.InvalidTargetConfigurationParsed);
        }

        [TestMethod]
        public void Parse_SimpleTargetConfigurationSet_ConfigurationParsed()
        {
            var configuration = CommandLineConfiguration.Parse(new[]
                {
                    "/t:TestTarget",
                    "/t.Property1:value1",
                    "/t.Property2:value2"
                });

            Assert.IsNull(configuration.SourceName, TestResources.InvalidSourceNameParsed);
            Assert.AreEqual("TestTarget", configuration.TargetName, TestResources.InvalidTargetNameParsed);

            Assert.AreEqual(0, configuration.InfrastructureConfiguration.Count, TestResources.InvalidInfrastructureConfigurationParsed);
            Assert.AreEqual(0, configuration.SourceConfiguration.Count, TestResources.InvalidSourceConfigurationParsed);

            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "Property1", "value1" },
                    { "Property2", "value2" }
                },
                configuration.TargetConfiguration.ToArray(),
                TestResources.InvalidTargetConfigurationParsed);
        }

        [TestMethod]
        public void Parse_ComplexTargetConfigurationSet_ConfigurationParsed()
        {
            var configuration = CommandLineConfiguration.Parse(new[]
                {
                    "/t:TestTarget",
                    "/t.Property1:with semi-colon :, spaces",
                    "/t.Property2:value with \"quotes\"",
                    "/t.Switch"
                });

            Assert.IsNull(configuration.SourceName, TestResources.InvalidSourceNameParsed);
            Assert.AreEqual("TestTarget", configuration.TargetName, TestResources.InvalidTargetNameParsed);

            Assert.AreEqual(0, configuration.InfrastructureConfiguration.Count, TestResources.InvalidInfrastructureConfigurationParsed);
            Assert.AreEqual(0, configuration.SourceConfiguration.Count, TestResources.InvalidSourceConfigurationParsed);

            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "Property1", "with semi-colon :, spaces" },
                    { "Property2", "value with \"quotes\"" },
                    { "Switch", Boolean.TrueString }
                },
                configuration.TargetConfiguration.ToArray(),
                TestResources.InvalidTargetConfigurationParsed);
        }

        [TestMethod]
        public void Parse_BothSourceAndTargetConfigurationSet_ConfigurationParsed()
        {
            var configuration = CommandLineConfiguration.Parse(new[]
                {
                    "/s:SomeSource",
                    "/s.Property1:source value",
                    "/s.IntegerProperty:42",
                    "/t:SomeTarget",
                    "/t.Property1:some target value",
                    "/t.Property2:another target value"
                });

            Assert.AreEqual("SomeSource", configuration.SourceName, TestResources.InvalidSourceNameParsed);
            Assert.AreEqual("SomeTarget", configuration.TargetName, TestResources.InvalidTargetNameParsed);

            Assert.AreEqual(0, configuration.InfrastructureConfiguration.Count, TestResources.InvalidInfrastructureConfigurationParsed);

            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "Property1", "source value" },
                    { "IntegerProperty", "42" }
                },
                configuration.SourceConfiguration.ToArray(),
                TestResources.InvalidSourceConfigurationParsed);

            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "Property1", "some target value" },
                    { "Property2", "another target value" }
                },
                configuration.TargetConfiguration.ToArray(),
                TestResources.InvalidTargetConfigurationParsed);
        }

        [TestMethod]
        public void Parse_SimpleInfrastructureConfigurationSet_ConfigurationParsed()
        {
            var configuration = CommandLineConfiguration.Parse(new[]
                {
                    "/Property1:Hello World!",
                    "/Switch",
                    "/Property2:value",
                    "/source:not a source"
                });

            Assert.IsNull(configuration.SourceName, TestResources.InvalidSourceNameParsed);
            Assert.IsNull(configuration.TargetName, TestResources.InvalidTargetNameParsed);

            CollectionAssert.AreEquivalent(
                new Dictionary<string, string>
                {
                    { "Property1", "Hello World!" },
                    { "Switch", Boolean.TrueString },
                    { "Property2", "value" },
                    { "source", "not a source" }
                },
                configuration.InfrastructureConfiguration.ToArray(),
                TestResources.InvalidInfrastructureConfigurationParsed);

            Assert.AreEqual(0, configuration.SourceConfiguration.Count, TestResources.InvalidSourceConfigurationParsed);
            Assert.AreEqual(0, configuration.TargetConfiguration.Count, TestResources.InvalidTargetConfigurationParsed);
        }
    }
}
