using Microsoft.DataTransfer.Basics;
using System.IO;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker
{
    sealed class AddSingleFolderCommand : AddFolderCommandBase
    {
        protected override string GetFolderSearchPattern(string folderPath)
        {
            return PathHelper.Combine(folderPath, @"*.*");
        }
    }
}
