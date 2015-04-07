using Microsoft.DataTransfer.WpfHost.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.UnitTests.Model
{
    [TestClass]
    public sealed class CommandLineProviderTests
    {
        [TestMethod]
        public void Get_AllConfigurationPopulated_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                "TestSource",
                new Dictionary<string, string>
                {
                    { "SourceArg1", "value" },
                    { "SourceArg2", "42" }
                },
                "TestSink",
                new Dictionary<string, string>
                {
                    { "SinkArg1", "100" },
                    { "SinkArg2", "value" }
                });

            Assert.AreEqual(96, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:TestSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg1:value", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg2:42", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:TestSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:100", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg2:value", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_NoSourceArguments_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                "AnotherSource", null,
                "SomeSink",
                new Dictionary<string, string>
                {
                    { "SinkArg1", "351" },
                    { "SinkArg2", "hello" }
                });

            Assert.AreEqual(63, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:AnotherSource", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:SomeSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:351", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg2:hello", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_NoSinkArguments_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                "SomeSource",
                new Dictionary<string, string>
                {
                    { "SourceArg1", "world" },
                    { "SourceArg2", "50" }
                },
                "AnotherSink", null);

            Assert.AreEqual(66, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:SomeSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg1:world", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg2:50", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:AnotherSink", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_SpacesInArgument_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                "TestSource",
                new Dictionary<string, string>
                {
                    { "SourceArg1", "value with spaces" },
                },
                "TestSink",
                new Dictionary<string, string>
                {
                    { "SinkArg1", "42" },
                });

            Assert.AreEqual(74, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:TestSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg1:\"value with spaces\"", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:TestSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:42", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_QuotesInArgument_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                "TestSource",
                new Dictionary<string, string>
                {
                    { "SourceArg1", "42" },
                },
                "TestSink",
                new Dictionary<string, string>
                {
                    { "SinkArg1", "quote\"here" },
                });

            Assert.AreEqual(67, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:TestSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg1:42", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:TestSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:quote\"\"\"here", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_QuotesAndSpacesInArgument_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                "TestSource",
                new Dictionary<string, string>
                {
                    { "SourceArg1", "42" },
                },
                "TestSink",
                new Dictionary<string, string>
                {
                    { "SinkArg1", "quotes\" and \"spaces" },
                });

            Assert.AreEqual(80, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:TestSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg1:42", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:TestSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:\"quotes\"\"\" and \"\"\"spaces\"", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_SwitchArgument_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                "TestSource",
                new Dictionary<string, string>
                {
                    { "SwitchArg", null },
                },
                "TestSink",
                new Dictionary<string, string>
                {
                    { "SinkArg1", "whatever" },
                });

            Assert.AreEqual(59, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:TestSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SwitchArg", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:TestSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:whatever", TestResources.CommandLineArgumentMissing);
        }
    }
}
