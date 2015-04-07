using Microsoft.DataTransfer.WpfHost.Basics.ValueConverters;
using System;
using System.Globalization;

namespace Microsoft.DataTransfer.WpfHost.Steps.Import
{
    sealed class ElapsedTimeFormatValueConverter : ValueConverterBase<TimeSpan, string>
    {
        protected override string Convert(TimeSpan value, object parameter, CultureInfo culture)
        {
            return String.Format(culture, @"{0:0}:{1:mm\:ss\.f}", Math.Floor(value.TotalHours), value);
        }

        protected override TimeSpan ConvertBack(string value, object parameter, CultureInfo culture)
        {
            return TimeSpan.Parse(value, culture);
        }
    }
}
