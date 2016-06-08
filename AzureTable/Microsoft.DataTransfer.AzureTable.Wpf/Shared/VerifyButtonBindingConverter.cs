using Microsoft.DataTransfer.AzureTable.Source;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Shared
{
    /// <summary>
    /// This is a binding converter to convert a Connection String and Location Mode to a single parameter, so we can pass it to the Test Command
    /// </summary>
    public class VerifyButtonBindingConverter : IMultiValueConverter
    {    
        /// <summary>
        /// Override of the IMultiValue Converter Convert function
        /// </summary>
        /// <param name="values">Values passed from the button</param>
        /// <param name="targetType">Default param</param>
        /// <param name="parameter">Default param</param>
        /// <param name="culture">Default param</param>
        /// <returns></returns>
 
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
         {
             if(values.Length > 1)
             {
                 return new AzureTableProbeClientParameter() { ConnectionString = values[0] as string, LocationMode = values[1] as AzureTableLocationMode? };
             }

             return null;
         }

        /// <summary>
        /// Override of the IMultiValue Converter ConvertBack function. NOT IMPLEMENTED
        /// </summary>
        /// <param name="value">Default param</param>
        /// <param name="targetTypes">Default param</param>
        /// <param name="parameter">Default param</param>
        /// <param name="culture">Default param</param>
        /// <returns></returns>
         public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
         {
             throw new NotImplementedException();
         }
    }
}
