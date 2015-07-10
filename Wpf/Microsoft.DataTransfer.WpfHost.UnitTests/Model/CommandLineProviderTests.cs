using Microsoft.DataTransfer.WpfHost.Model;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

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
                Mocks.Of<IInfrastructureConfiguration>(c =>
                    c.ErrorLog == "errors.csv" &&
                    c.OverwriteErrorLog == false).First(),
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

            Assert.AreEqual(117, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/ErrorLog:errors.csv", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/s:TestSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg1:value", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg2:42", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:TestSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:100", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg2:value", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_NoInfrastructureArguments_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                null,
                "AnotherSource",
                new Dictionary<string, string>
                {
                    { "SourceArg1", "value42" },
                    { "SourceArg2", "1" }
                },
                "SomeSink",
                new Dictionary<string, string>
                {
                    { "SinkArg1", "351" },
                    { "SinkArg2", "hello" }
                });

            Assert.AreEqual(100, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:AnotherSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg1:value42", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SourceArg2:1", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:SomeSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:351", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg2:hello", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_NoSourceArguments_Generated()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                Mocks.Of<IInfrastructureConfiguration>(c =>
                    c.ErrorLog == "somefile.csv" &&
                    c.OverwriteErrorLog == false).First(),
                "AnotherSource", null,
                "SomeSink",
                new Dictionary<string, string>
                {
                    { "SinkArg1", "351" },
                    { "SinkArg2", "hello" }
                });

            Assert.AreEqual(85, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/ErrorLog:somefile.csv", TestResources.CommandLineArgumentMissing);

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
                Mocks.Of<IInfrastructureConfiguration>(c =>
                    c.ErrorLog == "otherfile.csv" &&
                    c.OverwriteErrorLog == false).First(),
                "SomeSource",
                new Dictionary<string, string>
                {
                    { "SourceArg1", "world" },
                    { "SourceArg2", "50" }
                },
                "AnotherSink", null);

            Assert.AreEqual(90, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/ErrorLog:otherfile.csv", TestResources.CommandLineArgumentMissing);

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
                Mocks.Of<IInfrastructureConfiguration>(c =>
                    c.ErrorLog == @"d:\path with spaces\file.csv" &&
                    c.OverwriteErrorLog == false).First(),
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

            Assert.AreEqual(115, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/ErrorLog:\"d:\\path with spaces\\file.csv\"", TestResources.CommandLineArgumentMissing);

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
                Mocks.Of<IInfrastructureConfiguration>(c =>
                    c.ErrorLog == @"d:\pathwith""quotes""\file.csv" &&
                    c.OverwriteErrorLog == false).First(),
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

            Assert.AreEqual(110, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/ErrorLog:d:\\pathwith\"\"\"quotes\"\"\"\\file.csv", TestResources.CommandLineArgumentMissing);

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
                Mocks.Of<IInfrastructureConfiguration>(c =>
                    c.ErrorLog == @"d:\path with spaces and ""quotes""\file.csv" &&
                    c.OverwriteErrorLog == false).First(),
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

            Assert.AreEqual(138, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/ErrorLog:\"d:\\path with spaces and \"\"\"quotes\"\"\"\\file.csv\"", TestResources.CommandLineArgumentMissing);

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
                Mocks.Of<IInfrastructureConfiguration>(c =>
                    c.ErrorLog == "file.csv" &&
                    c.OverwriteErrorLog == true).First(),
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

            Assert.AreEqual(97, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/ErrorLog:file.csv", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/OverwriteErrorLog", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/s:TestSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.SwitchArg", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:TestSink", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/t.SinkArg1:whatever", TestResources.CommandLineArgumentMissing);
        }

        [TestMethod]
        public void Get_ArgumentWithNewLines_NewLinesRemoved()
        {
            var provider = new CommandLineProvider();

            var commandLine = provider.Get(
                null,
                "TestSource",
                new Dictionary<string, string>
                {
                    { "TestArg", "Hello" + Environment.NewLine + "World!" },
                },
                "TestSink", null);

            Assert.AreEqual(52, commandLine.Length, TestResources.InvalidCommandLineLength);

            StringAssert.Contains(commandLine, "/s:TestSource", TestResources.CommandLineArgumentMissing);
            StringAssert.Contains(commandLine, "/s.TestArg:\"Hello World!\"", TestResources.CommandLineArgumentMissing);

            StringAssert.Contains(commandLine, "/t:TestSink", TestResources.CommandLineArgumentMissing);
        }
    }
}
