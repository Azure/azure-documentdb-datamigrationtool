using System;
using System.Globalization;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Dates
{
    static class DateTimeConverter
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToEpoch(DateTime timeStamp)
        {
            return Convert.ToInt64(timeStamp.Subtract(Epoch).TotalSeconds);
        }

        public static string ToString(DateTime timeStamp)
        {
            return timeStamp.ToString("O", CultureInfo.InvariantCulture);
        }
    }
}
