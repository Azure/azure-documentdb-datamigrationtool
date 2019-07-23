using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.DataTransfer.AzureTable.UnitTests
{
    [TestClass]
    public class ContinuationTokenParserTests
    {
        [TestMethod]
        public void EncodeContinuationToken_StringEncoded()
        {
            var testString = "test";
            var encodedToken = ContinuationTokenParser.EncodeContinuationToken(testString);

            Assert.AreEqual("1!8!dGVzdA--", encodedToken, "The encoded token should be as expected.");
        }
    }
}
