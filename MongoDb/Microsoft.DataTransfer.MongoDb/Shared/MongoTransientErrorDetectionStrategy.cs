using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using MongoDB.Driver;
using MongoDB.Driver.Communication.Security;
using System;

namespace Microsoft.DataTransfer.MongoDb.Shared
{
    sealed class MongoTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            // If it is MongoConnectionException...
            return HasException<MongoConnectionException>(ex) &&
                // ..and not one of these
                !HasException<MongoAuthenticationException>(ex) &&
                !HasException<MongoInternalException>(ex) &&
                !HasException<MongoSecurityException>(ex);
        }

        private bool HasException<T>(Exception root)
            where T : Exception
        {
            if (root == null)
                return false;

            return root is T || HasException<T>(root.InnerException);
        }
    }
}
