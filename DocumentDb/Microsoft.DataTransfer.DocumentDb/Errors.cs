using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Exceptions;
using System;
using System.Runtime.CompilerServices;

namespace Microsoft.DataTransfer.DocumentDb
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception ConnectionStringMissing()
        {
            return new ArgumentException(Resources.ConnectionStringMissing);
        }

        public static Exception CollectionNameMissing()
        {
            return new ArgumentException(Resources.CollectionNameMissing);
        }

        public static Exception AccountEndpointMissing()
        {
            return new ArgumentException(Resources.AccountEndpointMissing);
        }

        public static Exception AccountKeyMissing()
        {
            return new ArgumentException(Resources.AccountKeyMissing);
        }

        public static Exception DatabaseNameMissing()
        {
            return new ArgumentException(Resources.DatabaseNameMissing);
        }

        public static Exception AmbiguousIndexingPolicy()
        {
            return new ArgumentException(Resources.AmbiguousIndexingPolicy);
        }

        public static Exception InvalidCollectionThroughput()
        {
            return new ArgumentException(Resources.InvalidCollectionThroughput);
        }

        public static Exception InvalidNumberOfParallelRequests()
        {
            return new ArgumentException(Resources.InvalidNumberOfParallelRequests);
        }

        public static Exception InvalidNumberOfRetries()
        {
            return new ArgumentException(Resources.InvalidNumberOfRetries);
        }

        public static Exception InvalidRetryInterval()
        {
            return new ArgumentException(Resources.InvalidRetryInterval);
        }

        public static Exception InvalidBatchSize()
        {
            return new ArgumentException(Resources.InvalidBatchSize);
        }

        public static Exception InvalidMaxScriptSize()
        {
            return new ArgumentException(Resources.InvalidMaxScriptSize);
        }

        public static Exception AmbiguousQuery()
        {
            return new ArgumentException(Resources.AmbiguousQuery);
        }

        public static Exception FailedToCreateCollection(string name)
        {
            return new InvalidOperationException(FormatMessage(Resources.FailedToCreateCollectionFormat, name));
        }

        public static Exception FailedToCreateDatabase(string name)
        {
            return new InvalidOperationException(FormatMessage(Resources.FailedToCreateDatabaseFormat, name));
        }

        public static Exception FailedToCreateStoredProcedure(string name)
        {
            return new InvalidOperationException(FormatMessage(Resources.FailedToCreateStoredProcedureFormat, name));
        }

        public static Exception SinkIsNotInitialized()
        {
            return new InvalidOperationException(Resources.SinkIsNotInitialized);
        }

        public static Exception SourceIsNotInitialized()
        {
            return new InvalidOperationException(Resources.SourceIsNotInitialized);
        }

        public static Exception BufferSlotIsOccupied()
        {
            return new ArgumentException(Resources.BufferSlotIsOccupied);
        }

        public static Exception FailedToCreateDocument(string message)
        {
            return new FailedToCreateDocumentException(message);
        }

        public static Exception DataItemAlreadyContainsField(string fieldName)
        {
            return new InvalidOperationException(FormatMessage(Resources.DataItemAlreadyContainsField, fieldName));
        }

        public static Exception DocumentSizeExceedsBulkScriptSize()
        {
            return new DocumentSizeExceedsScriptSizeLimitException(Resources.DocumentSizeExceedsBulkScriptSize);
        }

        public static Exception OperationNotSupported([CallerMemberName] string operationName = null)
        {
            return new NotSupportedException(FormatMessage(Resources.OperationNotSupportedFormat, operationName));
        }

        public static Exception UnexpectedCharacter(int position, char character)
        {
            return new FormatException(FormatMessage(Resources.UnexpectedCharacterFormat, position, character));
        }

        public static Exception FailedToExtractPartitionKey(string message)
        {
            return new InvalidOperationException(FormatMessage(Resources.FailedToExtractPartitionKeyFormat, message));
        }

        public static Exception UnexpectedPartitionCollection(string collectionName)
        {
            return new ArgumentException(FormatMessage(Resources.UnexpectedPartitionCollectionFormat, collectionName));
        }

        public static Exception FailedToReadSubstituion(string message)
        {
            return new FormatException(FormatMessage(Resources.FailedToReadSubstituionFormat, message));
        }

        public static Exception FailedToReadJavascriptMemberExpression(string message)
        {
            return new FormatException(FormatMessage(Resources.FailedToReadJavascriptMemberExpressionFormat, message));
        }

        public static Exception NotANumber(string value)
        {
            return new FormatException(FormatMessage(Resources.NotANumberFormat, value));
        }

        public static Exception InvalidRange(int start, int end)
        {
            return new ArgumentException(FormatMessage(Resources.InvalidRangeFormat, start, end));
        }

        public static Exception UnexpectedAsyncFlushError(Exception error)
        {
            return new Exception(FormatMessage(Resources.UnexpectedAsyncFlushErrorMessageFormat, error.Message), error);
        }
    }
}
