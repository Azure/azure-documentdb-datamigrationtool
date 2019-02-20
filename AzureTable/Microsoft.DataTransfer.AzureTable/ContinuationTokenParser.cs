using System;
using System.Text;

namespace Microsoft.DataTransfer.AzureTable
{
    /// <summary>
    /// The class which is used to encode the continuation token
    /// </summary>
    public static class ContinuationTokenParser
    {
        private const char ExclamationDelimiter = '!';

        /// <summary>
        /// Generates the encoded continuation token with fomart (Version)!(TokenLength)!(CustomBase64EncodedToken)
        /// </summary>
        /// <param name="key">The string that you want to encode into continuation token</param>
        /// <returns>The encoded continuation token</returns>
        public static string EncodeContinuationToken(string key)
        {
            StringBuilder encodedContinuationToken = new StringBuilder();
            // Version of the ContinuationToken
            encodedContinuationToken.Append(1);
            encodedContinuationToken.Append(ExclamationDelimiter);

            string base64EncodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(key.ToString()));
            string customBase64EncodedString = UrlCustomEscapeBase64String(base64EncodedToken);

            // Size is the lenght of base64 encoded key
            encodedContinuationToken.Append(customBase64EncodedString.Length);
            encodedContinuationToken.Append(ExclamationDelimiter);

            encodedContinuationToken.Append(customBase64EncodedString);
            return encodedContinuationToken.ToString();
        }

        private static string UrlCustomEscapeBase64String(string token)
        {
            StringBuilder escapedString = new StringBuilder();
            foreach (char c in token.ToCharArray())
            {
                escapedString.Append(TranslateChar(c));
            }

            return escapedString.ToString();
        }

        private static char TranslateChar(char c)
        {
            switch (c)
            {
                case '/': return '_';
                case '+': return '*';
                case '=': return '-';
                default: return c;
            }
        }
    }
}
