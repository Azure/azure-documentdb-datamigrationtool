using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using MongoDB.Driver;
using System;

namespace Microsoft.DataTransfer.MongoDb.Shared
{
    sealed class MongoTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            return
                // If it we use incompatible driver...
                HasException<MongoIncompatibleDriverException>(ex) ||
                    // ..or it is MongoConnectionException...
                    (HasException<MongoConnectionException>(ex) &&
                    // ..but not authentication issue
                    !HasException<MongoAuthenticationException>(ex));
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
