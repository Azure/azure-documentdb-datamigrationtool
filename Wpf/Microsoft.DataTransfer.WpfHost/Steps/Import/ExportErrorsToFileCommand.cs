using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.DataTransfer.WpfHost.Steps.Import
{
    sealed class ExportErrorsToFileCommand : ExportErrorsCommandBase
    {
        protected override void PersistErrors(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                OverwritePrompt = true,
                ValidateNames = true,
                Filter = ExportErrorsResources.FileFilter,
                DefaultExt = ExportErrorsResources.DefaultFileExtension
            };

            if (dialog.ShowDialog() != true)
                return;

            using (var file = File.CreateText(dialog.FileName))
            {
                file.WriteLine(ExportErrorsResources.CsvFileHeader);

                foreach (var error in errors)
                    file.WriteLine(EscapeValue(error.Key) + "," + EscapeValue(error.Value));
            }
        }

        private static string EscapeValue(string value)
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
