using Microsoft.DataTransfer.CsvFile.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Microsoft.DataTransfer.CsvFile.UnitTests
{
    [TestClass]
    public class CsvStreamReaderTests
    {
        [TestMethod]
        [DeploymentItem(@"TestData\SimpleQuotedValues.csv")]
        public void Read_SimpleQuotedValues_AllValuesRead()
        {
            ReadAndVerify(
                "SimpleQuotedValues.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "Jones", "Senior Director", "buyer@salesforcesample.com" },
                    new object[] { "Ian", "Dury", "Chief Imagineer", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\ComplexQuotedValues.csv")]
        public void Read_ComplexQuotedValues_AllValuesRead()
        {
            ReadAndVerify(
                "ComplexQuotedValues.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "Jones", "Senior\" Director", "buyer@salesforcesample.com" },
                    new object[] { "Ian", "Dury", "Chief\r\n\" Imagineer", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\IntegerValues.csv")]
        public void Read_IntegerValues_AllValuesRead()
        {
            ReadAndVerify(
                "IntegerValues.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "Jones", (long)10, "buyer@salesforcesample.com" },
                    new object[] { "Ian", "Dury", "20", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\FloatingPointValues.csv")]
        public void Read_FloatingPointValues_AllValuesRead()
        {
            ReadAndVerify(
                "FloatingPointValues.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "Jones", (double)10.1, "buyer@salesforcesample.com" },
                    new object[] { "Ian", "Dury", "20.2", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\BooleanValues.csv")]
        public void Read_BooleanValues_AllValuesRead()
        {
            ReadAndVerify(
                "BooleanValues.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "Jones", true, "buyer@salesforcesample.com" },
                    new object[] { "Ian", "Dury", "false", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\DateTimeValues.csv")]
        public void Read_DateTimeValues_AllValuesRead()
        {
            ReadAndVerify(
                "DateTimeValues.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "Jones", new DateTime(2015, 4, 15, 20, 0, 15, DateTimeKind.Utc), "buyer@salesforcesample.com" },
                    new object[] { "Ian", "Dury", "2010-02-01 02:10:00", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\UnquotedNulls.csv")]
        public void Read_UnquotedNulls_NullsReadAsNullsByDefault()
        {
            ReadAndVerify(
                "UnquotedNulls.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", null, "Senior Director", "buyer@salesforcesample.com" },
                    new object[] { "Ian", "NULL", null, "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\UnquotedNulls.csv")]
        public void Read_UnquotedNulls_NullsReadAsStrings()
        {
            ReadAndVerify(
                "UnquotedNulls.csv",
                new CsvReaderConfiguration
                {
                    IgnoreUnquotedNulls = true,
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "NULL", "Senior Director", "buyer@salesforcesample.com" },
                    new object[] { "Ian", "NULL", "null", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\EmptyLines.csv")]
        public void Read_EmptyLines_TreatedAsSingleNullValues()
        {
            ReadAndVerify(
                "EmptyLines.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { null },
                    new object[] { null }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\EmptySpaceQuotedValues.csv")]
        public void Read_EmptySpaceInQuotes_EmptySpaceIsNotTrimmedByDefault()
        {
            ReadAndVerify(
                "EmptySpaceQuotedValues.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "  Jones  ", "Senior Director", "buyer@salesforcesample.com" },
                    new object[] { "Ian", " Dury   ", "Chief Imagineer", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\EmptySpaceQuotedValues.csv")]
        public void Read_EmptySpaceInQuotes_EmptySpaceIsTrimmed()
        {
            ReadAndVerify(
                "EmptySpaceQuotedValues.csv",
                new CsvReaderConfiguration
                {
                    TrimQuoted = true,
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "Jones", "Senior Director", "buyer@salesforcesample.com" },
                    new object[] { "Ian", "Dury", "Chief Imagineer", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\EmptySpaceUnquotedValues.csv")]
        public void Read_EmptySpaceInUnquotedValues_EmptySpaceIsTrimmed()
        {
            ReadAndVerify(
                "EmptySpaceUnquotedValues.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = CultureInfo.InvariantCulture
                },
                new[]
                {
                    new object[] { "Tom", "Jones", "Senior Director", "buyer@salesforcesample.com" },
                    new object[] { "Ian", "Dury", "Chief Imagineer", "cto@salesforcesample.com" }
                });
        }

        [TestMethod]
        [DeploymentItem(@"TestData\CustomCulture.csv")]
        public void Read_CustomCulture_AllValuesRead()
        {
            var customCulture = new CultureInfo(CultureInfo.InvariantCulture.Name);
            customCulture.TextInfo.ListSeparator = "|";
            customCulture.NumberFormat.NumberDecimalSeparator = ",";
            customCulture.NumberFormat.NumberGroupSeparator = " ";

            ReadAndVerify(
                "CustomCulture.csv",
                new CsvReaderConfiguration
                {
                    ParserCulture = customCulture
                },
                new[]
                {
                    new object[] { "Jessica", "Simpson", (double)10.2, "jess@sample.com" },
                    new object[] { "Mark", "Doe", false, "mark@sample.com" },
                    new object[] { "Chris", "Johnson", (double)1050.10, "chris@sample.com" },
                });
        }

        private static void ReadAndVerify(string inputFileName, CsvReaderConfiguration configuration, object[][] expectedRows)
        {
            using (var csvReader = new CsvReader(new StreamReader(inputFileName), configuration))
            {
                IReadOnlyList<object> row;
                while ((row = csvReader.Read()) != null)
                {
                    Assert.IsTrue(csvReader.Row <= expectedRows.Length, TestResources.ExtraRowRead);
                    CollectionAssert.AreEqual(expectedRows[csvReader.Row - 1], row.ToArray(), TestResources.InvalidRowRead);
                }
            }
        }
    }
}
