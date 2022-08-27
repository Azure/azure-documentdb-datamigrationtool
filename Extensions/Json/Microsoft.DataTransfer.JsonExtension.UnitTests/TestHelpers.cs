using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.JsonExtension.UnitTests
{
    public static class TestHelpers
    {
        public static IConfiguration CreateConfig(Dictionary<string, string> values)
        {
            return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
        }
    }
}