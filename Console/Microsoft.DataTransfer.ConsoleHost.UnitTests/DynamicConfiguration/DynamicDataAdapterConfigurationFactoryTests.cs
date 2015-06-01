using Microsoft.DataTransfer.ConsoleHost.DynamicConfiguration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.ConsoleHost.UnitTests.DynamicConfiguration
{
    [TestClass]
    public class DynamicDataAdapterConfigurationFactoryTests
    {
        [TestMethod]
        public void TryCreate_SimpleConfiguration_Created()
        {
            const string Property1Value = "Test value";
            const int Property2Value = 10;

            var factory = new DynamicConfigurationFactory();
            var proxy = factory.TryCreate(
                typeof(ISimpleConfiguration),
                new Dictionary<string, string>
                {
                    { "Property1", Property1Value },
                    { "Property2", Property2Value.ToString() }
                });

            Assert.IsNotNull(proxy, TestResources.NullProxyGenerated);
            Assert.IsTrue(proxy is ISimpleConfiguration, TestResources.InvalidProxyType);

            var typedProxy = (ISimpleConfiguration)proxy;

            Assert.AreEqual(Property1Value, typedProxy.Property1, TestResources.InvalidProxyPropertyValueFormat, "Property1");
            Assert.AreEqual(Property2Value, typedProxy.Property2, TestResources.InvalidProxyPropertyValueFormat, "Property2");
        }

        [TestMethod]
        public void TryCreate_DerivedConfiguration_Created()
        {
            const string Property1Value = "Another test value";
            const int Property2Value = 20;
            const string NewProperty1Value = "Derived property value";

            var factory = new DynamicConfigurationFactory();
            var proxy = factory.TryCreate(
                typeof(IDerivedConfiguration),
                new Dictionary<string, string>
                {
                    { "Property1", Property1Value },
                    { "Property2", Property2Value.ToString() },
                    { "NewProperty1", NewProperty1Value },
                });

            Assert.IsNotNull(proxy, TestResources.NullProxyGenerated);
            Assert.IsTrue(proxy is IDerivedConfiguration, TestResources.InvalidProxyType);

            var typedProxy = (IDerivedConfiguration)proxy;

            Assert.AreEqual(Property1Value, typedProxy.Property1, TestResources.InvalidProxyPropertyValueFormat, "Property1");
            Assert.AreEqual(Property2Value, typedProxy.Property2, TestResources.InvalidProxyPropertyValueFormat, "Property2");
            Assert.AreEqual(NewProperty1Value, typedProxy.NewProperty1, TestResources.InvalidProxyPropertyValueFormat, "NewProperty1");
        }

        [TestMethod]
        public void TryGetConfigurationOptions_SimpleConfiguration_OptionsReturned()
        {
            var factory = new DynamicConfigurationFactory();
            var options = factory.TryGetConfigurationOptions(typeof(ISimpleConfiguration));

            Assert.IsNotNull(options, TestResources.NullOptionsReturned);
            Assert.AreEqual(2, options.Count, TestResources.InvalidNumberOfOptions);

            Assert.IsTrue(options.Any(o => o.Key == "Property1"), TestResources.ConfigurationOptionMissingFormat, "Property1");
            Assert.IsTrue(options.Any(o => o.Key == "Property2"), TestResources.ConfigurationOptionMissingFormat, "Property2");
        }

        [TestMethod]
        public void TryGetConfigurationOptions_DerivedConfiguration_OptionsReturned()
        {
            var factory = new DynamicConfigurationFactory();
            var options = factory.TryGetConfigurationOptions(typeof(IDerivedConfiguration));

            Assert.IsNotNull(options, TestResources.NullOptionsReturned);
            Assert.AreEqual(3, options.Count, TestResources.InvalidNumberOfOptions);

            Assert.IsTrue(options.Any(o => o.Key == "Property1"), TestResources.ConfigurationOptionMissingFormat, "Property1");
            Assert.IsTrue(options.Any(o => o.Key == "Property2"), TestResources.ConfigurationOptionMissingFormat, "Property2");
            Assert.IsTrue(options.Any(o => o.Key == "NewProperty1"), TestResources.ConfigurationOptionMissingFormat, "NewProperty1");
        }

        [TestMethod]
        public void TryGetConfigurationOptions_CustomDescription_DescriptionReturned()
        {
            var factory = new DynamicConfigurationFactory();
            var options = factory.TryGetConfigurationOptions(typeof(ICustomDescriptionConfiguration));

            Assert.IsNotNull(options, TestResources.NullOptionsReturned);
            Assert.AreEqual(2, options.Count, TestResources.InvalidNumberOfOptions);

            Assert.IsTrue(options.Any(o => o.Key == "PropA"), TestResources.ConfigurationOptionMissingFormat, "PropA");
            Assert.IsTrue(options.Any(o => o.Key == "PropB"), TestResources.ConfigurationOptionMissingFormat, "PropB");

            Assert.AreEqual("Hello", options["PropA"], TestResources.InvalidConfigurationOptionDescriptionFormat, "PropA");
            Assert.AreEqual("World!", options["PropB"], TestResources.InvalidConfigurationOptionDescriptionFormat, "PropB");
        }

        [TestMethod]
        public void TryGetConfigurationOptions_CustomDescriptionFromResourcesFile_DescriptionReturned()
        {
            var factory = new DynamicConfigurationFactory();
            var options = factory.TryGetConfigurationOptions(typeof(ICustomResourcesDescriptionConfiguration));

            Assert.IsNotNull(options, TestResources.NullOptionsReturned);
            Assert.AreEqual(2, options.Count, TestResources.InvalidNumberOfOptions);

            Assert.IsTrue(options.Any(o => o.Key == "PropA"), TestResources.ConfigurationOptionMissingFormat, "PropA");
            Assert.IsTrue(options.Any(o => o.Key == "PropB"), TestResources.ConfigurationOptionMissingFormat, "PropB");

            Assert.AreEqual(DescriptionResources.PropADescription, options["PropA"], TestResources.InvalidConfigurationOptionDescriptionFormat, "PropA");
            Assert.AreEqual(DescriptionResources.PropBDescription, options["PropB"], TestResources.InvalidConfigurationOptionDescriptionFormat, "PropB");
        }

        [TestMethod]
        public void TryCreate_ConfigurationWithEnum_Parsed()
        {
            const TestEnum Property1Value = TestEnum.OptionB;
            const string Property2Value = "Hello world!";

            var factory = new DynamicConfigurationFactory();
            var proxy = factory.TryCreate(
                typeof(IEnumConfiguration),
                new Dictionary<string, string>
                {
                    { "Property1", Property1Value.ToString() },
                    { "Property2", Property2Value }
                });

            Assert.IsNotNull(proxy, TestResources.NullProxyGenerated);
            Assert.IsTrue(proxy is IEnumConfiguration, TestResources.InvalidProxyType);

            var typedProxy = (IEnumConfiguration)proxy;

            Assert.AreEqual(Property1Value, typedProxy.Property1, TestResources.InvalidProxyPropertyValueFormat, "Property1");
            Assert.AreEqual(Property2Value, typedProxy.Property2, TestResources.InvalidProxyPropertyValueFormat, "Property2");
        }

        [TestMethod]
        public void TryCreate_ConfigurationWithNullableTypes_Parsed()
        {
            const TestEnum EnumPropValue = TestEnum.OptionA;

            var factory = new DynamicConfigurationFactory();
            var proxy = factory.TryCreate(
                typeof(INullableConfiguration),
                new Dictionary<string, string>
                {
                    { "EnumProp", EnumPropValue.ToString() }
                });

            Assert.IsNotNull(proxy, TestResources.NullProxyGenerated);
            Assert.IsTrue(proxy is INullableConfiguration, TestResources.InvalidProxyType);

            var typedProxy = (INullableConfiguration)proxy;

            Assert.IsFalse(typedProxy.IntProp.HasValue, TestResources.InvalidProxyPropertyValueFormat, "Property1");
            Assert.AreEqual(EnumPropValue, typedProxy.EnumProp, TestResources.InvalidProxyPropertyValueFormat, "Property2");
        }

        [TestMethod]
        public void TryCreate_ConfigurationWithTimeSpan_Parsed()
        {
            var timeSpanPropertyValue = TimeSpan.FromSeconds(123);

            var factory = new DynamicConfigurationFactory();
            var proxy = factory.TryCreate(
                typeof(ITimeSpanConfiguration),
                new Dictionary<string, string>
                {
                    { "TimeSpanProperty", timeSpanPropertyValue.ToString() }
                });

            Assert.IsNotNull(proxy, TestResources.NullProxyGenerated);
            Assert.IsTrue(proxy is ITimeSpanConfiguration, TestResources.InvalidProxyType);

            var typedProxy = (ITimeSpanConfiguration)proxy;

            Assert.AreEqual(timeSpanPropertyValue, typedProxy.TimeSpanProperty, TestResources.InvalidProxyPropertyValueFormat, "TimeSpanProperty");
        }

        [TestMethod]
        public void TryCreate_ConfigurationWithCollection_Parsed()
        {
            var collectionPropertyValue = new[] { @"Va\lue1;", ";Value2", "Val;ue3", @"Value\" };

            var factory = new DynamicConfigurationFactory();
            var proxy = factory.TryCreate(
                typeof(ICollectionConfiguration),
                new Dictionary<string, string>
                {
                    { "CollectionProperty", String.Join(";", collectionPropertyValue.Select(v => v.Replace(@"\", @"\\").Replace(";", @"\;"))) }
                });

            Assert.IsNotNull(proxy, TestResources.NullProxyGenerated);
            Assert.IsTrue(proxy is ICollectionConfiguration, TestResources.InvalidProxyType);

            var typedProxy = (ICollectionConfiguration)proxy;

            CollectionAssert.AreEquivalent(collectionPropertyValue, typedProxy.CollectionProperty.ToArray(),
                TestResources.InvalidProxyPropertyValueFormat, "CollectionProperty");
        }

        [TestMethod]
        public void TryCreate_ConfigurationWithCollection_UnnecessaryEscapeCharactersPreservedInTheOutput()
        {
            var factory = new DynamicConfigurationFactory();
            var proxy = factory.TryCreate(
                typeof(ICollectionConfiguration),
                new Dictionary<string, string>
                {
                    { "CollectionProperty", @"Value1;Valu\e2;Val\\ue3" }
                });

            Assert.IsNotNull(proxy, TestResources.NullProxyGenerated);
            Assert.IsTrue(proxy is ICollectionConfiguration, TestResources.InvalidProxyType);

            var typedProxy = (ICollectionConfiguration)proxy;

            CollectionAssert.AreEquivalent(new[] { "Value1", @"Valu\e2", @"Val\ue3" }, typedProxy.CollectionProperty.ToArray(),
                TestResources.InvalidProxyPropertyValueFormat, "CollectionProperty");
        }
    }
}
