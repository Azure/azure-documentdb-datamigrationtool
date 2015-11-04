using Microsoft.DataTransfer.ServiceModel.Errors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Steps.Import
{
    sealed class ExportErrorsToClipboardCommand : ExportErrorsCommandBase
    {
        protected override void PersistErrors(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            var errorsText = new StringBuilder();

            foreach (var error in errors)
                errorsText.AppendLine(EscapeValue(error.Key) + "\t" + EscapeValue(error.Value));

            Clipboard.SetText(errorsText.ToString());
        }

        private static string EscapeValue(string value)
        {
            return value.Replace("\t", " ").Replace(Environment.NewLine, " ");
        }
    }
}
