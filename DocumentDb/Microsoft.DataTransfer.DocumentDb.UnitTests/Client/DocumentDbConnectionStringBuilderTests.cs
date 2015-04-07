using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Client
{
    [TestClass]
    public class DocumentDbConnectionStringBuilderTests
    {
        [TestMethod]
        public void AccountEndpointAccountKey_TestValues_Appended()
        {
            var builder = new DocumentDbConnectionStringBuilder();
            builder.AccountEndpoint = "http://TestDocumentDbUrl.net";
            builder.AccountKey = "SecretKey";

            Assert.AreEqual(65, builder.ConnectionString.Length, TestResources.AdditionalParametersInConnectionString);

            StringAssert.Contains(builder.ConnectionString, "AccountEndpoint=http://TestDocumentDbUrl.net", TestResources.InvalidAccountEndpointInConnectionString);
            StringAssert.Contains(builder.ConnectionString, "AccountKey=SecretKey", TestResources.InvalidAccountKeyInConnectionString);
        }

        [TestMethod]
        public void AccountEndpointAccountKeyDatabase_TestValues_Appended()
        {
            var builder = new DocumentDbConnectionStringBuilder();
            builder.AccountEndpoint = "http://TestDocumentDbUrl.net";
            builder.AccountKey = "SecretKey";
            builder.Database = "SomeRandomDatabase";

            Assert.AreEqual(93, builder.ConnectionString.Length, TestResources.AdditionalParametersInConnectionString);

            StringAssert.Contains(builder.ConnectionString, "AccountEndpoint=http://TestDocumentDbUrl.net", TestResources.InvalidAccountEndpointInConnectionString);
            StringAssert.Contains(builder.ConnectionString, "AccountKey=SecretKey", TestResources.InvalidAccountKeyInConnectionString);
            StringAssert.Contains(builder.ConnectionString, "Database=SomeRandomDatabase", TestResources.InvalidDatabaseNameInConnectionString);
        }

        [TestMethod]
        public void Parse_ValidConnectionString_Parsed()
        {
            var builder = DocumentDbConnectionStringBuilder.Parse("AccountEndpoint=http://Test2DocumentDbUrl.net;AccountKey=SuperSecret");

            Assert.AreEqual("http://Test2DocumentDbUrl.net", builder.AccountEndpoint, TestResources.InvalidAccountEndpointParsedFromConnectionString);
            Assert.AreEqual("SuperSecret", builder.AccountKey, TestResources.InvalidAccountKeyParsedFromConnectionString);
            Assert.IsNull(builder.Database, TestResources.InvalidDatabaseNameParsedFromConnectionString);
        }

        [TestMethod]
        public void Parse_ValidConnectionStringWithSpecialCharacters_Parsed()
        {
            var builder = DocumentDbConnectionStringBuilder.Parse("AccountEndpoint=http://TestDocumentDbUrl.net;AccountKey=\"Super;S=ecret\"");

            Assert.AreEqual("http://TestDocumentDbUrl.net", builder.AccountEndpoint, TestResources.InvalidAccountEndpointParsedFromConnectionString);
            Assert.AreEqual("Super;S=ecret", builder.AccountKey, TestResources.InvalidAccountKeyParsedFromConnectionString);
            Assert.IsNull(builder.Database, TestResources.InvalidDatabaseNameParsedFromConnectionString);
        }

        [TestMethod]
        public void Parse_ConnectionStringWithUnknownArguments_Parsed()
        {
            var builder = DocumentDbConnectionStringBuilder.Parse("AccountEndpoint=http://Test2DocumentDbUrl.net;AccountKey=SuperSecret;SomeRandomArgument=Value");

            Assert.AreEqual("http://Test2DocumentDbUrl.net", builder.AccountEndpoint, TestResources.InvalidAccountEndpointParsedFromConnectionString);
            Assert.AreEqual("SuperSecret", builder.AccountKey, TestResources.InvalidAccountKeyParsedFromConnectionString);
            Assert.IsNull(builder.Database, TestResources.InvalidDatabaseNameParsedFromConnectionString);
        }

        [TestMethod]
        public void Parse_ConnectionStringWithDatabase_Parsed()
        {
            var builder = DocumentDbConnectionStringBuilder.Parse("AccountEndpoint=http://Test2DocumentDbUrl.net;AccountKey=SuperSecret;Database=TestDb");
            
            Assert.AreEqual("http://Test2DocumentDbUrl.net", builder.AccountEndpoint, TestResources.InvalidAccountEndpointParsedFromConnectionString);
            Assert.AreEqual("SuperSecret", builder.AccountKey, TestResources.InvalidAccountKeyParsedFromConnectionString);
            Assert.AreEqual("TestDb", builder.Database, TestResources.InvalidDatabaseNameParsedFromConnectionString);
        }
    }
}
