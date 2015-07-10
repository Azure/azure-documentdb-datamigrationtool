using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.HBase.Client.Authentication;
using System;

namespace Microsoft.DataTransfer.HBase.Client
{
    static class StargateClientFactory
    {
        public static IStargateClient Create(string connectionString)
        {
            Guard.NotEmpty("connectionString", connectionString);

            var connectionSettings = StargateConnectionStringBuilder.Parse(connectionString);

            if (String.IsNullOrEmpty(connectionSettings.ServiceURL))
                throw Errors.ServiceUrlMissing();

            return new StargateClient(connectionSettings.ServiceURL,
                String.IsNullOrEmpty(connectionSettings.Username)
                    ? (IRestAuthentication)NoRestAuthentication.Instance
                    : new BasicRestAuthentication(connectionSettings.Username, connectionSettings.Password));
        }
    }
}
