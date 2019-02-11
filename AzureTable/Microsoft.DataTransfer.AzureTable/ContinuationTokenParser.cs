using System;
using System.Text;

namespace Microsoft.DataTransfer.AzureTable
{
    /// <summary>
    /// The class which is used to encode the continuation token
    /// </summary>
    public class ContinuationTokenParser
    {
        /// <summary>
        /// Encode continuation token to fomart: (Version)!(TokenLength)!(CustomBase64EncodedToken)
        /// </summary>
        public static string EncodeContinuationToken(string key)
        {
            StringBuilder encodedContinuationToken = new StringBuilder();
            // Version of the ContinuationToken
            encodedContinuationToken.Append(1);
            encodedContinuationToken.Append(exclamationDelimiter);

            UTF8Encoding utf8Encoding = new UTF8Encoding();
            string base64EncodedToken = Convert.ToBase64String(utf8Encoding.GetBytes(key.ToString()));
            string customBase64EncodedString = UrlCustomEscapeBase64String(base64EncodedToken);

            //Size is the lenght of base64 encoded key
            encodedContinuationToken.Append(customBase64EncodedString.Length);
            encodedContinuationToken.Append(exclamationDelimiter);

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

        private const char exclamationDelimiter = '!';
    }
}
